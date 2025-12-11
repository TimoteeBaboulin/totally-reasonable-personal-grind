using System.Net;
using Microsoft.AspNetCore.Mvc;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;

public class ItemController : Controller
{
    private ItemDAO itemDAO;

    [HttpPost]
    [Route("Item/Create/{itemName}/{itemType}/{sellValue}")]
    public HttpResponseMessage CreateItem(string itemName, ItemType itemType, int sellValue)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.CreateItem(itemName, itemType, sellValue)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Item/Update/SellValue/{itemName}/{sellValue}")]
    public HttpResponseMessage UpdateItemSellValue(string itemName, int sellValue)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.UpdateItemSellValue(itemName, sellValue)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpPut]
    [Route("Item/Update/ItemName/{itemName}/{newName}")]
    public HttpResponseMessage UpdateItemName(string itemName, string newName)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.UpdateItemName(itemName, newName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpGet]
    [Route("Item/Get/{itemName}")]
    public Item? GetItem(string itemName)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.GetItem(itemName);
    }

    [HttpGet]
    [Route("Item/Exists/{itemName}")]
    public bool ItemExists(string itemName)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.ItemExists(itemName);
    }

    [HttpDelete]
    [Route("Item/Delete/{itemName}")]
    public HttpResponseMessage DeleteItem(string itemName)
    {
        itemDAO = new ItemDAO();

        return itemDAO.DeleteItem(itemName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
}