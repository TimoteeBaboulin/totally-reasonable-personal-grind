using System;
using MySql.Data.MySqlClient;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;

public class ItemDAO
{
    private MySqlConnection connection;

    public ItemDAO()
    {
        const string connectionString =
            "SERVER=10.0.7.74; DATABASE=totallyreasonablepersonalgrind; UID=root; PASSWORD=root;";
        connection = new MySqlConnection(connectionString);
    }

    public bool CreateItem(string itemName, int sellValue)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO item (name, sell_value, emoji_name)" +
                              "VALUES (@name, @sellValue, @emojiName)";
            cmd.Parameters.AddWithValue("@name", itemName);
            cmd.Parameters.AddWithValue("@sellValue", sellValue);
            cmd.Parameters.AddWithValue("@emojiName", "");
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
    
    public bool UpdateItemEmojiName(string itemName, string emojiName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE item SET emoji_name = @emojiName WHERE name = @name";
            cmd.Parameters.AddWithValue("@emojiName", emojiName);
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
    
    public Item? GetItemById(int itemId)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM item WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", itemId);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Item item = Item.FromSQLReader(reader);
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
                Item item = Item.FromSQLReader(reader);
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
    
    public List<Item> GetAllItems()
    {
        List<Item> items = new List<Item>();
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM item";
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(Item.FromSQLReader(reader));
            }
            connection.Close();
            return items;
        }
        catch
        {
            connection.Close();
            return items;
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