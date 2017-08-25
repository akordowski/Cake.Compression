/* ---------------------------------------------------------------------------------------------------- */
/* ARGUMENTS */

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


/* ---------------------------------------------------------------------------------------------------- */
/* PACKAGE VERSION */

var assemblyInfo = ParseAssemblyInfo("./src/Cake.Compression/Properties/AssemblyInfo.cs");
var version = assemblyInfo.AssemblyVersion;
var suffix = configuration == "Debug" ? "-dbg" : "";
var packageVersion = version + suffix;


/* ---------------------------------------------------------------------------------------------------- */
/* CONSTANTS */

var PROJECT = "Cake.Compression";
var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var BIN_DIR = PROJECT_DIR + "src/" + PROJECT + "/bin/" + configuration + "/";
var IMAGE_DIR = PROJECT_DIR + "image/" + PROJECT + "-" + packageVersion + "/";
var PACKAGE_DIR = PROJECT_DIR + "package/";

// Packages
var SRC_PACKAGE = PACKAGE_DIR + PROJECT + "-" + version + "-src.zip";
var ZIP_PACKAGE = PACKAGE_DIR + PROJECT + "-" + packageVersion + ".zip";


/* ---------------------------------------------------------------------------------------------------- */
/* HELPER METHODS - GENERAL */

void RunGitCommand(string arguments)
{
    StartProcess("git", new ProcessSettings()
    {
        Arguments = arguments
    });
}


/* ---------------------------------------------------------------------------------------------------- */
/* HELPER METHODS - BUILD */

void BuildProject(string projectPath, string configuration)
{
    BuildProject(projectPath, configuration, MSBuildPlatform.Automatic);
}

void BuildProject(string projectPath, string configuration, MSBuildPlatform buildPlatform)
{
    if(IsRunningOnWindows())
    {
        // Use MSBuild
        MSBuild(projectPath, new MSBuildSettings()
            .SetConfiguration(configuration)
            .SetMSBuildPlatform(buildPlatform)
            .SetVerbosity(Verbosity.Minimal)
            .SetNodeReuse(false)
        );
    }
    else
    {
        // Use XBuild
        XBuild(projectPath, new XBuildSettings()
            .WithTarget("Build")
            .WithProperty("Configuration", configuration)
            .SetVerbosity(Verbosity.Minimal)
        );
    }
}


/* ---------------------------------------------------------------------------------------------------- */
/* BUILD */

Task("Build")
    .Does(() =>
    {
        BuildProject(string.Format("src/{0}.sln", PROJECT), configuration);
    });


/* ---------------------------------------------------------------------------------------------------- */
/* PACKAGE */

var RootFiles = new FilePath[]
{
    "LICENSE.txt"
};

var BinFiles = new FilePath[]
{
    "Cake.Compression.dll",
    "Cake.Compression.xml"
};

Task("CreateImage")
    .Does(() =>
    {
        CreateDirectory(IMAGE_DIR);
        CleanDirectory(IMAGE_DIR);

        CopyFiles(RootFiles, IMAGE_DIR);

        foreach (FilePath file in BinFiles)
        {
            var sourcePath = BIN_DIR + "/" + file;

            if (FileExists(sourcePath))
                CopyFileToDirectory(sourcePath, IMAGE_DIR);
        }
    });

Task("PackageSource")
  .Does(() =>
    {
        CreateDirectory(PACKAGE_DIR);
        RunGitCommand(string.Format("archive -o {0} HEAD", SRC_PACKAGE));
    });

Task("PackageZip")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
        CreateDirectory(PACKAGE_DIR);

        var zipFiles = GetFiles(IMAGE_DIR + "/*.*");

        Zip(IMAGE_DIR, File(ZIP_PACKAGE), zipFiles);
    });

Task("PackageNuGet")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
        CreateDirectory(PACKAGE_DIR);

        NuGetPack("nuget/" + PROJECT + ".nuspec", new NuGetPackSettings()
        {
            Version = packageVersion,
            BasePath = IMAGE_DIR,
            OutputDirectory = PACKAGE_DIR
        });
    });


/* ---------------------------------------------------------------------------------------------------- */
/* TASK TARGETS */

Task("Default")
    .IsDependentOn("Build");

Task("Package")
    .IsDependentOn("PackageSource")
    .IsDependentOn("PackageZip")
    .IsDependentOn("PackageNuGet");


/* ---------------------------------------------------------------------------------------------------- */
/* EXECUTION */

RunTarget(target);