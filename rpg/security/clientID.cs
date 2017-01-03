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

            Console.WriteLine("Name: {0} ; Gender: {1}",name,gender);
            break;
        }

        API.setPlayerSkin(player, (PedHash)1885233650);

        Random c_rdm = new Random();

        string glasses = Clothing_glasses[c_rdm.Next(0,Clothing_glasses.Length)];
        string hair = Clothing_hair[c_rdm.Next(0,Clothing_hair.Length)];
        string headgear = Clothing_headgear[c_rdm.Next(0,Clothing_headgear.Length)];
        string masc = Clothing_masc[c_rdm.Next(0,Clothing_masc.Length)];
        string pants = Clothing_pants[c_rdm.Next(0,Clothing_pants.Length)];
        string shirt = Clothing_shirt[c_rdm.Next(0,Clothing_shirt.Length)];
        string shoes = Clothing_shoes[c_rdm.Next(0,Clothing_shoes.Length)];

        API.sendChatMessageToPlayer(player,"~y~GLASSES: ~w~ "+glasses+"; ~y~HAIR: ~w~ "+hair+"; ~y~HEADGEAR: ~w~ "+headgear+"; ~y~MASC: ~w~ "+masc+"; ~y~PANTS: ~w~ "+pants+"; ~y~SHIRT: ~w~ "+shirt+"; ~y~SHOES: ~w~ "+shoes);

        int[] glasses_t = misc_getClothesTextures("m_glasses",glasses);
        int[] hair_t = misc_getClothesTextures("m_hair",hair);
        int[] headgear_t = misc_getClothesTextures("m_headgear",headgear);
        int[] masc_t = misc_getClothesTextures("m_masc",masc);
        int[] pants_t = misc_getClothesTextures("m_pants",pants);
        int[] shirt_t = misc_getClothesTextures("m_shirt",shirt);
        int[] shoes_t = misc_getClothesTextures("m_shoes",shoes);

        player_setGlasses(player, glasses, glasses_t[c_rdm.Next(0,glasses_t.Length)]);
        player_setHair(player, hair, hair_t[c_rdm.Next(0,hair_t.Length)]);
        player_setHeadGear(player, headgear, headgear_t[c_rdm.Next(0,headgear_t.Length)]);
        player_setMasc(player, masc, masc_t[c_rdm.Next(0,masc_t.Length)]);
        player_setPants(player, pants, pants_t[c_rdm.Next(0,pants_t.Length)]);
        player_setShirt(player, shirt, shirt_t[c_rdm.Next(0,shirt_t.Length)]);
        player_setShoes(player, shoes, shoes_t[c_rdm.Next(0,shoes_t.Length)]);
    }

    public void onClientEventTrigger(Client sender, string name, object[] args)
    {
        if (name == "SESSION_INIT")
        {
            API.setEntityData(sender, "session_id", getCID(sender.socialClubName));

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