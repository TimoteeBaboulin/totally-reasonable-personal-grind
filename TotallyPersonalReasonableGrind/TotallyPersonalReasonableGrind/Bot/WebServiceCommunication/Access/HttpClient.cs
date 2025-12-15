namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class HttpClient
{
    public static RestClient Client { get; } = new();

    public static void Start()
    {
        RestClient.Init();
    }
}