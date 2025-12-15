using System;
using System.Collections;
using System.Net;
using System.Threading.Tasks;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class PlayerAccess
{
    public static async Task<Player> GetOrCreatePlayer(string playerName)
    {
        string existsResponse = await HttpClient.Client.SendToWebServiceAsync($"Player/Exists/{playerName}", HttpVerb.GET, null);
        bool exists = bool.Parse(existsResponse);
        
        if (exists)
        {
            string playerData = await HttpClient.Client.SendToWebServiceAsync($"Player/Get/{playerName}", HttpVerb.GET, null);
            return Player.FromJson(playerData);
        }
        
        string createResponse = await HttpClient.Client.SendToWebServiceAsync($"Player/Create/{playerName}", HttpVerb.POST, null);
        if (createResponse.Contains(((int)HttpStatusCode.OK).ToString()))
        {
            string playerData = await HttpClient.Client.SendToWebServiceAsync($"Player/Get/{playerName}", HttpVerb.GET, null);
            return Player.FromJson(playerData);
        }
        throw new Exception("Failed to create player.");
    }
}