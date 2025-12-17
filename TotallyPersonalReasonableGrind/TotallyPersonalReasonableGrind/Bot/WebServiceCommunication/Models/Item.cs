using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Item
{
    public int      Id          { get; set; }
    public string   Name        { get; set; }
    public int      SellValue   { get; set; }
    public string   EmojiName   { get; set; }
    
    public static Item? FromJson(string createResponse)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Item>(createResponse);
    }
    
    public static List<Item> ListFromJson(string listResponse)
    {
        List<Item>? items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Item>>(listResponse);
        return items ?? [];
    }

    public static string EmojiFromName(Item item)
    {
        return ":" + item.EmojiName + ":";
    }

    public static Item FromSqlReader(MySqlDataReader reader)
    {
        return new Item
        {
            Id          = reader.GetInt32("id"),
            Name        = reader.GetString("name"),
            SellValue   = reader.GetInt32("sell_value"),
            EmojiName   = reader.GetString("emoji_name")
        };
    }
}