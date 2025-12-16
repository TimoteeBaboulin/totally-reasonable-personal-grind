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

    public static async Task<bool> UpdatePlayerCombatStats(string playerName, int expGained)
    {
        Player player = await GetOrCreatePlayer(playerName);
        player.CombatEXP += expGained;
        bool leveledUp = false;
        if (player.CombatEXP >= (player.CombatLVL + 1) * 100)
        {
            player.CombatEXP -= (player.CombatLVL + 1) * 100;
            ++player.CombatLVL;
            leveledUp = true;
        }
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/CombatStats/EXP/{playerName}/{player.CombatEXP}", HttpVerb.PUT, null);
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/CombatStats/Level/{playerName}/{player.CombatLVL}", HttpVerb.PUT, null);
        return leveledUp;
    }
    
    public static async Task<bool> UpdatePlayerExplorationStats(string playerName, int expGained)
    {
        Player player = await GetOrCreatePlayer(playerName);
        player.ExplorationEXP += expGained;
        bool leveledUp = false;
        if (player.ExplorationEXP >= (player.ExplorationLVL + 1) * 100)
        {
            player.ExplorationEXP -= (player.ExplorationLVL + 1) * 100;
            ++player.ExplorationLVL;
            leveledUp = true;
        }
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/ExplorationStats/EXP/{playerName}/{player.ExplorationEXP}", HttpVerb.PUT, null);
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/ExplorationStats/Level/{playerName}/{player.ExplorationLVL}", HttpVerb.PUT, null);
        return leveledUp;
    }
    
    public static async Task UpdatePlayerMoney(string playerName, int amountToAdd)
    {
        Player player = await GetOrCreatePlayer(playerName);
        player.Money += amountToAdd;
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/Money/{playerName}/{player.Money}", HttpVerb.PUT, null);
    }
    
    public static async Task<int> GetPlayerMeanLevel(string playerName)
    {
        Player player = await GetOrCreatePlayer(playerName);
        return (int)Math.Floor((player.CombatLVL + player.ExplorationLVL) / 2f);
    }

    public static async Task<int> GetPlayerMoney(string playerName)
    {
        Player player = await GetOrCreatePlayer(playerName);
        return player.Money;
    }
    
    public static async Task<int> GetPlayerCombatLevel(string playerName)
    {
        Player player = await GetOrCreatePlayer(playerName);
        return player.CombatLVL;
    }
    
    public static async Task<int> GetPlayerExplorationLevel(string playerName)
    {
        Player player = await GetOrCreatePlayer(playerName);
        return player.ExplorationLVL;
    }
    
    public static async Task<int> GetNextEXPRequiredForNextLevel(string playerName, string statType)
    {
        Player player = await GetOrCreatePlayer(playerName);
        return statType switch
        {
            "Combat" => (player.CombatLVL + 1) * 100,
            "Exploration" => (player.ExplorationLVL + 1) * 100,
            _ => throw new Exception("Invalid stat type.")
        };
    }
}