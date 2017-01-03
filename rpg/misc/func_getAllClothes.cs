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
    private string[] misc_getAllClothes(string type)
    {
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json",type));
        string[] Clothes = new string[Lines.Length];
        for(int i = 0;i < Lines.Length;i++) {
            Clothes[i] = (string)JObject.Parse(Lines[i]).SelectToken("class");
        }
        return Clothes;
    }
}