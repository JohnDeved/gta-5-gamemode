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
    private Dictionary<string, string> config = new Dictionary<string, string>
    {
        {"db_name", "gta_server"},
        {"db_host", "localhost"},
        {"db_user", "root"},
        {"db_table", "security"},
        {"db_password", "EKbYU6DJNVEwpDTJNFwV3jiG3"},
        {"cid_length", "25"}
    };
    private Random _random = new Random();

    private bool CreateTableIfNotExists()
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return false;

        string query = string.Format(@"CREATE TABLE IF NOT EXISTS `{0}` (
                                    `socialclub_id` VARCHAR(256) NOT NULL,
                                    `session_id` VARCHAR({1}) NOT NULL,
                                    PRIMARY KEY (`socialclub_id`),
                                    UNIQUE INDEX `socialclub_id_UNIQUE` (`socialclub_id` ASC));", config["db_table"],config["cid_length"]);

        MySqlCommand createTable = new MySqlCommand(query, db_conn);
        createTable.ExecuteNonQuery();
        db_conn.Close();
        return true;
    }

    private bool DropTableIfNotExists()
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return false;

        string query = string.Format(@"DROP TABLE IF EXISTS `{0}`", config["db_table"]);

        MySqlCommand dropTable = new MySqlCommand(query, db_conn);
        dropTable.ExecuteNonQuery();
        db_conn.Close();
        return true;
    }

    private string getCID(string socialclub_id)
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return "-7";

        string query = string.Format(@"SELECT session_id FROM `{0}` WHERE socialclub_id='{1}'", config["db_table"], socialclub_id);

        MySqlCommand q_getCID = new MySqlCommand(query, db_conn);
        StringBuilder session_id = new StringBuilder("");
        session_id.Append((string)q_getCID.ExecuteScalar());

        if (session_id.ToString() != "")
        {
            db_conn.Close();
            return session_id.ToString();
        }
        else
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-".ToCharArray();
            for (int i = 0; i < Convert.ToInt16(config["cid_length"]); i++)
            {
                int charindex = (_random).Next(chars.Length);
                session_id.Append(chars[charindex]);
            }

            query = string.Format(@"INSERT INTO `{0}` SET socialclub_id='{1}', session_id='{2}'", config["db_table"], socialclub_id, session_id.ToString(), 0);
            MySqlCommand q_insertCID = new MySqlCommand(query, db_conn);
            q_insertCID.ExecuteNonQuery();

            db_conn.Close();
            return session_id.ToString();
        }
    }

    private string getSocialClubID(string session_id)
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return "-5";

        string query = string.Format(@"SELECT IFNULL((SELECT socialclub_id FROM `{0}` WHERE session_id='{1}'),NULL)", config["db_table"], session_id);

        MySqlCommand q_getUID = new MySqlCommand(query, db_conn);
        string socialclub_id;
        object result = q_getUID.ExecuteScalar();

        if (result != DBNull.Value)
        {
            socialclub_id = result.ToString();
            db_conn.Close();
            return socialclub_id;
        }
        else
        {
            return "-2";
        }
    }

    private void InitPlayer(Client player)
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return;

        string query = string.Format(@"SELECT * FROM account WHERE socialclub_id='{0}'", player.socialClubName);
        
        var reader = new MySqlCommand(query, db_conn).ExecuteReader();
        while(reader.Read())
        {
            if(reader["socialclub_id"] == DBNull.Value) {
                API.kickPlayer(player, "Fehler beim Initialisieren: 0x01");
                return;
            };

            var socialclub_id = reader["socialclub_id"];
            var name = reader["name"];
            var gender = reader["gender"];

            API.setEntitySyncedData(player, "name", name);
            API.setEntitySyncedData(player, "gender", gender);
            API.setEntitySyncedData(player, "money", (int)reader["money"]);
            API.setEntitySyncedData(player, "bank", (int)reader["bank"]);
            string[] Roles = {"Civilian","Police","Medic"};
            API.setEntitySyncedData(player, "role", Roles[new Random().Next(0,Roles.Length)]);
            API.setEntityData(player, "kxJSzqKiUzqL2TT9gikkeer1L_money", reader["money"]);
            API.setEntityData(player, "kxJSzqKiUzqL2TT9gikkeer1L_bank", reader["bank"]);
            API.setPlayerNametag(player, (string)name);
            API.setPlayerNametagVisible(player, false);
            API.setWorldSyncedData("p_allPlayers", API.getAllPlayers());

            Console.WriteLine("Money: {0} ; Bank: {1}",(int)reader["money"],(int)reader["bank"]);
            break;
        }

        API.setPlayerSkin(player, (PedHash)1885233650);

        if(!player_loadClothes(player))
        {
            player_setRandomClothes(player);
            player_saveClothes(player);
        };
        API.givePlayerWeapon(player,WeaponHash.SawnoffShotgun, 10000, true,true);

    }

    public void onClientEventTrigger(Client sender, string name, object[] args)
    {
        if (name == "SESSION_INIT")
        {
            API.setEntityData(sender, "session_id", getCID(sender.socialClubName));
            API.triggerClientEvent(sender, "SESSION_CRYPT", "var cef=API.createCefBrowser(0,0,true);API.waitUntilCefBrowserInit(cef);API.setCefBrowserPosition(cef,0,00);API.loadPageCefBrowser(cef,'');cef.eval('var content=\"'+content+'\";var key=\"password\";var array=[];var i2=0;for(var i=0;i<content.length;i++){if(i2>=key.length){i2=0}array+=String.fromCharCode((content[i].charCodeAt(0))-15000-(key[i2].charCodeAt(0)));i2++};resourceEval(array)')");
        }
        if (name == "SESSION_READY")
        {
            if(player_isRegistered(sender))
            {
                InitPlayer(sender);
                API.sendChatMessageToPlayer(sender, "~r~REGISTER:~w~ Spieler ist registriert");
                API.triggerClientEvent(sender, "SESSION_SEND", "modal", sender.socialClubName, API.getEntityData(sender, "session_id"));
            }else{
                API.sendChatMessageToPlayer(sender, "~r~REGISTER:~w~ Spieler ist nicht registriert");
                API.triggerClientEvent(sender, "SESSION_SEND", "start", sender.socialClubName, API.getEntityData(sender, "session_id"));
            }            
        }
        if (name == "SESSION_GET")
        {
            API.triggerClientEvent(sender, "SESSION_SEND", args[0], sender.socialClubName, API.getEntityData(sender, "session_id"));
        }
        if (name == "ADMIN_VERIFY")
        {
            if (Array.IndexOf(new string[] {"Admin"}, API.getPlayerAclGroup(sender)) > -1)
            {
                API.sendChatMessageToPlayer(sender, "Sende Admin-Level-Verification");
                API.triggerClientEvent(sender, "ADMIN_VERIFY", API.getPlayerAclGroup(sender));
            }
        }        
    }        
}