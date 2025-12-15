using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class InventoryAccess
{
    public static async Task<List<Inventory>> GetInventoryData(string playerName)
    {
        string dataJson = await HttpClient.Client.SendToWebServiceAsync($"Inventory/Get/All/{playerName}", HttpVerb.GET, null);
        List<Inventory> inventoryList = new();
        foreach (var inventoryJson in System.Text.Json.JsonDocument.Parse(dataJson).RootElement.EnumerateArray())
        {
            inventoryList.Add(Inventory.FromJson(inventoryJson.GetRawText()));
        }

        return inventoryList;
    }
    
    public static async Task AddItemToInventory(string playerName, string itemName, int quantity)
    {
        string exist = await HttpClient.Client.SendToWebServiceAsync($"Inventory/Exists/{playerName}/{itemName}", HttpVerb.GET, null);
        if (!bool.Parse(exist))
        {
            await HttpClient.Client.SendToWebServiceAsync($"Inventory/Create/{playerName}/{itemName}", HttpVerb.POST, null);
        }
        string inventoryQuantity = await HttpClient.Client.SendToWebServiceAsync($"Inventory/Get/Quantity/{playerName}/{itemName}", HttpVerb.GET, null);
        int newQuantity = int.Parse(inventoryQuantity) + quantity;
        await HttpClient.Client.SendToWebServiceAsync($"Inventory/Update/Quantity/{playerName}/{itemName}/{newQuantity}", HttpVerb.PUT, null);
    }
}