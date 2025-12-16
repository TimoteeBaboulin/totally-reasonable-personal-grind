using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public enum LootRarity
{
    Common,
    UnCommon,
    Rare,
    Epic,
    Legendary
}

public enum LootType
{
    Walk,
    Hit
}

public class Loot
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int AreaId { get; set; }
    public int Quantity { get; set; }
    public LootType Type { get; set; }
    public LootRarity Rarity { get; set; }
    public int RequiredLevel { get; set; }
    
    public static Loot? FromJson(string createResponse)
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            Converters = new List<Newtonsoft.Json.JsonConverter>
            {
                new Newtonsoft.Json.Converters.StringEnumConverter()
            }
        };
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Loot>(createResponse, settings);
    }
    
    public static List<Loot> FromJsonList(string createResponse)
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            Converters = new List<Newtonsoft.Json.JsonConverter>
            {
                new Newtonsoft.Json.Converters.StringEnumConverter()
            }
        };
        return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Loot>>(createResponse, settings);
    }

    public static Loot FromSQLReader(ref MySqlDataReader reader)
    {
        Loot loot = new Loot
        {
            Id = reader.GetInt32("id"),
            ItemId = reader.GetInt32("item_id"),
            AreaId = reader.GetInt32("area_id"),
            Quantity = reader.GetInt32("quantity"),
            Type = Enum.Parse<LootType>(reader.GetString("type")),
            Rarity = Enum.Parse<LootRarity>(reader.GetString("rarity")),
            RequiredLevel = reader.GetInt32("required_lvl")
        };
        return loot;
    }
}