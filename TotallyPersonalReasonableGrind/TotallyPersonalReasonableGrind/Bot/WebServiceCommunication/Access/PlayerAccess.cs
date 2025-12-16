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

    public static async Task UpdatePlayerCombatStats(string playerName, int expGained)
    {
        Player player = await GetOrCreatePlayer(playerName);
        player.CombatEXP += expGained;
        if (player.CombatEXP >= (player.CombatLVL + 1) * 100)
        {
            player.CombatEXP -= (player.CombatLVL + 1) * 100;
            ++player.CombatLVL;
        }
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/CombatStats/EXP/{playerName}/{player.CombatEXP}", HttpVerb.PUT, null);
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/CombatStats/Level/{playerName}/{player.CombatLVL}", HttpVerb.PUT, null);
    }
    
    public static async Task UpdatePlayerExplorationStats(string playerName, int expGained)
    {
        Player player = await GetOrCreatePlayer(playerName);
        player.ExplorationEXP += expGained;
        if (player.ExplorationEXP >= (player.ExplorationLVL + 1) * 100)
        {
            player.ExplorationEXP -= (player.ExplorationLVL + 1) * 100;
            ++player.ExplorationLVL;
        }
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/ExplorationStats/EXP/{playerName}/{player.ExplorationEXP}", HttpVerb.PUT, null);
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/ExplorationStats/Level/{playerName}/{player.ExplorationLVL}", HttpVerb.PUT, null);
    }
    
    public static async Task<int> GetPlayerMeanLevel(string playerName)
    {
        Player player = await GetOrCreatePlayer(playerName);
        return (int)Math.Floor((player.CombatLVL + player.ExplorationLVL) / 2f);
    }
}