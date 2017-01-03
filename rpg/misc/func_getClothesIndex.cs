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
    private int misc_getClothesIndex(string type, string classname)
    {
        switch(type) {
            case"m_headgear":
                foreach (string Line in File.ReadAllLines(@"resources\rpg\player\clothes\m_headgear.json")) {
                    var jObj = JObject.Parse(Line);
                    if((string)jObj.SelectToken("class") == classname) {
                        return((int)(decimal)jObj.SelectToken("index"));
                    };
                }
                return -1;
            break;
            default:
                return -1;
            break;
        }
    }
}