using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Player
{
    public int      Id                { get; set; }
    public string   Name              { get; set; }
    public int      CombatExp         { get; set; }
    public int      CombatLvl         { get; set; }
    public int      ExplorationExp    { get; set; }
    public int      ExplorationLvl    { get; set; }
    public int      AreaId            { get; set; }
    public int      Money             { get; set; }

    public static Player? FromJson(string createResponse)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(createResponse);
    }
    
    public static List<Player> FromJsonList(string createResponse)
    {
        List<Player>? players = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Player>>(createResponse);
        
        return players ?? [];
    }

    public static Player FromSqlReader(MySqlDataReader reader)
    {
        return new Player
        {
            Id              = reader.GetInt32("id"),
            Name            = reader.GetString("name"),
            CombatExp       = reader.GetInt32("combat_exp"),
            CombatLvl       = reader.GetInt32("combat_lvl"),
            ExplorationExp  = reader.GetInt32("exploration_exp"),
            ExplorationLvl  = reader.GetInt32("exploration_lvl"),
            AreaId          = reader.GetInt32("area_id"),
            Money           = reader.GetInt32("money")
        };
    }
}