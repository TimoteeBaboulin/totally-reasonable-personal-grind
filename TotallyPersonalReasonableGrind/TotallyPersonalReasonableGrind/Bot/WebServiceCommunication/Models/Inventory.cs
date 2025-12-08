namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Inventory
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}