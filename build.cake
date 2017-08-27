#tool "nuget:?package=NUnit.ConsoleRunner"

/* ---------------------------------------------------------------------------------------------------- */
/* Arguments */

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

/* ---------------------------------------------------------------------------------------------------- */
/* Constants */

const string Project = "Cake.Compression";
const string Solution = "./src/" + Project + ".sln";

/* ---------------------------------------------------------------------------------------------------- */
/* Methods */

AssemblyInfoParseResult GetAssemblyInfo()
{
    return ParseAssemblyInfo(string.Format("./src/{0}/Properties/AssemblyInfo.cs", Project));
}

string GetPackageName()
{
    var assemblyInfo = GetAssemblyInfo();
    var version = assemblyInfo.AssemblyVersion;
    var suffix = configuration == "Debug" ? "-dbg" : "";
    var packageName = string.Format("{0}-{1}{2}", Project, version, suffix);
    
    return packageName;
}

/* ---------------------------------------------------------------------------------------------------- */
/* Tasks */

Task("RestoreNuGet")
    .Description("Restore NuGet packages.")
    .Does(() =>
    {
        NuGetRestore(Solution);
    });

Task("Build")
    .Description("Builds the solution.")
    .IsDependentOn("RestoreNuGet")
    .Does(() =>
    {
        DotNetBuild(Solution, settings =>
            settings
                .SetConfiguration(configuration)
                .SetVerbosity(Verbosity.Minimal)
                .WithTarget("Build"));
    });

Task("Test")
    .Description("Tests the project.")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testFile = string.Format("./src/{0}.Tests/bin/{1}/{0}.Tests.dll", Project, configuration);
        
        NUnit3(testFile, new NUnit3Settings
        {
            NoResults = true
        });
    });

Task("CreateImage")
    .Description("Copies all files into the image directory")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var packageName = GetPackageName();
        var imageDir = string.Format("./build/{0}/", packageName);
        var binDirFile = string.Format("./src/{0}/bin/{1}/{0}", Project, configuration);
        
        var files = new FilePath[]
        {
            "LICENSE.txt",
            binDirFile + ".dll",
            binDirFile + ".xml"
        };
        
        CleanDirectory("./build/");
        CreateDirectory(imageDir);
        CopyFiles(files, imageDir);
    });

Task("PackageNuGet")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
        var assemblyInfo = GetAssemblyInfo();
        var packageName = GetPackageName();
        var imageDir = string.Format("./{0}/", packageName);

        var nuGetPackSettings = new NuGetPackSettings
        {
            Id              = assemblyInfo.Title,
            Version         = assemblyInfo.AssemblyVersion,
            Title           = assemblyInfo.Title,
            Authors         = new[] { "Artur Kordowski" },
            Owners          = new[] { "Artur Kordowski" },
            Description     = "A Cake AddIn which provides compression functionality for BZip2, GZip and Zip.",
            Summary         = "A Cake AddIn which provides compression functionality for BZip2, GZip and Zip.",
            ProjectUrl      = new Uri("https://github.com/akordowski/Cake.Compression"),
            IconUrl         = new Uri("https://cdn.rawgit.com/cake-contrib/graphics/a5cf0f881c390650144b2243ae551d5b9f836196/png/cake-contrib-medium.png"),
            LicenseUrl      = new Uri("https://github.com/akordowski/Cake.Compression/blob/master/LICENSE.txt"),
            Copyright       = assemblyInfo.Copyright,
            Tags            = new[] { "cake", "build", "compression", "bzip2", "gzip", "tar", "zip" },
            Files           = new []
            {
                new NuSpecContent { Source = string.Format("{0}LICENSE.txt", imageDir) },
                new NuSpecContent { Source = string.Format("{0}{1}.dll", imageDir, Project), Target = @"lib\net45\" },
                new NuSpecContent { Source = string.Format("{0}{1}.xml", imageDir, Project), Target = @"lib\net45\" }
            },
            Dependencies    = new[]
            {
                new NuSpecDependency { Id = "SharpZipLib", Version = "0.86.0" }
            },
            OutputDirectory = "./build"
        };

        NuGetPack(nuGetPackSettings);
    });

Task("PackageZip")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
        var packageName = GetPackageName();
        var imageDir = string.Format("./build/{0}/", packageName);
        var zipFile = string.Format("./build/{0}.zip", packageName);
        
        Zip(imageDir, zipFile);
    });

/* ---------------------------------------------------------------------------------------------------- */
/* Tasks Targets */

Task("Default")
    .Description("Builds the project")
    .IsDependentOn("Build");

Task("Package")
    .Description("Packages the project")
    .IsDependentOn("PackageNuGet")
    .IsDependentOn("PackageZip");

Task("Appveyor")
    .Description("Builds, tests and packages on AppVeyor")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

/* ---------------------------------------------------------------------------------------------------- */
/* Execution */

RunTarget(target);