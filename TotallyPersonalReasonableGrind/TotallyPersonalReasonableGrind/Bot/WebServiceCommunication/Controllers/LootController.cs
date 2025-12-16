using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;

public class LootController : Controller
{
    private LootDAO lootDAO;

    [HttpPost]
    [Route("Loot/Create/{itemName}/{areaName}/{quantity}/{lootType}/{lootRarity}/{requiredLevel}")]
    public HttpResponseMessage CreateLootEntry(string itemName, string areaName, int quantity, string lootType,
        string lootRarity, int requiredLevel)
    {
        lootDAO = new LootDAO();

        LootType parsedType = Enum.Parse<LootType>(lootType);
        LootRarity parsedRarity = Enum.Parse<LootRarity>(lootRarity);

        return lootDAO.CreateLootEntry(itemName, areaName, quantity, parsedType, parsedRarity, requiredLevel)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Loot/Update/{itemName}/{areaName}/{quantity}")]
    public HttpResponseMessage UpdateLootQuantity(string itemName, string areaName, int quantity)
    {
        lootDAO = new LootDAO();

        return lootDAO.UpdateLootQuantity(itemName, areaName, quantity)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpPut]
    [Route("Loot/Update/Type/{itemName}/{areaName}/{lootType}")]
    public HttpResponseMessage UpdateLootType(string itemName, string areaName, string lootType)
    {
        lootDAO = new LootDAO();
        LootType parsedType = Enum.Parse<LootType>(lootType);

        return lootDAO.UpdateLootType(itemName, areaName, parsedType)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpPut]
    [Route("Loot/Update/Rarity/{itemName}/{areaName}/{lootRarity}")]
    public HttpResponseMessage UpdateLootRarity(string itemName, string areaName, string lootRarity)
    {
        lootDAO = new LootDAO();
        LootRarity parsedRarity = Enum.Parse<LootRarity>(lootRarity);

        return lootDAO.UpdateLootRarity(itemName, areaName, parsedRarity)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
    
    [HttpPut]
    [Route("Loot/Update/RequiredLevel/{itemName}/{areaName}/{requiredLevel}")]
    public HttpResponseMessage UpdateLootRequiredLevel(string itemName, string areaName, int requiredLevel)
    {
        lootDAO = new LootDAO();

        return lootDAO.UpdateLootRequiredLevel(itemName, areaName, requiredLevel)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpGet]
    [Route("Loot/Get/{itemName}/{areaName}")]
    public Loot? GetLootEntry(string itemName, string areaName)
    {
        lootDAO = new LootDAO();

        return lootDAO.GetLootEntry(itemName, areaName);
    }

    [HttpGet]
    [Route("Loot/GetAll/{areaName}")]
    public List<Loot> GetLootEntriesOfArea(string areaName)
    {
        lootDAO = new LootDAO();

        return lootDAO.GetLootEntriesOfArea(areaName);
    }
    
    [HttpGet]
    [Route("Loot/GetAllByItemId/{itemId}")]
    public List<Loot> GetLootEntriesByItemId(int itemId)
    {
        lootDAO = new LootDAO();
        
        return lootDAO.GetLootEntriesByItemId(itemId);
    }
    
    [HttpGet]
    [Route("Loot/GetAllByItemName/{itemName}")]
    public List<Loot> GetLootEntriesByItemName(string itemName)
    {
        lootDAO = new LootDAO();
        
        return lootDAO.GetLootEntriesByItemName(itemName);
    }

    [HttpDelete]
    [Route("Loot/Delete/{itemName}/{areaName}")]
    public HttpResponseMessage DeleteLootEntry(string itemName, string areaName)
    {
        lootDAO = new LootDAO();

        return lootDAO.DeleteLootEntry(itemName, areaName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }

    [HttpDelete]
    [Route("Loot/DeleteAll/{areaName}")]
    public HttpResponseMessage DeleteLootEntriesOfArea(string areaName)
    {
        lootDAO = new LootDAO();

        return lootDAO.DeleteLootEntriesOfArea(areaName)
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
}