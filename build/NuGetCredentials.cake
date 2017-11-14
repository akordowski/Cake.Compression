#load "./Precondition.cake"

public class NuGetCredentials
{
    public string ApiKey { get; }
    public string ApiUrl { get; }

    public NuGetCredentials(string apiKey, string apiUrl)
    {
        Precondition.IsNotNullOrWhiteSpace(apiKey, nameof(apiKey));
        Precondition.IsNotNullOrWhiteSpace(apiUrl, nameof(apiUrl));
        
        ApiKey = apiKey;
        ApiUrl = apiUrl;
    }

    public static NuGetCredentials GetInstance(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));

        var apiKey = context.EnvironmentVariable("NUGET_API_KEY");
        var apiUrl = context.EnvironmentVariable("NUGET_API_URL");

        Precondition.IsNotNullOrWhiteSpace(apiKey, "NUGET_API_KEY", "Could not resolve NUGET_API_KEY.");
        Precondition.IsNotNullOrWhiteSpace(apiUrl, "NUGET_API_URL", "Could not resolve NUGET_API_URL.");
        
        return new NuGetCredentials(apiKey, apiUrl);
    }
}