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
            var dictionary = new Dictionary<string, string>{};
            dictionary.Add("m_glasses", string.Join(",",JsonConvert.SerializeObject(m_glasses)));
            dictionary.Add("m_hair", string.Join(",",JsonConvert.SerializeObject(m_hair)));
            dictionary.Add("m_headgear", string.Join(",",JsonConvert.SerializeObject(m_headgear)));
            dictionary.Add("m_mask", string.Join(",",JsonConvert.SerializeObject(m_mask)));
            dictionary.Add("m_pants", string.Join(",",JsonConvert.SerializeObject(m_pants)));
            dictionary.Add("m_shirt", string.Join(",",JsonConvert.SerializeObject(m_shirt)));
            dictionary.Add("m_shoes", string.Join(",",JsonConvert.SerializeObject(m_shoes)));

            return JsonConvert.SerializeObject(dictionary);
        } else {
            return "0";
        }
    }
}