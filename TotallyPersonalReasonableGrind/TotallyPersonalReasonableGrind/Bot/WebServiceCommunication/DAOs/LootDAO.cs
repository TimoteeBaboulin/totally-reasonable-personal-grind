using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.DAOs;

public class LootDAO
{
    private MySqlConnection connection;

    public LootDAO()
    {
        const string connectionString =
            "SERVER=10.0.7.74; DATABASE=totallyreasonablepersonalgrind; UID=root; PASSWORD=root;";
        connection = new MySqlConnection(connectionString);
    }
    
    public bool CreateLootEntry(string itemName, string areaName, int quantity, LootType lootType, LootRarity rarity, int requiredLevel)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO loot (item_id, area_id, quantity, type, rarity, required_lvl) " +
                              "VALUES ((SELECT id FROM item WHERE name = @itemName), " +
                              "(SELECT id FROM area WHERE name = @areaName), @quantity, @type, @rarity, @requiredLevel)";
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@type", lootType.ToString());
            cmd.Parameters.AddWithValue("@rarity", rarity.ToString());
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
    
    public bool UpdateLootQuantity(string itemName, string areaName, int quantity)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE loot SET quantity = @quantity " +
                              "WHERE item_id = (SELECT id FROM item WHERE name = @itemName) " +
                              "AND area_id = (SELECT id FROM area WHERE name = @areaName)";
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
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
    
    public bool UpdateLootType(string itemName, string areaName, LootType lootType)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE loot SET type = @type " +
                              "WHERE item_id = (SELECT id FROM item WHERE name = @itemName) " +
                              "AND area_id = (SELECT id FROM area WHERE name = @areaName)";
            cmd.Parameters.AddWithValue("@type", lootType.ToString());
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
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
    
    public bool UpdateLootRarity(string itemName, string areaName, LootRarity rarity)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE loot SET rarity = @rarity " +
                              "WHERE item_id = (SELECT id FROM item WHERE name = @itemName) " +
                              "AND area_id = (SELECT id FROM area WHERE name = @areaName)";
            cmd.Parameters.AddWithValue("@rarity", rarity.ToString());
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
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
    
    public bool UpdateLootRequiredLevel(string itemName, string areaName, int requiredLevel)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE loot SET required_lvl = @requiredLevel " +
                              "WHERE item_id = (SELECT id FROM item WHERE name = @itemName) " +
                              "AND area_id = (SELECT id FROM area WHERE name = @areaName)";
            cmd.Parameters.AddWithValue("@requiredLevel", requiredLevel);
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
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
    
    public Loot? GetLootEntry(string itemName, string areaName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM loot " +
                              "INNER JOIN item ON loot.item_id = item.id " +
                              "INNER JOIN area ON loot.area_id = area.id " +
                              "WHERE item.name = @itemName AND area.name = @areaName";
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
            MySqlDataReader reader = cmd.ExecuteReader();
            Loot? loot = null;
            if (reader.Read())
            {
                loot = Loot.FromSQLReader(reader);
            }
            connection.Close();
            return loot;
        }
        catch
        {
            connection.Close();
            return null;
        }
    }
    
    public List<Loot> GetLootEntriesOfArea(string areaName)
    {
        List<Loot> loots = new List<Loot>();
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * " +
                              "FROM loot INNER JOIN area ON loot.area_id = area.id " +
                              "WHERE area.name = @areaName";
            cmd.Parameters.AddWithValue("@areaName", areaName);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                loots.Add(Loot.FromSQLReader(reader));
            }
            connection.Close();
        }
        catch
        {
            connection.Close();
        }
        return loots;
    }

    public List<Loot> GetLootEntriesByItemId(int itemId)
    {
        List<Loot> loots = new List<Loot>();
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * " +
                              "FROM loot WHERE item_id = @itemId";
            cmd.Parameters.AddWithValue("@itemId", itemId);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                loots.Add(Loot.FromSQLReader(reader));
            }
            connection.Close();
        }
        catch
        {
            connection.Close();
        }
        return loots;
    }
    
    public List<Loot> GetLootEntriesByItemName(string itemName)
    {
        List<Loot> loots = new List<Loot>();
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * " +
                              "FROM loot INNER JOIN item ON loot.item_id = item.id " +
                              "WHERE item.name = @itemName";
            cmd.Parameters.AddWithValue("@itemName", itemName);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                loots.Add(Loot.FromSQLReader(reader));
            }
            connection.Close();
        }
        catch
        {
            connection.Close();
        }
        return loots;
    }
    
    public bool DeleteLootEntry(string itemName, string areaName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM loot " +
                              "WHERE item_id = (SELECT id FROM item WHERE name = @itemName) " +
                              "AND area_id = (SELECT id FROM area WHERE name = @areaName)";
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
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
    
    public bool DeleteLootEntriesOfArea(string areaName)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM loot " +
                              "WHERE area_id = (SELECT id FROM area WHERE name = @areaName)";
            cmd.Parameters.AddWithValue("@areaName", areaName);
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