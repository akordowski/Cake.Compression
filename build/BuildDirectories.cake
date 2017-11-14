#load "./Precondition.cake"

public class BuildDirectories
{
    public DirectoryPath Artifacts { get; private set; }
    public DirectoryPath Bin { get; private set; }
    public DirectoryPath BinNet20 { get; private set; }
    public DirectoryPath BinNet30 { get; private set; }
    public DirectoryPath BinNet35 { get; private set; }
    public DirectoryPath BinNet40 { get; private set; }
    public DirectoryPath BinNet45 { get; private set; }
    public DirectoryPath BinNet451 { get; private set; }
    public DirectoryPath BinNet452 { get; private set; }
    public DirectoryPath BinNet46 { get; private set; }
    public DirectoryPath BinNet461 { get; private set; }
    public DirectoryPath BinNet462 { get; private set; }
    public DirectoryPath BinNetCore10 { get; private set; }
    public DirectoryPath BinNetCore11 { get; private set; }
    public DirectoryPath BinNetCore20 { get; private set; }
    public DirectoryPath BinNetStandard10 { get; private set; }
    public DirectoryPath BinNetStandard11 { get; private set; }
    public DirectoryPath BinNetStandard12 { get; private set; }
    public DirectoryPath BinNetStandard13 { get; private set; }
    public DirectoryPath BinNetStandard14 { get; private set; }
    public DirectoryPath BinNetStandard15 { get; private set; }
    public DirectoryPath BinNetStandard16 { get; private set; }
    public DirectoryPath BinNetStandard20 { get; private set; }
    public DirectoryPath Docs { get; private set; }
    public DirectoryPath NuGet { get; private set; }
    public DirectoryPath TestCoverage { get; private set; }
    public DirectoryPath TestResult { get; private set; }
    public DirectoryPath Zip { get; private set; }

    public static BuildDirectories GetInstance(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));
        
        var artifacts = context.MakeAbsolute((DirectoryPath)context.Directory("./artifacts"));
        var bin = artifacts.Combine("bin");
        
        return new BuildDirectories
        {
            Artifacts = artifacts,
            Bin = bin,
            BinNet20 = bin.Combine("net20"),
            BinNet30 = bin.Combine("net30"),
            BinNet35 = bin.Combine("net35"),
            BinNet40 = bin.Combine("net40"),
            BinNet45 = bin.Combine("net45"),
            BinNet451 = bin.Combine("net451"),
            BinNet452 = bin.Combine("net452"),
            BinNet46 = bin.Combine("net46"),
            BinNet461 = bin.Combine("net461"),
            BinNet462 = bin.Combine("net462"),
            BinNetCore10 = bin.Combine("netcoreapp1.0"),
            BinNetCore11 = bin.Combine("netcoreapp1.1"),
            BinNetCore20 = bin.Combine("netcoreapp2.0"),
            BinNetStandard10 = bin.Combine("netstandard1.0"),
            BinNetStandard11 = bin.Combine("netstandard1.1"),
            BinNetStandard12 = bin.Combine("netstandard1.2"),
            BinNetStandard13 = bin.Combine("netstandard1.3"),
            BinNetStandard14 = bin.Combine("netstandard1.4"),
            BinNetStandard15 = bin.Combine("netstandard1.5"),
            BinNetStandard16 = bin.Combine("netstandard1.6"),
            BinNetStandard20 = bin.Combine("netstandard2.0"),
            Docs = artifacts.Combine("docs"),
            NuGet = artifacts.Combine("nuget"),
            TestCoverage = artifacts.Combine("testCoverage"),
            TestResult = artifacts.Combine("testResult"),
            Zip = artifacts.Combine("zip")
        };
    }
}