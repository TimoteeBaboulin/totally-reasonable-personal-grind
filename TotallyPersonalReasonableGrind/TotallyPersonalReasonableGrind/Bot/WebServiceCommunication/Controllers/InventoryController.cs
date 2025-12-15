using System.Net;
using Microsoft.AspNetCore.Mvc;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;

public class InventoryController : Controller
{
    private InventoryDAO inventoryDAO;
    
    [HttpPost]
    [Route("Inventory/Create/{playerName}/{itemName}")]
    public HttpResponseMessage CreateInventorySlot(string playerName, string itemName)
    {
        inventoryDAO = new InventoryDAO();
        
        return inventoryDAO.CreateInventorySlot(playerName, itemName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Inventory/Update/Quantity/{playerName}/{itemName}/{quantity}")]
    public HttpResponseMessage UpdateInventoryQuantity(string playerName, string itemName, int quantity)
    {
        inventoryDAO = new InventoryDAO();
        
        return inventoryDAO.UpdateInventoryQuantity(playerName, itemName, quantity)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpGet]
    [Route("Inventory/Get/Quantity/{playerName}/{itemName}")]
    public int GetInventoryQuantity(string playerName, string itemName)
    {
        inventoryDAO = new InventoryDAO();

        return inventoryDAO.GetInventoryQuantity(playerName, itemName);
    }

    [HttpGet]
    [Route("Inventory/Exists/{playerName}/{itemName}")]
    public bool DoesInventorySlotExist(string playerName, string itemName)
    {
        inventoryDAO = new InventoryDAO();
        
        return inventoryDAO.DoesInventorySlotExist(playerName, itemName);
    }
    
    [HttpGet]
    [Route("Inventory/Get/All/{playerName}")]
    public List<Inventory> GetAllInventorySlotsForPlayer(string playerName)
    {
        inventoryDAO = new InventoryDAO();
        
        return inventoryDAO.GetAllInventorySlotsForPlayer(playerName);
    }
    
    [HttpDelete]
    [Route("Inventory/Delete/Single/{playerName}/{itemName}")]
    public HttpResponseMessage DeleteInventorySlot(string playerName, string itemName)
    {
        inventoryDAO = new InventoryDAO();
        
        return inventoryDAO.DeleteInventorySlot(playerName, itemName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpDelete]
    [Route("Inventory/Delete/Clear/{playerName}")]
    public HttpResponseMessage ClearInventoryForPlayer(string playerName)
    {
        inventoryDAO = new InventoryDAO();
        
        return inventoryDAO.ClearInventoryForPlayer(playerName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
}