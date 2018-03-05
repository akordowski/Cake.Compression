#load "./build/BuildParameters.cake"
#load "./build/AppVeyorCI.cake"
#load "./build/GitHub.cake"
#load "./build/NuGet.cake"
#load "./build/Twitter.cake"
#tool "nuget:?package=NUnit.ConsoleRunner"

/* ---------------------------------------------------------------------------------------------------- */
/* Arguments */

var parameters = BuildParameters.GetInstance(Context, "akordowski", "Cake.Compression", "master", "Cake.Compression", "./src/Cake.Compression/Properties/AssemblyInfo.cs");
var publishingError = false;

/* ---------------------------------------------------------------------------------------------------- */
/* Setup */

Setup(context =>
{
    if (parameters.Repository.CanPublish)
    {
        Information($"Building version {parameters.Version.SemVersion} of {parameters.Project.Name} (Configuration: {parameters.Configuration}, Target: {parameters.Target}, IsTagged: {parameters.Repository.IsTagged}).");
    }
    else
    {
        Information($"Building of {parameters.Project.Name} (Configuration: {parameters.Configuration}, Target: {parameters.Target}).");
    }
});

/* ---------------------------------------------------------------------------------------------------- */
/* Tasks */

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(parameters.Directories.Artifacts);
    });

Task("RestoreNuGet")
    .Does(() =>
    {
        NuGetRestore(parameters.Project.SolutionFile);
    });

Task("Build")
    .IsDependentOn("RestoreNuGet")
    .Does(() =>
    {
        MSBuild(parameters.Project.SolutionFile, settings => settings
            .SetConfiguration(parameters.Configuration)
            .SetVerbosity(Verbosity.Minimal)
            .WithProperty("Version", parameters.Version.SemVersion)
            .WithProperty("AssemblyVersion", parameters.Version.Version)
            .WithProperty("FileVersion", parameters.Version.Version));
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testFile = GetFiles("./src/**/bin/**/*.Tests.dll");

        NUnit3(testFile, new NUnit3Settings
        {
            NoHeader = true,
            NoResults = true
        });
    });

Task("CreateImage")
    .IsDependentOn("Clean")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var binDir = parameters.Directories.BinNet461;
        var filePath = $"./src/{parameters.Project.Name}/bin/{parameters.Configuration}/{parameters.Project.Name}";

        CleanDirectory(binDir);

        CopyFileToDirectory(parameters.Files.License, parameters.Directories.Bin);
        CopyFileToDirectory($"{filePath}.dll", binDir);
        CopyFileToDirectory($"{filePath}.xml", binDir);
    });

Task("PackageNuGet")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
        NuGetPack($"./nuspec/{parameters.Project.Name}.nuspec", new NuGetPackSettings
        {
            Version = parameters.Version.SemVersion,
            ReleaseNotes = parameters.ReleaseNotes.Notes.ToArray(),
            BasePath = parameters.Directories.Bin,
            OutputDirectory = parameters.Directories.NuGet,
            Files = NuGet.GetFiles(Context, parameters.Directories.Bin),
            Symbols = false
        });
    });

Task("PackageZip")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
        CleanDirectory(parameters.Directories.Zip);
        Zip(parameters.Directories.Bin, parameters.Files.ZipArtifacts);
    });

/* ---------------------------------------------------------------------------------------------------- */
/* Tasks Publish */

Task("PublishAppVeyor")
    .IsDependentOn("Package")
    .WithCriteria(() => parameters.Repository.CanPublish)
    .Does(() =>
    {
        var artifacts = GetFiles(parameters.Directories.NuGet + "/*")
                      + GetFiles(parameters.Directories.Zip + "/*");

        var appVeyor = AppVeyorCI.GetInstance(Context);
        appVeyor.UploadArtifacts(artifacts);
    })
    .OnError(exception =>
    {
        Warning("PublishAppVeyor Task failed, but continuing with next Task...");
        publishingError = true;
    });

Task("PublishNuGet")
    .IsDependentOn("Package")
    .WithCriteria(() => parameters.Repository.CanPublish)
    .Does(() =>
    {
        var nugetFiles = GetFiles(parameters.Directories.NuGet + "/*");

        var nuGet = NuGet.GetInstance(Context);
        nuGet.Push(nugetFiles);
    })
    .OnError(exception =>
    {
        Warning("PublishNuGet Task failed, but continuing with next Task...");
        publishingError = true;
    });

Task("PublishGitHub")
    .IsDependentOn("Package")
    .WithCriteria(() => parameters.Repository.CanPublish)
    .Does(() =>
    {
        var version = parameters.Version.SemVersion;
        var assets = GetFiles(parameters.Directories.NuGet + "/*")
                   + GetFiles(parameters.Directories.Zip + "/*");

        GitHub.CreateReleaseNotes(parameters.ReleaseNotes, parameters.Files.GitReleaseNotes);

        var gitHub = GitHub.GetInstance(Context);
        gitHub.Create(new GitReleaseManagerCreateSettings
        {
            Name = version,
            InputFilePath = parameters.Files.GitReleaseNotes,
            Prerelease = true,
            Assets = GitHub.GetAssets(assets),
            TargetCommitish = parameters.Repository.Branch
        });
        gitHub.Publish(version);
    })
    .OnError(exception =>
    {
        Warning("PublishGitHub Task failed, but continuing with next Task...");
        publishingError = true;
    });

/* ---------------------------------------------------------------------------------------------------- */
/* Tasks Send Message */

Task("SendTwitterMessage")
    .WithCriteria(() => parameters.Repository.CanPublish && !publishingError)
    .Does(() =>
    {
        var message = $"Version {parameters.Version.SemVersion} of {parameters.Project.Name} has just been released, https://www.nuget.org/packages/{parameters.Project.Name}.";

        var twitter = Twitter.GetInstance(Context);
        twitter.SendMessage(message);
    })
    .OnError(exception =>
    {
        Warning("SendTwitterMessage Task failed, but continuing with next Task...");
        publishingError = true;
    });

/* ---------------------------------------------------------------------------------------------------- */
/* Tasks Targets */

Task("Default")
    .IsDependentOn("Build");

Task("Package")
    .IsDependentOn("PackageNuGet")
    .IsDependentOn("PackageZip");

Task("Publish")
    .IsDependentOn("PublishAppVeyor")
    .IsDependentOn("PublishNuGet")
    .IsDependentOn("PublishGitHub");

Task("SendMessage")
    .IsDependentOn("SendTwitterMessage");

var taskAppVeyor = Task("AppVeyor")
    .IsDependentOn("Test")
    .Finally(() =>
    {
        if (publishingError)
        {
            throw new Exception("An error occurred during the publishing. All publishing tasks have been attempted.");
        }
    });

if (parameters.Repository.CanPublish)
{
    taskAppVeyor.IsDependentOn("Package");
    taskAppVeyor.IsDependentOn("Publish");
    taskAppVeyor.IsDependentOn("SendMessage");
}

/* ---------------------------------------------------------------------------------------------------- */
/* Execution */

RunTarget(parameters.Target);