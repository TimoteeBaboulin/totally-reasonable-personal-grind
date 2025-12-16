using System.Collections.Generic;
using System.Threading.Tasks;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class LootAccess
{
    public static async Task<List<Loot>> GetLootFromArea(int areaId)
    {
        string areaData = await HttpClient.Client.SendToWebServiceAsync($"Area/GetById/{areaId}", HttpVerb.GET, null);
        Area? area = Area.FromJson(areaData);
        if (area == null)
        {
            return new List<Loot>();
        }
        string lootData = await HttpClient.Client.SendToWebServiceAsync($"Loot/GetAll/{area.Name}", HttpVerb.GET, null);
        return Loot.FromJsonList(lootData);
    }

    public static async Task<List<Loot>> GetLootEntriesByItemId(int itemId)
    {
        string lootData = await HttpClient.Client.SendToWebServiceAsync($"Loot/GetAllByItemId/{itemId}", HttpVerb.GET, null);
        return Loot.FromJsonList(lootData);
    }
    
    public static async Task<List<Loot>> GetLootEntriesByItemName(string itemName)
    {
        string lootData = await HttpClient.Client.SendToWebServiceAsync($"Loot/GetAllByItemName/{itemName}", HttpVerb.GET, null);
        return Loot.FromJsonList(lootData);
    }
}