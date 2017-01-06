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
	struct Player
	{
	    public string socialclub_id;
	    public string player_name;
	}

    private string http_returnAllPlayers(string post_raw)
    {
        var args = JObject.Parse(post_raw);

        if((string)args.SelectToken("session_id") == "") return "0";
        if((string)args.SelectToken("socialclub_id") == "") return "0";

        if(player_isSessionIDValid((string)args.SelectToken("socialclub_id"),(string)args.SelectToken("session_id"))) {
			var dictionary = new Dictionary<string, string>
			dictionary.Add("session_id", "123");
			dictionary.Add("socialclub_id", "abc");

			var serialized = JsonConvert.SerializeObject(dictionary);
			Console.WriteLine(serialized);
			return serialized;
        } else {
            return "0";
        }
    }
}