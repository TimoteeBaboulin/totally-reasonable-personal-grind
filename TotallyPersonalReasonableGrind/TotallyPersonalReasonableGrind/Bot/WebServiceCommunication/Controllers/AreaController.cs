using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;

public class AreaController : Controller
{
    private AreaDAO areaDAO;

    [HttpPost]
    [Route("Area/Create/{areaName}/{requiredLevel}")]
    public HttpResponseMessage CreateArea(string areaName, int requiredLevel)
    {
        areaDAO = new AreaDAO();
        
        return areaDAO.CreateArea(areaName, requiredLevel)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Area/Update/RequiredLevel/{areaName}/{requiredLevel}")]
    public HttpResponseMessage UpdateAreaRequiredLevel(string areaName, int requiredLevel)
    {
        areaDAO = new AreaDAO();

        return areaDAO.UpdateAreaRequiredLevel(areaName, requiredLevel)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Area/Update/AreaName/{areaName}/{newName}")]
    public HttpResponseMessage UpdateAreaName(string areaName, string newName)
    {
        areaDAO = new AreaDAO();

        return areaDAO.UpdateAreaName(areaName, newName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpGet]
    [Route("Area/GetById/{areaId}")]
    public Area? GetAreaById(int areaId)
    {
        areaDAO = new AreaDAO();
        
        return areaDAO.GetAreaById(areaId);
    }
    
    [HttpGet]
    [Route("Area/GetbyName/{areaName}")]
    public Area? GetAreaByName(string areaName)
    {
        areaDAO = new AreaDAO();
        
        return areaDAO.GetAreaByName(areaName);
    }
    
    [HttpGet]
    [Route("Area/GetAll")]
    public List<Area> GetAllAreas()
    {
        areaDAO = new AreaDAO();
        
        return areaDAO.GetAllAreas();
    }

    [HttpGet]
    [Route("Area/Exists/{areaName}")]
    public bool DoesAreaExist(string areaName)
    {
        areaDAO = new AreaDAO();
        
        return areaDAO.DoesAreaExist(areaName);
    }

    [HttpDelete]
    [Route("Area/Delete/{areaName}")]
    public HttpResponseMessage DeleteArea(string areaName)
    {
        areaDAO = new AreaDAO();

        return areaDAO.DeleteArea(areaName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
}