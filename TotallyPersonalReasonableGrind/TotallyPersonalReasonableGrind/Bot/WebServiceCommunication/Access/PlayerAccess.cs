using System.Collections;
using System.Net;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class PlayerAccess
{
    public static Player GetOrCreatePlayer(string playerName)
    {
        string getResponse = HttpClient.Client.SendToWebService($"Player/Exists/{playerName}", HttpVerb.GET, null);
        bool exists = bool.Parse(getResponse);
        
        if (exists)
        {
            string playerData = HttpClient.Client.SendToWebService($"Player/Get/{playerName}", HttpVerb.GET, null);
            return Player.FromJson(playerData);
        }
        
        string createResponse = HttpClient.Client.SendToWebService($"Player/Create/{playerName}", HttpVerb.POST, null);
        if (createResponse.Contains(((int)HttpStatusCode.OK).ToString()))
        {
            string playerData = HttpClient.Client.SendToWebService($"Player/Get/{playerName}", HttpVerb.GET, null);
            return Player.FromJson(playerData);
        }
        throw new Exception("Failed to create player.");
    }
}