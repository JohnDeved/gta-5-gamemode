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
    private string misc_getClothesProperty(string type, string classname, string property)
    {
        foreach (string Line in File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json",type))) {
            var jObj = JObject.Parse(Line);
            if((string)jObj.SelectToken("class") == classname) {
                return((string)jObj.SelectToken(property));
            };
        }
        return -1;
    }
}