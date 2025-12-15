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
            "SERVER=127.0.0.1; DATABASE=totallypersonalreasonablegrind; UID=root; PASSWORD=root;";
        connection = new MySqlConnection(connectionString);
    }

    public bool CreatePlayer(string playerName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO player (name, combat_exp, combat_lvl, exploration_exp, exploration_lvl, area_id)" +
                              "VALUES (@name, 0, 0, 0, 0, 1)";
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
    
    public bool UpdatePlayerCombatStatsEXP(string playerName, int exp)
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
    
    public bool UpdatePlayerExplorationStatsEXP(string playerName, int exp)
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

    public bool UpdatePlayerArea(string playerName, string areaName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE player SET area_id = (SELECT id FROM area WHERE name = @areaName)" +
                              "WHERE name = @name";
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
    
    public Player? GetPlayer(string playerName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM player WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", playerName);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Player player = new Player
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    CombatEXP = reader.GetInt32("combat_exp"),
                    CombatLVL = reader.GetInt32("combat_lvl"),
                    ExplorationEXP = reader.GetInt32("exploration_exp"),
                    ExplorationLVL = reader.GetInt32("exploration_lvl"),
                    AreaId = reader.GetInt32("area_id")
                };
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