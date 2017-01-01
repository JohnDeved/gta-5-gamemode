using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;
using MySql.Data.MySqlClient;


public class clientID : Script
{
    public clientID()
    {
        API.onResourceStart += onResourceStart;
        API.onClientEventTrigger += onClientEventTrigger;
    }

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

    public void onClientEventTrigger(Client sender, string name, object[] args)
    {
        if (name == "SESSION_INIT")
        {
            API.setPlayerSkin(sender, (PedHash)1885233650);
            Random rand = new Random();

            API.setPlayerClothes(sender, 0, rand.Next(0, 45), 0);
            API.setPlayerClothes(sender, 1, rand.Next(0, 0), 0);
            API.setPlayerClothes(sender, 2, rand.Next(0, 36), 4);
            API.setPlayerClothes(sender, 3, rand.Next(0, 0), 0);
            API.setPlayerClothes(sender, 4, rand.Next(0, 85), 0);
            API.setPlayerClothes(sender, 5, rand.Next(0, 69), 0);
            API.setPlayerClothes(sender, 6, rand.Next(0, 59), 0);
            API.setPlayerClothes(sender, 7, rand.Next(0, 99), 0);
            API.setPlayerClothes(sender, 8, rand.Next(0, 99), 0);
            API.setPlayerClothes(sender, 9, rand.Next(0, 99), 0);
            API.setPlayerClothes(sender, 11, rand.Next(0, 99), 0);
            
            API.setEntityData(sender, "session_id", getCID(sender.socialClubName));
        }
        if (name == "SESSION_GET")
        {
            API.triggerClientEvent(sender, "SESSION_SEND", args[0], sender.socialClubName, API.getEntityData(sender, "session_id"));
        }
    }        
}