using System.Net;
using Microsoft.AspNetCore.Mvc;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;

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
}