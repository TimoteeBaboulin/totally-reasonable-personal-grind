using System.Threading.Tasks;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class AreaAccess
{
    public static async Task<Area> GetAreaById(int areaId)
    {
        string areaData = await HttpClient.Client.SendToWebServiceAsync($"Area/GetById/{areaId}", HttpVerb.GET, null);
        return Area.FromJson(areaData);
    }

    public static async Task<List<Area>> GetAllAreas()
    {
        string areasData = await HttpClient.Client.SendToWebServiceAsync($"Area/GetAll", HttpVerb.GET, null);
        return Area.FromJsonList(areasData);
    }
}