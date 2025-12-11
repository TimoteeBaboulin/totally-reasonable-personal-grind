using MySql.Data.MySqlClient;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;

public class ItemDAO
{
    private MySqlConnection connection;

    public ItemDAO()
    {
        const string connectionString =
            "SERVER=127.0.0.1; DATABASE=totallypersonalreasonablegrind; UID=root; PASSWORD=root;";
        connection = new MySqlConnection(connectionString);
    }

    public bool CreateItem(string itemName, ItemType itemType, int sellValue)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO item (name, type, sell_value)" +
                              "VALUES (@name, @type, @sellValue)";
            cmd.Parameters.AddWithValue("@name", itemName);
            cmd.Parameters.AddWithValue("@type", itemType.ToString());
            cmd.Parameters.AddWithValue("@sellValue", sellValue);
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
    
    public bool UpdateItemSellValue(string itemName, int sellValue)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE item SET sell_value = @sellValue WHERE name = @name";
            cmd.Parameters.AddWithValue("@sellValue", sellValue);
            cmd.Parameters.AddWithValue("@name", itemName);
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
    
    public bool UpdateItemName(string currentName, string newName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE item SET name = @newName WHERE name = @currentName";
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
    
    public bool UpdateItemType(string itemName, ItemType itemType)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE item SET type = @type WHERE name = @name";
            cmd.Parameters.AddWithValue("@type", itemType.ToString());
            cmd.Parameters.AddWithValue("@name", itemName);
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
    
    public Item? GetItem(string itemName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM item WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", itemName);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Item item = new Item
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Type = Enum.Parse<ItemType>(reader.GetString("type")),
                    SellValue = reader.GetInt32("sell_value")
                };
                connection.Close();
                return item;
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
    
    public bool ItemExists(string itemName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM item WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", itemName);
            long count = (long)cmd.ExecuteScalar();
            connection.Close();
            return count > 0;
        }
        catch
        {
            connection.Close();
            return false;
        }
    }
    
    public bool DeleteItem(string itemName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM item WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", itemName);
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