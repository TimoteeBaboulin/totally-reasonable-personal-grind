using System.Threading.Tasks;
using System.Collections.Generic;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class ItemAccess
{
    public static async Task<Item> GetItemById(int itemId)
    {
        string response = await HttpClient.Client.SendToWebServiceAsync($"Item/GetById/{itemId}", HttpVerb.GET, null);
        Item? item = Item.FromJson(response);
        if (item == null)
        {
            throw new System.Exception($"Item with ID {itemId} not found.");
        }
        return item;
    }

    public static async Task<List<Item>> GetAllItems()
    {
        string response = await HttpClient.Client.SendToWebServiceAsync("Item/GetAll", HttpVerb.GET, null);
        return Item.ListFromJson(response);
    }
}