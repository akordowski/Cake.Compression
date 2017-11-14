#load "./Precondition.cake"

public class TwitterCredentials
{
    public string OAuthConsumerKey { get; }
    public string OAuthConsumerSecret { get; }
    public string AccessToken { get; }
    public string AccessTokenSecret { get; }

    public TwitterCredentials(string oAuthConsumerKey, string oAuthConsumerSecret, string accessToken, string accessTokenSecret)
    {
        Precondition.IsNotNullOrWhiteSpace(oAuthConsumerKey, nameof(oAuthConsumerKey));
        Precondition.IsNotNullOrWhiteSpace(oAuthConsumerSecret, nameof(oAuthConsumerSecret));
        Precondition.IsNotNullOrWhiteSpace(accessToken, nameof(accessToken));
        Precondition.IsNotNullOrWhiteSpace(accessTokenSecret, nameof(accessTokenSecret));
        
        OAuthConsumerKey = oAuthConsumerKey;
        OAuthConsumerSecret = oAuthConsumerSecret;
        AccessToken = accessToken;
        AccessTokenSecret = accessTokenSecret;
    }

    public static TwitterCredentials GetInstance(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));
        
        var oAuthConsumerKey = context.EnvironmentVariable("TWITTER_CONSUMER_KEY");
        var oAuthConsumerSecret = context.EnvironmentVariable("TWITTER_CONSUMER_SECRET");
        var accessToken = context.EnvironmentVariable("TWITTER_ACCESS_TOKEN");
        var accessTokenSecret = context.EnvironmentVariable("TWITTER_ACCESS_TOKEN_SECRET");

        Precondition.IsNotNullOrWhiteSpace(oAuthConsumerKey, "TWITTER_CONSUMER_KEY", "Could not resolve TWITTER_CONSUMER_KEY.");
        Precondition.IsNotNullOrWhiteSpace(oAuthConsumerSecret, "TWITTER_CONSUMER_SECRET", "Could not resolve TWITTER_CONSUMER_SECRET.");
        Precondition.IsNotNullOrWhiteSpace(accessToken, "TWITTER_ACCESS_TOKEN", "Could not resolve TWITTER_ACCESS_TOKEN.");
        Precondition.IsNotNullOrWhiteSpace(accessTokenSecret, "TWITTER_ACCESS_TOKEN_SECRET", "Could not resolve TWITTER_ACCESS_TOKEN_SECRET.");

        return new TwitterCredentials(
            oAuthConsumerKey,
            oAuthConsumerSecret,
            accessToken,
            accessTokenSecret);
    }
}