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
    public clientID()
    {
        API.onResourceStart += onResourceStart;
        API.onClientEventTrigger += onClientEventTrigger;
    }

    public static Dictionary<string, string> config = new Dictionary<string, string>
    {
        {"db_name", "gta_server"},
        {"db_host", "localhost"},
        {"db_user", "root"},
        {"db_table", "security"},
        {"db_password", "EKbYU6DJNVEwpDTJNFwV3jiG3"},
        {"cid_length", "25"}
    };
    public static Random _random = new Random();

    private MySqlConnection ConnectToDatabase()
    {
        MySqlConnection db_conn = null;
        try
        {
            db_conn = new MySqlConnection(string.Format("server={0};database={1};uid={2};password={3}",config["db_host"],config["db_name"],config["db_user"],config["db_password"]));
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

    private void onResourceStart()
    {
        DropTableIfNotExists();
        CreateTableIfNotExists();
    }

    private bool Player_isRegistered(string socialclub_id)
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return false;

        string query = string.Format(@"SELECT IFNULL((SELECT 1 FROM account WHERE socialclub_id='{0}'),0)", socialclub_id);
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

            Console.WriteLine("Name: {0} ; Gender: {1}",name,gender);
            break;
        }

        Dictionary<string, int> clothes = new Dictionary<string, int>
        {
            {"face",        23},
            {"beard",       1},
            {"hair",        106},
            {"shirt",       301},
            {"pants",       161},
            {"hands",       2},
            {"shoes",       189},
            {"ties",        264},
            {"misc",        1},
            {"missions",    1},
            {"decals",      1},
            {"inner",       161}
        };

        API.setPlayerSkin(player, (PedHash)1885233650);
        API.setPlayerClothes(player, 0, clothes["face"], 0);
        API.setPlayerClothes(player, 1, clothes["beard"], 0);
        API.setPlayerClothes(player, 2, clothes["hair"], 0);
        API.setPlayerClothes(player, 3, clothes["shirt"], 0);
        API.setPlayerClothes(player, 4, clothes["pants"], 0);
        API.setPlayerClothes(player, 5, clothes["hands"], 0);
        API.setPlayerClothes(player, 6, clothes["shoes"], 0);
        API.setPlayerClothes(player, 7, clothes["ties"], 0);
        API.setPlayerClothes(player, 8, clothes["misc"], 0);
        API.setPlayerClothes(player, 9, clothes["missions"], 0);
        API.setPlayerClothes(player, 10, clothes["decals"], 0);
        API.setPlayerClothes(player, 11, clothes["inner"], 0);
    }

    public void onClientEventTrigger(Client sender, string name, object[] args)
    {
        if (name == "SESSION_INIT")
        {
            API.setEntityData(sender, "session_id", getCID(sender.socialClubName));

            if(Player_isRegistered(sender.socialClubName))
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
    }        
}