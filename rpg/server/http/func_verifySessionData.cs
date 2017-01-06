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
    private string http_verifySessionData(string post_raw)
    {
        var post = HttpUtility.ParseQueryString(post_raw);

        if(post["socialclub_id"] == "") return "0";
        if(post["session_id"] == "") return "0";

        if(player_isSessionIDValid(post["socialclub_id"],post["session_id"])) {
            return "1";
        } else {
            return "0";
        }
    }
}