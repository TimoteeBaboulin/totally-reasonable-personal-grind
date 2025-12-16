using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;

public class ItemController : Controller
{
    private ItemDAO itemDAO;

    [HttpPost]
    [Route("Item/Create/{itemName}/{sellValue}")]
    public HttpResponseMessage CreateItem(string itemName, int sellValue)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.CreateItem(itemName, sellValue)
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

    [HttpPut]
    [Route("Item/Update/EmojiName/{itemName}/{emojiName}")]
    public HttpResponseMessage UpdateEmojiName(string itemName, string emojiName)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.UpdateItemEmojiName(itemName, emojiName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpGet]
    [Route("Item/GetById/{itemId}")]
    public Item? GetItemById(int itemId)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.GetItemById(itemId);
    }
    
    [HttpGet]
    [Route("Item/Get/{itemName}")]
    public Item? GetItem(string itemName)
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.GetItem(itemName);
    }
    
    [HttpGet]
    [Route("Item/GetAll")]
    public List<Item> GetAllItems()
    {
        itemDAO = new ItemDAO();
        
        return itemDAO.GetAllItems();
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