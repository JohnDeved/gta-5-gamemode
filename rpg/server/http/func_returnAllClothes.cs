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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class rpg : Script
{
    private string http_returnAllClothes(string post_raw)
    {
        var args = JObject.Parse(post_raw);

        if((string)args.SelectToken("session_id") == "") return "0";
        if((string)args.SelectToken("socialclub_id") == "") return "0";

        if(player_isSessionIDValid((string)args.SelectToken("socialclub_id"),(string)args.SelectToken("session_id"))) {
            string s_m_glasses = "[" + string.Join(",",JsonConvert.SerializeObject(m_glasses)) + "]";
            File.WriteAllText("TestFile.txt",s_m_glasses);
            return s_m_glasses;
        } else {
            return "0";
        }
    }
}