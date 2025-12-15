namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

public class Area
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RequiredLVL { get; set; }
    
    public static Area? FromJson(string createResponse)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Area>(createResponse);
    }
}