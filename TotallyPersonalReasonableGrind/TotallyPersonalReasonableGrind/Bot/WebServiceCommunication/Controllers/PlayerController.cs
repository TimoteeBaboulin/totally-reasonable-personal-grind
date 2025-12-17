using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;

public class PlayerController : Controller
{
    private PlayerDAO _playerDAO;

    [HttpPost] [Route("Player/Create/{playerName}")]
    public HttpResponseMessage CreatePlayer(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.CreatePlayer(playerName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut] [Route("Player/Update/Name/{playerName}/{newName}")]
    public HttpResponseMessage UpdatePlayerName(string playerName, string newName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerName(playerName, newName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut] [Route("Player/Update/CombatStats/Exp/{playerName}/{exp}")]
    public HttpResponseMessage UpdatePlayerCombatStatsExp(string playerName, int exp)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerCombatStatsExp(playerName, exp)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut] [Route("Player/Update/CombatStats/Lvl/{playerName}/{level}")]
    public HttpResponseMessage UpdatePlayerCombatStatsLevel(string playerName, int level)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerCombatStatsLevel(playerName, level)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut] [Route("Player/Update/ExplorationStats/Exp/{playerName}/{exp}")]
    public HttpResponseMessage UpdatePlayerExplorationStatsExp(string playerName, int exp)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerExplorationStatsExp(playerName, exp)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut] [Route("Player/Update/ExplorationStats/Lvl/{playerName}/{level}")]
    public HttpResponseMessage UpdatePlayerExplorationStatsLevel(string playerName, int level)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerExplorationStatsLevel(playerName, level)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpPut] [Route("Player/Update/Area/ById/{playerName}/{areaId}")]
    public HttpResponseMessage UpdatePlayerAreaById(string playerName, int areaId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerAreaById(playerName, areaId)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpPut] [Route("Player/Update/Area/ByName/{playerName}/{areaName}")]
    public HttpResponseMessage UpdatePlayerAreaByName(string playerName, string areaName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerAreaByName(playerName, areaName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpPut] [Route("Player/Update/Money/{playerName}/{money}")]
    public HttpResponseMessage UpdatePlayerMoney(string playerName, int money)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerMoney(playerName, money)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpGet] [Route("Player/Get/Player/{playerName}")]
    public Player? GetPlayerFromName(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerFromName(playerName);
    }
    
    [HttpGet] [Route("Player/Get/IdByName/{playerName}")]
    public int GetPlayerIdByName(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerIdFromName(playerName);
    }
    
    [HttpGet] [Route("Player/Get/Name/{playerId}")]
    public string GetPlayerNameFromId(int playerId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerNameFromId(playerId);
    }
    
    [HttpGet] [Route("Player/Get/Combat/Exp/{playerId}")]
    public int GetPlayerCombatExpFromId(int playerId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerCombatExpFromId(playerId);
    }

    [HttpGet] [Route("Player/Get/Combat/Lvl/{playerId}")]
    public int GetPlayerCombatLvlFromId(int playerId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerCombatLvlFromId(playerId);
    }

    [HttpGet] [Route("Player/Get/Exploration/Exp/{playerId}")]
    public int GetPlayerExplorationExpFromId(int playerId)
    {
        _playerDAO = new PlayerDAO();

        return _playerDAO.GetPlayerExplorationExpFromId(playerId);
    }
    
    [HttpGet] [Route("Player/Get/Exploration/Lvl/{playerId}")]
    public int GetPlayerExplorationLvlFromId(int playerId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerExplorationLvlFromId(playerId);
    }
    
    [HttpGet] [Route("Player/Get/Area/{playerId}")]
    public int GetPlayerAreaFromId(int playerId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerAreaFromId(playerId);
    }
    
    [HttpGet] [Route("Player/Get/Money/{playerId}")]
    public int GetPlayerMoneyFromId(int playerId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerMoneyFromId(playerId);
    }

    [HttpGet] [Route("Player/Exists/{playerName}")]
    public bool PlayerExists(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.PlayerExists(playerName);
    }

    [HttpDelete] [Route("Player/Delete/{playerName}")]
    public HttpResponseMessage DeletePlayer(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.DeletePlayer(playerName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
}