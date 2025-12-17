using System;
using MySql.Data.MySqlClient;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;

public class PlayerDAO
{
    private MySqlConnection connection;

    public PlayerDAO()
    {
        const string connectionString =
            "SERVER=10.0.7.74; DATABASE=totallyreasonablepersonalgrind; UID=root; PASSWORD=root;";
        connection = new MySqlConnection(connectionString);
    }

    public bool CreatePlayer(string playerName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO player (name, combat_exp, combat_lvl, exploration_exp, exploration_lvl, area_id, money)" +
                              "VALUES (@name, 0, 0, 0, 0, 1, 0)";
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }

    public bool UpdatePlayerName(string playerName, string newName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET name = @newName WHERE name = @name";
            cmd.Parameters.AddWithValue("@newName", newName);
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool UpdatePlayerCombatStatsExp(string playerName, int exp)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET combat_exp = @exp WHERE name = @name";
            cmd.Parameters.AddWithValue("@exp", exp);
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool UpdatePlayerCombatStatsLevel(string playerName, int level)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET combat_lvl = @level WHERE name = @name";
            cmd.Parameters.AddWithValue("@level", level);
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool UpdatePlayerExplorationStatsExp(string playerName, int exp)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET exploration_exp = @exp WHERE name = @name";
            cmd.Parameters.AddWithValue("@exp", exp);
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool UpdatePlayerExplorationStatsLevel(string playerName, int level)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET exploration_lvl = @level WHERE name = @name";
            cmd.Parameters.AddWithValue("@level", level);
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }

    public bool UpdatePlayerAreaById(string playerName, int areaId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET area_id = @areaId WHERE name = @name";
            cmd.Parameters.AddWithValue("@areaId", areaId);
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool UpdatePlayerAreaByName(string playerName, string areaName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET area_id = (SELECT id FROM area WHERE name = @areaName) WHERE name = @name";
            cmd.Parameters.AddWithValue("@areaName", areaName);
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool UpdatePlayerMoney(string playerName, int money)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET money = @money WHERE name = @name";
            cmd.Parameters.AddWithValue("@money", money);
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public Player? GetPlayerFromName(string playerName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT player.* FROM player WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", playerName);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Player player = Player.FromSqlReader(reader);
                connection.Close();
                return player;
            }
            connection.Close();
            return null;
        }
        catch
        {
            connection.Close();
            return null;
        }
    }
    
    public int GetPlayerIdFromName(string playerName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id FROM player WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", playerName);
            MySqlDataReader reader = cmd.ExecuteReader();
            int playerId = -1;
            if (reader.Read())
            {
                playerId = reader.GetInt32("id");
            }
            connection.Close();
            return playerId;
        }
        catch
        {
            connection.Close();
            return -1;
        }
    }
    
    public string GetPlayerNameFromId(int playerId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT name FROM player WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            MySqlDataReader reader = cmd.ExecuteReader();
            string playerName = string.Empty;
            if (reader.Read())
            {
                playerName = reader.GetString("name");
            }
            connection.Close();
            return playerName;
        }
        catch
        {
            connection.Close();
            return string.Empty;
        }
    }

    public int GetPlayerCombatExpFromId(int playerId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT combat_exp FROM player WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            MySqlDataReader reader = cmd.ExecuteReader();
            int combatExp = -1;
            if (reader.Read())
            {
                combatExp = reader.GetInt32("combat_exp");
            }
            connection.Close();
            return combatExp;
        }
        catch
        {
            connection.Close();
            return -1;
        }
    }

    public int GetPlayerCombatLvlFromId(int playerId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT combat_lvl FROM player WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            MySqlDataReader reader = cmd.ExecuteReader();
            int combatLvl = -1;
            if (reader.Read())
            {
                combatLvl = reader.GetInt32("combat_lvl");
            }
            connection.Close();
            return combatLvl;
        }
        catch
        {
            connection.Close();
            return -1;
        }
    }

    public int GetPlayerExplorationExpFromId(int playerId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT exploration_exp FROM player WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            MySqlDataReader reader = cmd.ExecuteReader();
            int explorationExp = -1;
            if (reader.Read())
            {
                explorationExp = reader.GetInt32("exploration_exp");
            }
            connection.Close();
            return explorationExp;
        }
        catch
        {
            connection.Close();
            return -1;
        }
    }
    
    public int GetPlayerExplorationLvlFromId(int playerId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT exploration_lvl FROM player WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            MySqlDataReader reader = cmd.ExecuteReader();
            int explorationLvl = -1;
            if (reader.Read())
            {
                explorationLvl = reader.GetInt32("exploration_lvl");
            }
            connection.Close();
            return explorationLvl;
        }
        catch
        {
            connection.Close();
            return -1;
        }
    }
    
    public int GetPlayerAreaFromId(int playerId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT area_id FROM player WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            MySqlDataReader reader = cmd.ExecuteReader();
            int areaId = -1;
            if (reader.Read())
            {
                areaId = reader.GetInt32("area_id");
            }
            connection.Close();
            return areaId;
        }
        catch
        {
            connection.Close();
            return -1;
        }
    }
    
    public int GetPlayerMoneyFromId(int playerId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT money FROM player WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", playerId);
            MySqlDataReader reader = cmd.ExecuteReader();
            int money = -1;
            if (reader.Read())
            {
                money = reader.GetInt32("money");
            }
            connection.Close();
            return money;
        }
        catch
        {
            connection.Close();
            return -1;
        }
    }
    
    public bool PlayerExists(string playerName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM player WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", playerName);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            connection.Close();
            return count > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool DeletePlayer(string playerName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM player WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", playerName);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
}