using MySql.Data.MySqlClient;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Area
{
    public int      Id              { get; set; }
    public string   Name            { get; set; }
    public int      RequiredLvl     { get; set; }
    
    public static Area? FromJson(string createResponse)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Area>(createResponse);
    }

    public static List<Area> FromJsonList(string createResponse)
    {
        List<Area>? areas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Area>>(createResponse);
        return areas ?? [];
    }

    public static Area FromSqlReader(MySqlDataReader reader)
    {
        return new Area
        {
            Id              = reader.GetInt32("id"),
            Name            = reader.GetString("name"),
            RequiredLvl     = reader.GetInt32("required_lvl")
        };
    }
}