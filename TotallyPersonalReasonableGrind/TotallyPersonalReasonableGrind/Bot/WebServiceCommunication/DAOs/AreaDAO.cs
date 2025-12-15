using MySql.Data.MySqlClient;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;

public class AreaDAO
{
    private MySqlConnection connection;

    public AreaDAO()
    {
        const string connectionString =
            "SERVER=10.0.7.74; DATABASE=totallypersonalreasonablegrind; UID=root; PASSWORD=root;";
        connection = new MySqlConnection(connectionString);
    }

    public bool CreateArea(string areaName, int requiredLevel)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO area (name, required_level)" +
                              "VALUES (@name, @requiredLevel)";
            cmd.Parameters.AddWithValue("@name", areaName);
            cmd.Parameters.AddWithValue("@requiredLevel", requiredLevel);
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
    
    public bool UpdateAreaRequiredLevel(string areaName, int requiredLevel)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE area SET required_level = @requiredLevel WHERE name = @name";
            cmd.Parameters.AddWithValue("@requiredLevel", requiredLevel);
            cmd.Parameters.AddWithValue("@name", areaName);
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
    
    public bool UpdateAreaName(string currentName, string newName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE area SET name = @newName WHERE name = @currentName";
            cmd.Parameters.AddWithValue("@newName", newName);
            cmd.Parameters.AddWithValue("@currentName", currentName);
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
    
    public Area? GetAreaByName(string areaName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, name, required_level FROM area WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", areaName);
            MySqlDataReader reader = cmd.ExecuteReader();
            Area? area = null;
            if (reader.Read())
            {
                area = new Area
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    RequiredLVL = reader.GetInt32("required_level")
                };
            }
            connection.Close();
            return area;
        }
        catch
        {
            connection.Close();
            return null;
        }
    }
    
    public bool DoesAreaExist(string areaName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM area WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", areaName);
            long count = (long)cmd.ExecuteScalar()!;
            connection.Close();
            return count > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool DeleteArea(string areaName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM area WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", areaName);
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