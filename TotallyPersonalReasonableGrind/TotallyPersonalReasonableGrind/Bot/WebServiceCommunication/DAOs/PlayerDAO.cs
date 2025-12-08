using MySql.Data.MySqlClient;

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
            cmd.CommandText = "INSERT INTO player (name, combat_exp, combat_lvl, exploration_exp, exploration_lvl)" +
                              "VALUES (@name, 0, 0, 0, 0)";
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
}