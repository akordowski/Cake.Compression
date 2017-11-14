#load "./Precondition.cake"
#load "./TwitterCredentials.cake"
#addin "nuget:?package=Cake.Twitter&version=0.6.0"

public class Twitter
{
    public TwitterCredentials Credentials { get; }
    
    private ICakeContext context;

    public Twitter(ICakeContext context, TwitterCredentials credentials)
    {
        Precondition.IsNotNull(context, nameof(context));
        Precondition.IsNotNull(credentials, nameof(credentials));
        
        Credentials = credentials;
        
        this.context = context;
    }

    public static Twitter GetInstance(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));

        var credentials = TwitterCredentials.GetInstance(context);
        
        return new Twitter(context, credentials);
    }

    public void SendMessage(string message)
    {
        Precondition.IsNotNullOrWhiteSpace(message, nameof(message));
        
        context.TwitterSendTweet(
            Credentials.OAuthConsumerKey,
            Credentials.OAuthConsumerSecret,
            Credentials.AccessToken,
            Credentials.AccessTokenSecret,
            message);
    }
}