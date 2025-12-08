namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public enum ItemType
{
    Material,
    Loot
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public int SellValue { get; set; }
}