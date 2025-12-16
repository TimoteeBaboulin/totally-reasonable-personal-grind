using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;

public class PlayerController : Controller
{
    private PlayerDAO _playerDAO;

    [HttpPost]
    [Route("Player/Create/{playerName}")]
    public HttpResponseMessage CreatePlayer(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.CreatePlayer(playerName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Player/Update/CombatStats/EXP/{playerName}/{exp}")]
    public HttpResponseMessage UpdatePlayerCombatStatsEXP(string playerName, int exp)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerCombatStatsEXP(playerName, exp)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Player/Update/CombatStats/Level/{playerName}/{level}")]
    public HttpResponseMessage UpdatePlayerCombatStatsLevel(string playerName, int level)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerCombatStatsLevel(playerName, level)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Player/Update/ExplorationStats/EXP/{playerName}/{exp}")]
    public HttpResponseMessage UpdatePlayerExplorationStatsEXP(string playerName, int exp)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerExplorationStatsEXP(playerName, exp)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Player/Update/ExplorationStats/Level/{playerName}/{level}")]
    public HttpResponseMessage UpdatePlayerExplorationStatsLevel(string playerName, int level)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerExplorationStatsLevel(playerName, level)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpPut]
    [Route("Player/Update/Area/{playerName}/{areaId}")]
    public HttpResponseMessage UpdatePlayerArea(string playerName, int areaId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerArea(playerName, areaId)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpPut]
    [Route("Player/Update/Money/{playerName}/{money}")]
    public HttpResponseMessage UpdatePlayerMoney(string playerName, int money)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.UpdatePlayerMoney(playerName, money)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpGet]
    [Route("Player/Get/{playerName}")]
    public Player? GetPlayer(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayer(playerName);
    }

    [HttpGet]
    [Route("Player/Exists/{playerName}")]
    public bool PlayerExists(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.PlayerExists(playerName);
    }
    
    [HttpGet]
    [Route("Player/GetIdByName/{playerName}")]
    public int GetPlayerIdByName(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerIdFromName(playerName);
    }
    
    [HttpGet]
    [Route("Player/GetNameById/{playerId}")]
    public string GetPlayerNameById(int playerId)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.GetPlayerNameFromId(playerId);
    }

    [HttpDelete]
    [Route("Player/Delete/{playerName}")]
    public HttpResponseMessage DeletePlayer(string playerName)
    {
        _playerDAO = new PlayerDAO();
        
        return _playerDAO.DeletePlayer(playerName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
}