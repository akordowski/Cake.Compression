#load "./Precondition.cake"
#load "./GitHubCredentials.cake"
#tool "nuget:?package=GitReleaseManager&version=0.7.0"

public class GitHub
{
    public GitHubCredentials Credentials { get; }
    public string Owner { get; }
    public string Repo { get; }

    private ICakeContext context;

    public GitHub(ICakeContext context, GitHubCredentials credentials, string owner, string repo)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNull(credentials, nameof(credentials));
        Precondition.IsNotNullOrWhiteSpace(owner, nameof(owner));
        Precondition.IsNotNullOrWhiteSpace(repo, nameof(repo));

        Credentials = credentials;
        Owner = owner;
        Repo = repo;

        this.context = context;
    }

    public static GitHub GetInstance(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));

        var owner = context.EnvironmentVariable("GITHUB_OWNER");
        var repo = context.EnvironmentVariable("GITHUB_REPO");

        Precondition.IsNotNullOrWhiteSpace(owner, "GITHUB_OWNER", "Could not resolve GITHUB_OWNER.");
        Precondition.IsNotNullOrWhiteSpace(repo, "GITHUB_REPO", "Could not resolve GITHUB_REPO.");

        var credentials = GitHubCredentials.GetInstance(context);

        return new GitHub(context, credentials, owner, repo);
    }

    public static string GetAssets(FilePathCollection files)
    {
        Precondition.IsNotNull(files, nameof(files));

        var arr = files.ToList().ConvertAll(f => f.ToString()).ToArray();
        var assets = String.Join(",", arr);

        return assets;
    }

    public static void CreateReleaseNotes(ReleaseNotes releaseNotes, FilePath fileOutputPath)
    {
        System.IO.File.WriteAllLines(fileOutputPath.FullPath, releaseNotes.Notes.ToArray());
    }

    public void AddAssets(string tagName, string assets)
    {
        Precondition.IsNotNullOrWhiteSpace(tagName, nameof(tagName));
        Precondition.IsNotNullOrWhiteSpace(assets, nameof(assets));

        context.GitReleaseManagerAddAssets(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            tagName,
            assets);
    }

    public void AddAssets(string tagName, string assets, GitReleaseManagerAddAssetsSettings settings)
    {
        Precondition.IsNotNullOrWhiteSpace(tagName, nameof(tagName));
        Precondition.IsNotNullOrWhiteSpace(assets, nameof(assets));
        Precondition.IsNotNull(settings, nameof(settings));

        context.GitReleaseManagerAddAssets(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            tagName,
            assets,
            settings);
    }

    public void Close(string milestone)
    {
        Precondition.IsNotNullOrWhiteSpace(milestone, nameof(milestone));

        context.GitReleaseManagerClose(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            milestone);
    }

    public void Close(string milestone, GitReleaseManagerCloseMilestoneSettings settings)
    {
        Precondition.IsNotNullOrWhiteSpace(milestone, nameof(milestone));
        Precondition.IsNotNull(settings, nameof(settings));

        context.GitReleaseManagerClose(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            milestone,
            settings);
    }

    public void Create()
    {
        context.GitReleaseManagerCreate(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo);
    }

    public void Create(GitReleaseManagerCreateSettings settings)
    {
        Precondition.IsNotNull(settings, nameof(settings));

        context.GitReleaseManagerCreate(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            settings);
    }

    public void Export(FilePath fileOutputPath)
    {
        Precondition.IsNotNull(fileOutputPath, nameof(fileOutputPath));

        context.GitReleaseManagerExport(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            fileOutputPath);
    }

    public void Export(FilePath fileOutputPath, GitReleaseManagerExportSettings settings)
    {
        Precondition.IsNotNull(fileOutputPath, nameof(fileOutputPath));
        Precondition.IsNotNull(settings, nameof(settings));

        context.GitReleaseManagerExport(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            fileOutputPath,
            settings);
    }

    public void Publish(string tagName)
    {
        Precondition.IsNotNullOrWhiteSpace(tagName, nameof(tagName));

        context.GitReleaseManagerPublish(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            tagName);
    }

    public void Publish(string tagName, GitReleaseManagerPublishSettings settings)
    {
        Precondition.IsNotNullOrWhiteSpace(tagName, nameof(tagName));
        Precondition.IsNotNull(settings, nameof(settings));

        context.GitReleaseManagerPublish(
            Credentials.Username,
            Credentials.Password,
            Owner,
            Repo,
            tagName,
            settings);
    }
}