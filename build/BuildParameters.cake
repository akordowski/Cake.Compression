#load "./Precondition.cake"
#load "./BuildDirectories.cake"
#load "./BuildFiles.cake"
#load "./BuildVersion.cake"
#load "./Project.cake"
#load "./Repository.cake"

public class BuildParameters
{
    public string Target { get; private set; }
    public string Configuration { get; private set; }

    public BuildDirectories Directories { get; private set; }
    public BuildFiles Files { get; private set; }
    public BuildVersion Version { get; private set; }
    public Project Project { get; private set; }
    public Repository Repository { get; private set; }
    public ReleaseNotes ReleaseNotes { get; private set; }
    
    public static BuildParameters GetInstance(
        ICakeContext context,
        string repositoryOwner,
        string repositoryName,
        string repositoryBranch,
        string projectName,
        string projectInfoFile)
    {
        Precondition.IsNotNull(context, nameof(context));

        var repository = Repository.GetInstance(context, repositoryOwner, repositoryName, repositoryBranch);
        var project = Project.GetInstance(context, projectName, projectInfoFile);
        var version = BuildVersion.GetInstance(context, projectInfoFile);
        var directories = BuildDirectories.GetInstance(context);
        var files = BuildFiles.GetInstance(context, directories, project.Name, version.SemVersion);
        
        return new BuildParameters
        {
            Target = context.Argument("target", "Default"),
            Configuration = context.Argument("configuration", "Release"),
            Directories = directories,
            Files = files,
            Version = version,
            Project = project,
            Repository = repository,
            ReleaseNotes = context.ParseReleaseNotes(files.ReleaseNotes)
        };
    }
}