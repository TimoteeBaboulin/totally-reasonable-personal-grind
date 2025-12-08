namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public enum LootRarity
{
    Common,
    UnCommon,
    Rare,
    Epic,
    Legendary
}

public class Loot
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int AreaId { get; set; }
    public int Quantity { get; set; }
    public LootRarity Rarity { get; set; }
    public int RequiredLevel { get; set; }
}