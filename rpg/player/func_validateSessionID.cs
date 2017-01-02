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
    private bool validateSessionID(string socialclub_id,string session_id)
    {
        foreach (Client player in API.getAllPlayers()) {
            if(player.socialClubName == socialclub_id) {
                return(API.getEntityData(player, "session_id") == session_id);
            }
        }
        return false;
    }
}