using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class InventoryAccess
{
    public static async Task<Inventory> GetInventoryData(string playerName)
    {
        var dataJson = await HttpClient.Client.SendToWebServiceAsync($"Inventory/Get/All/{playerName}", HttpVerb.GET, null);
        Inventory? realData = Inventory.FromJson(dataJson);
        if (realData != null)
        {
           return realData;
        }
        throw new Exception("Failed to get inventory data.");
    }
    
    public static async Task AddItemToInventory(string playerName, string itemName)
    {
        var exist = await HttpClient.Client.SendToWebServiceAsync($"Inventory/Exists/{playerName}/{itemName}", HttpVerb.GET, null);
        if (!bool.Parse(exist))
        {
            await HttpClient.Client.SendToWebServiceAsync($"Inventory/Add/{playerName}/{itemName}", HttpVerb.POST, null);
        }
    }
}