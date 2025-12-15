using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class ItemAccess
{
    public static async Task<Item> GetItemById(int itemId)
    {
        string response = await HttpClient.Client.SendToWebServiceAsync($"Item/GetById/{itemId}", HttpVerb.GET, null);
        return Item.FromJson(response);
    }
}