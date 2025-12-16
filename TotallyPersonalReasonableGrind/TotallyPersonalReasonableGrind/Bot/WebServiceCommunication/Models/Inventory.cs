using System.Collections.Generic;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Inventory
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    
    public static Inventory? FromJson(string createResponse)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Inventory>(createResponse);
    }

    public static List<Inventory> FromJsonList(string createResponse)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Inventory>>(createResponse);
    }
}