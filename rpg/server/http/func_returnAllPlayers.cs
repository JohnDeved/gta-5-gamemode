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
	    private string socialclub_id;
	    private string player_name;
	}

    private string http_returnAllPlayers(string post_raw)
    {
        var post = HttpUtility.ParseQueryString(post_raw);

        if(post["socialclub_id"] == "") return "0";
        if(post["session_id"] == "") return "0";

        if(player_isSessionIDValid(post["socialclub_id"],post["session_id"])) {
		    var dict = new Dictionary<string, Player[]>();
		    dict.Add("player1", new Player[3]);
		    dict["player2"][0] = new Player { name = "aaa", value = "123" };
		    dict["player3"][1] = new Player { name = "bb", value = "34" };
		    dict["player4"][2] = new Player { name = "cc", value = "56" };

		    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(dict));
		    return Newtonsoft.Json.JsonConvert.SerializeObject(dict);
        } else {
            return "0";
        }
    }
}