namespace Server
{
    public static class Constants
    {
        public const string SentimentApiKey = "YOUR-TEXT-ANALYTICS-KEY";
        public const string FeatureToken = "YOUR-ROS-FEATURE-TOKEN";

        public const string RosUrl = "localhost:9080";
        public const string RosUsername = "realm-admin";
        public const string RosPassword = "";

        public const string SentimentUrl = "https://northeurope.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
        public const string KeyPhraseUrl = "https://northeurope.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";
        public const string NotifierDirectory = "NotifierRealms";
        public const string FeedbackRealm = "feedback";
    }
}
