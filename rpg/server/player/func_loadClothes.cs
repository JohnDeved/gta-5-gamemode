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
    private bool player_loadClothes(Client player)
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return false;

        string query = string.Format(@"SELECT * FROM account WHERE socialclub_id='{0}'", player.socialClubName);
        
        var reader = new MySqlCommand(query, db_conn).ExecuteReader();
        while(reader.Read())
        {
            if(reader["c_glasses"] == DBNull.Value||reader["c_glasses"]=="") {
                return false;
            };
            if(reader["c_hair"] == DBNull.Value||reader["c_hair"]=="") {
                return false;
            };
            if(reader["c_headgear"] == DBNull.Value||reader["c_headgear"]=="") {
                return false;
            };
            if(reader["c_mask"] == DBNull.Value||reader["c_mask"]=="") {
                return false;
            };
            if(reader["c_pants"] == DBNull.Value||reader["c_pants"]=="") {
                return false;
            };
            if(reader["c_shirt"] == DBNull.Value||reader["c_shirt"]=="") {
                return false;
            };
            if(reader["c_shoes"] == DBNull.Value||reader["c_shoes"]=="") {
                return false;
            };
            if(reader["c_glasses_tex"] == DBNull.Value||reader["c_glasses_tex"]=="") {
                return false;
            };
            if(reader["c_hair_tex"] == DBNull.Value||reader["c_hair_tex"]=="") {
                return false;
            };
            if(reader["c_headgear_tex"] == DBNull.Value||reader["c_headgear_tex"]=="") {
                return false;
            };
            if(reader["c_mask_tex"] == DBNull.Value||reader["c_mask_tex"]=="") {
                return false;
            };
            if(reader["c_pants_tex"] == DBNull.Value||reader["c_pants_tex"]=="") {
                return false;
            };
            if(reader["c_shirt_tex"] == DBNull.Value||reader["c_shirt_tex"]=="") {
                return false;
            };
            if(reader["c_shoes_tex"] == DBNull.Value||reader["c_shoes_tex"]=="") {
                return false;
            };

            player_setGlasses(player,(string)reader["c_glasses"],(int)reader["c_glasses_tex"]);
            player_setHair(player,(string)reader["c_hair"],(int)reader["c_hair_tex"]);
            player_setHeadGear(player,(string)reader["c_headgear"],(int)reader["c_headgear_tex"]);
            player_setMask(player,(string)reader["c_mask"],(int)reader["c_mask_tex"]);
            player_setPants(player,(string)reader["c_pants"],(int)reader["c_pants_tex"]);
            player_setShirt(player,(string)reader["c_shirt"],(int)reader["c_shirt_tex"]);
            player_setShoes(player,(string)reader["c_shoes"],(int)reader["c_shoes_tex"]);

            break;
        }
        return true;
    }
}