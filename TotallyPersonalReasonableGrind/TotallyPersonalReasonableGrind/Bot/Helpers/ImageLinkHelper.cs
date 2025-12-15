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
        { "walk", "https://cdn.discordapp.com/attachments/1375086042213908541/1450119001249419294/n3aaowanynj61.png?ex=6941600f&is=69400e8f&hm=f657c43c20169f4ef4f1d07c9b8a13f3717d3c787eb74cc50bdb6d0d5af9e406&" },
        { "sell", "https://cdn.discordapp.com/attachments/1375086042213908541/1450119452522709062/L1_Joon_1.png?ex=6941607b&is=69400efb&hm=9565b5e4d4ee0d9c48fc19dd54cfd947b0b4d089abd04ad51f346fb41d48318c&" },
        { "hit", "https://cdn.discordapp.com/attachments/1375086042213908541/1443986552287137863/image.png?ex=6940d186&is=693f8006&hm=01c86e985c9ecca5705241ba0bb71dde76ab438cbb1bf69ad1dcb415a56d7317&" },
        { "shop", "https://cdn.discordapp.com/attachments/1375086042213908541/1450118600273825832/240px-Morshu_cutscene.png?ex=69415fb0&is=69400e30&hm=edacf115a9484f734e3600043c6733af13a7c72f13c70782eb1b135925460e89&" },
        { "move", "https://i.imgur.com/EVMx6dP.jpeg" },
        { "missing", "https://cdn.discordapp.com/attachments/1375086042213908541/1450122067159814276/2048px-Minecraft_missing_texture_block.png?ex=694162ea&is=6940116a&hm=0842c0221d47e1323bbae4c9c3178f9a813eefa909853f4d4c5e0d4bb377e436&" }
    };
}