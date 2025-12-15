using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;

public class InventoryDAO
{
    private MySqlConnection connection;

    public InventoryDAO()
    {
        const string connectionString =
            "SERVER=127.0.0.1; DATABASE=totallypersonalreasonablegrind; UID=root; PASSWORD=root;";
        connection = new MySqlConnection(connectionString);
    }

    public bool CreateInventorySlot(string playerName, string itemName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO inventory (player_id, item_id, quantity) " +
                              "VALUES ((SELECT id FROM player WHERE name = @playerName), " +
                              "(SELECT id FROM item WHERE name = @itemName), 0)";
            cmd.Parameters.AddWithValue("@playerName", playerName);
            cmd.Parameters.AddWithValue("@itemName", itemName);
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
    
    public bool UpdateInventoryQuantity(string playerName, string itemName, int quantity)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE inventory SET quantity = @quantity " +
                              "WHERE player_id = (SELECT id FROM player WHERE name = @playerName) " +
                              "AND item_id = (SELECT id FROM item WHERE name = @itemName)";
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@playerName", playerName);
            cmd.Parameters.AddWithValue("@itemName", itemName);
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
    
    public int GetInventoryQuantity(string playerName, string itemName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT quantity FROM inventory INNER JOIN player ON inventory.player_id = player.id " +
                              "INNER JOIN item ON inventory.item_id = item.id " +
                              "WHERE player.name = @playerName AND item.name = @itemName";
            cmd.Parameters.AddWithValue("@playerName", playerName);
            cmd.Parameters.AddWithValue("@itemName", itemName);
            MySqlDataReader reader = cmd.ExecuteReader();
            int quantity = 0;
            if (reader.Read())
            {
                quantity = reader.GetInt32("quantity");
            }
            connection.Close();
            return quantity;
        }
        catch
        {
            connection.Close();
            return -1;
        }
    }
    
    public bool DoesInventorySlotExist(string playerName, string itemName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM inventory INNER JOIN player ON inventory.player_id = player.id " +
                              "INNER JOIN item ON inventory.item_id = item.id " +
                              "WHERE player.name = @playerName AND item.name = @itemName";
            cmd.Parameters.AddWithValue("@playerName", playerName);
            cmd.Parameters.AddWithValue("@itemName", itemName);
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
    
    public List<Inventory> GetAllInventorySlotsForPlayer(string playerName)
    {
        List<Inventory> inventorySlots = new List<Inventory>();
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM inventory INNER JOIN player ON inventory.player_id = player.id " +
                              "WHERE player.name = @playerName";
            cmd.Parameters.AddWithValue("@playerName", playerName);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Inventory inventory = new Inventory
                {
                    Id = reader.GetInt32("inventory.id"),
                    PlayerId = reader.GetInt32("inventory.player_id"),
                    ItemId = reader.GetInt32("inventory.item_id"),
                    Quantity = reader.GetInt32("inventory.quantity")
                };
                inventorySlots.Add(inventory);
            }
            connection.Close();
            return inventorySlots;
        }
        catch
        {
            connection.Close();
            return inventorySlots;
        }
    }
    
    public bool DeleteInventorySlot(string playerName, string itemName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM inventory " +
                              "WHERE player_id = (SELECT id FROM player WHERE name = @playerName) " +
                              "AND item_id = (SELECT id FROM item WHERE name = @itemName)";
            cmd.Parameters.AddWithValue("@playerName", playerName);
            cmd.Parameters.AddWithValue("@itemName", itemName);
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
    
    public bool ClearInventoryForPlayer(string playerName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM inventory " +
                              "WHERE player_id = (SELECT id FROM player WHERE name = @playerName)";
            cmd.Parameters.AddWithValue("@playerName", playerName);
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