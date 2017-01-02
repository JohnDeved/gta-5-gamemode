using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;
using MySql.Data.MySqlClient;

public partial class rpg : Script
{
    private Dictionary<string, string> db_config = new Dictionary<string, string>
    {
        {"db_name", "gta_server"},
        {"db_host", "localhost"},
        {"db_user", "root"},
        {"db_table", "security"},
        {"db_password", "EKbYU6DJNVEwpDTJNFwV3jiG3"},
        {"cid_length", "25"}
    };

    private MySqlConnection ConnectToDatabase()
    {
        MySqlConnection db_conn = null;
        try
        {
            db_conn = new MySqlConnection(string.Format("server={0};database={1};uid={2};password={3}",db_config["db_host"],db_config["db_name"],db_config["db_user"],db_config["db_password"]));
            db_conn.Open();
        }
        catch (ArgumentException a_ex)
        {
            return null;
        }
        catch (MySqlException ex)
        {
            return null;
        }
        return db_conn;
    }    
}