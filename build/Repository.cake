#load "./Precondition.cake"

public class Repository
{
    public string Owner { get; }
    public string Name { get; }
    public string FullName { get; }
    public string Branch { get; }
    public bool IsMainRepo { get; }
    public bool IsMainBranch { get; }
    public bool IsPullRequest { get; }
    public bool IsTagged { get; }
    
    public bool CanPublish => !IsPullRequest && IsTagged && IsMainRepo && IsMainBranch;

    public Repository(ICakeContext context, string owner, string name, string branch)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNullOrWhiteSpace(owner, nameof(owner));
        Precondition.IsNotNullOrWhiteSpace(name, nameof(name));
        Precondition.IsNotNullOrWhiteSpace(branch, nameof(branch));
        
        Owner = owner;
        Name = name;
        FullName = $"{owner}/{name}";
        Branch = branch;

        var buildSystem = context.BuildSystem();

        if (buildSystem.AppVeyor.IsRunningOnAppVeyor)
        {
            var repo = buildSystem.AppVeyor.Environment.Repository;

            IsMainRepo = StringComparer.OrdinalIgnoreCase.Equals(FullName, repo.Name);
            IsMainBranch = StringComparer.OrdinalIgnoreCase.Equals(Branch, repo.Branch);
            IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
            IsTagged = repo.Tag.IsTag && !string.IsNullOrWhiteSpace(repo.Tag.Name);
        }
    }

    public static Repository GetInstance(ICakeContext context, string owner, string name, string branch)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNullOrWhiteSpace(owner, nameof(owner));
        Precondition.IsNotNullOrWhiteSpace(name, nameof(name));
        Precondition.IsNotNullOrWhiteSpace(branch, nameof(branch));

        return new Repository(
            context,
            owner,
            name,
            branch);
    }
}