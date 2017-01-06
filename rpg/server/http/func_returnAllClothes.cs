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

        if(player_isSessionIDValid((string)args.SelectToken("socialclub_id"),(string)args.SelectToken("session_id") {
            var dictionary = new Dictionary<string, string>{};
            dictionary.Add("m_glasses", m_glasses);
            dictionary.Add("m_hair", m_hair);
            dictionary.Add("m_headgear", m_headgear);
            dictionary.Add("m_mask", m_mask);
            dictionary.Add("m_pants", m_pants);
            dictionary.Add("m_shirt", m_shirt);
            dictionary.Add("m_shoes", m_shoes);
            dictionary.Add("c_owned", new JObject[] {
                JObject.Parse("{m_glasses:'0'}"),
                JObject.Parse("{m_hair:'0'}"),
                JObject.Parse("{m_headgear:'0'}"),
                JObject.Parse("{m_mask:'0'}"),
                JObject.Parse("{m_pants:'0'}"),
                JObject.Parse("{m_shirt:'0'}"),
                JObject.Parse("{m_shoes:'0'}")
            });

            return JsonConvert.SerializeObject(dictionary);
        } else {
            return "0";
        }
    }
}