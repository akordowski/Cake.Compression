#load "./Precondition.cake"

public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string Milestone { get; private set; }
    
    public static BuildVersion GetInstance(ICakeContext context, FilePath projectInfoFile)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNull(projectInfoFile, nameof(projectInfoFile));

        string version = ReadProjectInfoVersion(context, projectInfoFile);
        string semVersion = version;
        string milestone = version;
        
        return new BuildVersion
        {
            Version = version,
            SemVersion = semVersion,
            Milestone = milestone
        };
    }

    private static string ReadProjectInfoVersion(ICakeContext context, FilePath projectInfoFile)
    {
        var projectInfo = context.ParseAssemblyInfo(projectInfoFile);

        if (string.IsNullOrEmpty(projectInfo.AssemblyVersion))
        {
            throw new CakeException("Could not parse version.");
        }
        
        return projectInfo.AssemblyVersion;
    }
}