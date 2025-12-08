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
        return new HttpResponseMessage(HttpStatusCode.Accepted);
    }
}