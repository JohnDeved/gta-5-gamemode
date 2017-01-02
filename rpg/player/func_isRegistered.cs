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
    private bool player_isRegistered(Client player)
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return false;

        string query = string.Format(@"SELECT IFNULL((SELECT 1 FROM account WHERE socialclub_id='{0}'),0)", player.socialClubName);
        string registered;
        object result = new MySqlCommand(query, db_conn).ExecuteScalar();

        if (result != DBNull.Value)
        {
            registered = result.ToString();
            db_conn.Close();
            return registered == "1";
        }
        else
        {
            return false;
        }        
    }
}