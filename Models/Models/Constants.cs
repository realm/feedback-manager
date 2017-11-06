using System;

public static class Constants
{
    public static string RosUrl { get; set; } = "127.0.0.1:9080";

    public static Uri SyncServerUri => new Uri($"realm://{RosUrl}/feedback");

    public static Uri AuthServerUri => new Uri($"http://{RosUrl}");
}
