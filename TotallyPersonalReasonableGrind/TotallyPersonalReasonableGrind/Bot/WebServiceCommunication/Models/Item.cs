using System.Collections.Generic;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SellValue { get; set; }
    public string EmojiName { get; set; }
    
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

    public static string EmojiFromName(Item item)
    {
        return ":" + item.EmojiName + ":";
    }
}