using System;
using System.Collections;
using System.Net;
using System.Threading.Tasks;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class PlayerAccess
{
    public static async Task<Player?> GetOrCreatePlayer(string playerName)
    {
        string existsResponse = await HttpClient.Client.SendToWebServiceAsync($"Player/Exists/{playerName}", HttpVerb.GET, null);
        bool exists = bool.Parse(existsResponse);
        
        if (exists)
        {
            string playerData = await HttpClient.Client.SendToWebServiceAsync($"Player/Get/Player/{playerName}", HttpVerb.GET, null);
            return Player.FromJson(playerData);
        }
        
        string createResponse = await HttpClient.Client.SendToWebServiceAsync($"Player/Create/{playerName}", HttpVerb.POST, null);
        if (createResponse.Contains(((int)HttpStatusCode.OK).ToString()))
        {
            string playerData = await HttpClient.Client.SendToWebServiceAsync($"Player/Get/Player/{playerName}", HttpVerb.GET, null);
            return Player.FromJson(playerData);
        }
        throw new Exception("Failed to create player.");
    }

    public static async Task<bool> UpdatePlayerCombatStats(string playerName, int expGained)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        player.CombatExp += expGained;
        bool leveledUp = false;
        if (player.CombatExp >= (player.CombatLvl + 1) * 100)
        {
            player.CombatExp -= (player.CombatLvl + 1) * 100;
            ++player.CombatLvl;
            leveledUp = true;
        }
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/CombatStats/Exp/{playerName}/{player.CombatExp}", HttpVerb.PUT, null);
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/CombatStats/Lvl/{playerName}/{player.CombatLvl}", HttpVerb.PUT, null);
        return leveledUp;
    }
    
    public static async Task<bool> UpdatePlayerExplorationStats(string playerName, int expGained)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        player.ExplorationExp += expGained;
        bool leveledUp = false;
        if (player.ExplorationExp >= (player.ExplorationLvl + 1) * 100)
        {
            player.ExplorationExp -= (player.ExplorationLvl + 1) * 100;
            ++player.ExplorationLvl;
            leveledUp = true;
        }
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/ExplorationStats/Exp/{playerName}/{player.ExplorationExp}", HttpVerb.PUT, null);
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/ExplorationStats/Lvl/{playerName}/{player.ExplorationLvl}", HttpVerb.PUT, null);
        return leveledUp;
    }
    
    public static async Task UpdatePlayerMoney(string playerName, int amountToAdd)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        player.Money += amountToAdd;
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/Money/{playerName}/{player.Money}", HttpVerb.PUT, null);
    }
    
    public static async Task<int> GetPlayerMeanLevel(string playerName)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        return (int)Math.Floor((player.CombatLvl + player.ExplorationLvl) / 2f);
    }

    public static async Task<int> GetPlayerMoney(string playerName)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        return player.Money;
    }
    
    public static async Task<int> GetPlayerCombatLevel(string playerName)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        return player.CombatLvl;
    }
    
    public static async Task<int> GetPlayerExplorationLevel(string playerName)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        return player.ExplorationLvl;
    }
    
    public static async Task<int> GetNextExpRequiredForNextLevel(string playerName, string statType)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        return statType switch
        {
            "Combat" => (player.CombatLvl + 1) * 100,
            "Exploration" => (player.ExplorationLvl + 1) * 100,
            _ => throw new Exception("Invalid stat type.")
        };
    }

    public static async Task UpdatePlayerAreaById(string playerName, int areaId)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        player.AreaId = areaId;
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/Area/ById/{playerName}/{player.AreaId}", HttpVerb.PUT, null);
    }
    
    public static async Task UpdatePlayerAreaByName(string playerName, string areaName)
    {
        Player? player = await GetOrCreatePlayer(playerName);
        if (player == null)
        {
            throw new Exception("Player not found.");
        }
        // Assuming AreaDAO.GetAreaIdFromName is a method that retrieves area ID from area name
        Area area = await AreaAccess.GetAreaByName(areaName);
        player.AreaId = area.Id;
        await HttpClient.Client.SendToWebServiceAsync($"Player/Update/Area/ByName/{playerName}/{areaName}", HttpVerb.PUT, null);
    }
}