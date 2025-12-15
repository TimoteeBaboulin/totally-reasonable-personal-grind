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
    
    public bool CreateLootEntry(string itemName, string areaName, int quantity, LootRarity rarity, int requiredLevel)
    {
        try
        {
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO loot (item_id, area_id, quantity, rarity, required_level) " +
                              "VALUES ((SELECT id FROM item WHERE name = @itemName), " +
                              "(SELECT id FROM area WHERE name = @areaName), @quantity, @rarity, @requiredLevel)";
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
            cmd.Parameters.AddWithValue("@quantity", quantity);
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
            cmd.CommandText = "UPDATE loot SET required_level = @requiredLevel " +
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
            cmd.CommandText = "SELECT quantity, rarity, required_level FROM loot " +
                              "INNER JOIN item ON loot.item_id = item.id " +
                              "INNER JOIN area ON loot.area_id = area.id " +
                              "WHERE item.name = @itemName AND area.name = @areaName";
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@areaName", areaName);
            MySqlDataReader reader = cmd.ExecuteReader();
            Loot? loot = null;
            if (reader.Read())
            {
                int quantity = reader.GetInt32("quantity");
                string rarityStr = reader.GetString("rarity");
                int requiredLevel = reader.GetInt32("required_level");
                LootRarity rarity = Enum.Parse<LootRarity>(rarityStr);
                loot = new Loot
                {
                    Id = reader.GetInt32("id"),
                    ItemId =  reader.GetInt32("item_id"),
                    AreaId =  reader.GetInt32("area_id"),
                    Quantity = quantity,
                    Rarity = rarity,
                    RequiredLevel = requiredLevel
                };
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
            cmd.CommandText = "SELECT loot.id, loot.item_id, loot.area_id, loot.quantity, loot.rarity, loot.required_level " +
                              "FROM loot INNER JOIN area ON loot.area_id = area.id " +
                              "WHERE area.name = @areaName";
            cmd.Parameters.AddWithValue("@areaName", areaName);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Loot loot = new Loot
                {
                    Id = reader.GetInt32("id"),
                    ItemId = reader.GetInt32("item_id"),
                    AreaId = reader.GetInt32("area_id"),
                    Quantity = reader.GetInt32("quantity"),
                    Rarity = Enum.Parse<LootRarity>(reader.GetString("rarity")),
                    RequiredLevel = reader.GetInt32("required_level")
                };
                loots.Add(loot);
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