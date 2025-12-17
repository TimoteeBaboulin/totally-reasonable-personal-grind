using System.Threading.Tasks;
using TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Models;

namespace TotallyPersonalReasonableGrind.Bot.WebServiceCommunication.Access;

public class AreaAccess
{
    public static async Task<Area> GetAreaById(int areaId)
    {
        string areaData = await HttpClient.Client.SendToWebServiceAsync($"Area/GetById/{areaId}", HttpVerb.GET, null);
        Area? area = Area.FromJson(areaData);
        if (area == null)
        {
            throw new Exception($"Area with ID {areaId} not found.");
        }
        return area;
    }

    public static async Task<List<Area>> GetAllAreas()
    {
        string areasData = await HttpClient.Client.SendToWebServiceAsync($"Area/GetAll", HttpVerb.GET, null);
        return Area.FromJsonList(areasData);
    }
}