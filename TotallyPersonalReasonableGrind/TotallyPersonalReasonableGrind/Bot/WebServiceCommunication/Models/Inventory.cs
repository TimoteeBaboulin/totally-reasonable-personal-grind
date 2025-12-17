using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Inventory
{
    public int Id           { get; set; }
    public int PlayerId     { get; set; }
    public int ItemId       { get; set; }
    public int Quantity     { get; set; }
    
    public static Inventory? FromJson(string createResponse)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Inventory>(createResponse);
    }

    public static List<Inventory> FromJsonList(string createResponse)
    {
        List<Inventory>? inventories = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Inventory>>(createResponse);
        return inventories ?? [];
    }

    public static Inventory FromSqlReader(MySqlDataReader reader)
    {
        return new Inventory
        {
            Id          = reader.GetInt32("id"),
            PlayerId    = reader.GetInt32("player_id"),
            ItemId      = reader.GetInt32("item_id"),
            Quantity    = reader.GetInt32("quantity")
        };
    }
}