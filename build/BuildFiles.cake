#load "./Precondition.cake"

public class BuildFiles
{
    public FilePath License { get; private set; }
    public FilePath ReleaseNotes { get; private set; }
    public FilePath GitReleaseNotes { get; private set; }
    public FilePath TestCoverage { get; private set; }
    public FilePath TestResult { get; private set; }
    public FilePath ZipArtifacts { get; private set; }
    public FilePath ZipArtifactsNet { get; private set; }
    public FilePath ZipArtifactsNetCore { get; private set; }
    public FilePath ZipArtifactsNetStandard { get; private set; }
    public FilePath ZipDocs { get; private set; }

    public static BuildFiles GetInstance(ICakeContext context, BuildDirectories directories, string projectName, string version)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNull(directories, nameof(directories));
        Precondition.IsNotNullOrWhiteSpace(projectName, nameof(projectName));
        Precondition.IsNotNullOrWhiteSpace(version, nameof(version));
        
        return new BuildFiles
        {
            License = (FilePath)context.File("./LICENSE.txt"),
            ReleaseNotes = (FilePath)context.File("./RELEASENOTES.md"),
            GitReleaseNotes = directories.Artifacts.CombineWithFilePath("./GITRELEASENOTES.md"),
            TestCoverage = directories.TestCoverage.CombineWithFilePath("OpenCover.xml"),
            TestResult = directories.TestResult.CombineWithFilePath("TestResult.xml"),
            ZipArtifacts = directories.Zip.CombineWithFilePath($"{projectName}.{version}.zip"),
            ZipArtifactsNet = directories.Zip.CombineWithFilePath($"{projectName}.dotnet.{version}.zip"),
            ZipArtifactsNetCore = directories.Zip.CombineWithFilePath($"{projectName}.coreclr.{version}.zip"),
            ZipArtifactsNetStandard = directories.Zip.CombineWithFilePath($"{projectName}.standard.{version}.zip"),
            ZipDocs = directories.Docs.CombineWithFilePath($"Docs.{projectName}.{version}.zip")
        };
    }
}