using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Web;
//GTANetwork
using GTANetworkServer;
using GTANetworkShared;
//External
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

public partial class rpg : Script
{
    private int[] misc_getClothesTextures(string type, string classname)
    {
        foreach (string Line in File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json",type))) {
            var jObj = JObject.Parse(Line);
            if((string)jObj.SelectToken("class") == classname) {
                string colors_raw = (string)jObj.SelectToken("colors");
                int[] colors = new int[colors_raw.Split(',').Length];
                for(int i = 0;i < colors_raw.Split(',').Length;i++)
                {
                    colors[i] = int.Parse(colors_raw.Split(',')[i]);
                }

                return colors;
            };
        }
        return(new int[] {});
    }
}