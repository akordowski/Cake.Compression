#load "./Precondition.cake"

public class Project
{
    public string Name { get; }
    public AssemblyInfoParseResult AssemblyInfo { get; }
    public FilePath InfoFile { get; }
    public FilePathCollection ProjectFiles { get; }
    public FilePathCollection TestProjectFiles { get; }
    public FilePath SolutionFile { get; }

    public Project(ICakeContext context, string name, FilePath infoFile)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNullOrWhiteSpace(name, nameof(name));
        Precondition.IsNotNull(infoFile, nameof(infoFile));
        
        Name = name;
        InfoFile = context.MakeAbsolute(infoFile);

        AssemblyInfo = context.ParseAssemblyInfo(InfoFile);
        SolutionFile = context.GetFiles("./**/*.sln").FirstOrDefault();
        ProjectFiles = context.GetFiles("./**/*.csproj");
        TestProjectFiles = context.GetFiles("./**/*.Tests.csproj");
    }

    public static Project GetInstance(ICakeContext context, string name, FilePath infoFile)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNullOrWhiteSpace(name, nameof(name));
        Precondition.IsNotNull(infoFile, nameof(infoFile));

        return new Project(
            context,
            name,
            infoFile);
    }
}