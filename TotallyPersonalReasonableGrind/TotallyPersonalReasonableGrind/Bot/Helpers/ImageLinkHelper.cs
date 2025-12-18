using System.Collections.Generic;

namespace TotallyPersonalReasonableGrind.Bot.Helpers;

public class ImageLinkHelper
{
    public static string GetImageLink(string key)
    {
        if (m_imageLinks.TryGetValue(key, out var link))
        {
            return link;
        }
        return m_imageLinks["missing"];
    }
    
    private static Dictionary<string, string> m_imageLinks = new()
    {
        {"initial", "https://i.redd.it/so-a-little-while-back-i-posted-a-theoretical-kingambit-v0-rct4ah3ka58a1.jpg?width=2388&format=pjpg&auto=webp&s=c864176efb2df181f600f5c7ec0d2759d587f8ab"},
        { "walk", "https://i.pinimg.com/originals/f9/61/ff/f961ffb976af2240e164a678b2f4e060.gif" },
        { "sell", "https://o.aolcdn.com/hss/storage/midas/be1c55cad131758aafbaa8ff426e8d42/201194055/jamesgif.gif" },
        { "hit", "https://superherojacked.b-cdn.net/wp-content/uploads/2019/09/Machamp-Workout-4.gif" },
        { "shop", "https://i.pinimg.com/736x/a3/2b/b4/a32bb4a252223bd97385af930175042f.jpg" },
        { "move", "https://daily.pokecommunity.com/wp-content/uploads/2022/08/koraidon_running.gif" },
        { "missing", "https://cdn.discordapp.com/attachments/1375086042213908541/1450122067159814276/2048px-Minecraft_missing_texture_block.png?ex=694162ea&is=6940116a&hm=0842c0221d47e1323bbae4c9c3178f9a813eefa909853f4d4c5e0d4bb377e436&" }
    };
}