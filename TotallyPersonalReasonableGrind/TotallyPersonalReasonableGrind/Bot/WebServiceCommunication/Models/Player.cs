namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CombatEXP { get; set; }
    public int CombatLVL { get; set; }
    public int ExplorationEXP { get; set; }
    public int ExplorationLVL { get; set; }
    public int AreaId { get; set; }

    public static Player FromJson(string createResponse)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(createResponse);
    }
}