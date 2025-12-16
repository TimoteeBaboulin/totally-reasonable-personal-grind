using System.Collections.Generic;
using System.Threading.Tasks;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Controllers;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class InventoryAccess
{
    public static async Task<List<Inventory>> GetInventoryData(string playerName)
    {
        string dataJson = await HttpClient.Client.SendToWebServiceAsync($"Inventory/Get/All/{playerName}", HttpVerb.GET, null);
        return Inventory.FromJsonList(dataJson);
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
    
    public static async Task<bool> RemoveItemFromInventory(string playerName, string itemName, int quantity)
    {
        string inventoryQuantity = await HttpClient.Client.SendToWebServiceAsync($"Inventory/Get/Quantity/{playerName}/{itemName}", HttpVerb.GET, null);
        int currentQuantity = int.Parse(inventoryQuantity);
        if (currentQuantity < quantity)
        {
            return false; // Not enough items to remove
        }
        int newQuantity = currentQuantity - quantity;
        if (newQuantity == 0)
        {
            await HttpClient.Client.SendToWebServiceAsync($"Inventory/Delete/Single/{playerName}/{itemName}", HttpVerb.DELETE, null);
        }
        else
        {
            await HttpClient.Client.SendToWebServiceAsync($"Inventory/Update/Quantity/{playerName}/{itemName}/{newQuantity}", HttpVerb.PUT, null);
        }
        return true;
    }
    
    public static async Task ClearInventory(string playerName)
    {
        await HttpClient.Client.SendToWebServiceAsync($"Inventory/Delete/Clear/{playerName}", HttpVerb.DELETE, null);
    }
}