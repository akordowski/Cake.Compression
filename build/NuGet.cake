#load "./Precondition.cake"
#load "./NuGetCredentials.cake"

public class NuGet
{
    public NuGetCredentials Credentials { get; }
    public string ApiKey { get; }
    public string ApiUrl { get; }
    
    private ICakeContext context;

    public NuGet(ICakeContext context, NuGetCredentials credentials, string apiKey, string apiUrl)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNull(credentials, nameof(credentials));
        Precondition.IsNotNullOrWhiteSpace(apiKey, nameof(apiKey));
        Precondition.IsNotNullOrWhiteSpace(apiUrl, nameof(apiUrl));
        
        Credentials = credentials;
        ApiKey = apiKey;
        ApiUrl = apiUrl;
        
        this.context = context;
    }

    public static NuGet GetInstance(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));

        var apiKey = context.EnvironmentVariable("NUGET_API_KEY");
        var apiUrl = context.EnvironmentVariable("NUGET_API_URL");

        Precondition.IsNotNullOrWhiteSpace(apiKey, "NUGET_API_KEY", "Could not resolve NUGET_API_KEY.");
        Precondition.IsNotNullOrWhiteSpace(apiUrl, "NUGET_API_URL", "Could not resolve NUGET_API_URL.");

        var credentials = NuGetCredentials.GetInstance(context);
        
        return new NuGet(context, credentials, apiKey, apiUrl);
    }

    public static NuSpecContent[] GetFiles(ICakeContext context, DirectoryPath binDirectory)
    {
        var pathLength = binDirectory.FullPath.Length + 1;

        var files = context.GetFiles(binDirectory + "/**/*")
            .Select(file => file.FullPath.Substring(pathLength).Replace("/", "\\"))
            .Select(file => new NuSpecContent { Source = file, Target = (file.Contains("\\") ? "lib\\" : "")  + file })
            .ToArray();
        
        return files;
    }

    public void Push(FilePath nugetFile)
    {
        Precondition.IsNotNull(nugetFile, nameof(nugetFile));

        Push(nugetFile, new NuGetPushSettings
        {
            ApiKey = ApiKey,
            Source = ApiUrl
        });
    }

    public void Push(FilePathCollection nugetFiles)
    {
        Precondition.IsNotNull(nugetFiles, nameof(nugetFiles));

        Push(nugetFiles, new NuGetPushSettings
        {
            ApiKey = ApiKey,
            Source = ApiUrl
        });
    }

    public void Push(FilePath nugetFile, NuGetPushSettings settings)
    {
        Precondition.IsNotNull(nugetFile, nameof(nugetFile));
        Precondition.IsNotNull(settings, nameof(settings));

        context.NuGetPush(nugetFile, settings);
    }

    public void Push(FilePathCollection nugetFiles, NuGetPushSettings settings)
    {
        Precondition.IsNotNull(nugetFiles, nameof(nugetFiles));
        Precondition.IsNotNull(settings, nameof(settings));

        foreach(var nugetFile in nugetFiles)
        {
            Push(nugetFile, settings);
        }
    }
}