using System.Collections.Generic;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public enum ItemType
{
    Material,
    Loot
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public int SellValue { get; set; }
    
    public static Item FromJson(string createResponse)
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            Converters = new List<Newtonsoft.Json.JsonConverter>
            {
                new Newtonsoft.Json.Converters.StringEnumConverter()
            }
        };
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(createResponse, settings);
    }
}