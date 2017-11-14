#load "./Precondition.cake"

public class GitHubCredentials
{
    public string Username { get; }
    public string Password { get; }

    public GitHubCredentials(string username, string password)
    {
        Precondition.IsNotNullOrWhiteSpace(username, nameof(username));
        Precondition.IsNotNullOrWhiteSpace(password, nameof(password));
        
        Username = username;
        Password = password;
    }

    public static GitHubCredentials GetInstance(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));
        
        var username = context.EnvironmentVariable("GITHUB_USERNAME");
        var password = context.EnvironmentVariable("GITHUB_PASSWORD");

        Precondition.IsNotNullOrWhiteSpace(username, "GITHUB_USERNAME", "Could not resolve GITHUB_USERNAME.");
        Precondition.IsNotNullOrWhiteSpace(password, "GITHUB_PASSWORD", "Could not resolve GITHUB_PASSWORD.");

        return new GitHubCredentials(username, password);
    }
}