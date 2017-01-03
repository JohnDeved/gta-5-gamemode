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
            if(reader["c_glasses"] == DBNull.Value) {
                return false;
            };
            if(reader["c_hair"] == DBNull.Value) {
                return false;
            };
            if(reader["c_headgear"] == DBNull.Value) {
                return false;
            };
            if(reader["c_mask"] == DBNull.Value) {
                return false;
            };
            if(reader["c_pants"] == DBNull.Value) {
                return false;
            };
            if(reader["c_shirt"] == DBNull.Value) {
                return false;
            };
            if(reader["c_shoes"] == DBNull.Value) {
                return false;
            };

            player_setGlasses(player,(int)reader["c_glasses"],0);
            player_setHair(player,(int)reader["c_hair"],0);
            player_setHeadGear(player,(int)reader["c_headgear"],0);
            player_setMask(player,(int)reader["c_mask"],0);
            player_setPants(player,(int)reader["c_pants"],0);
            player_setShirt(player,(int)reader["c_shirt"],0);
            player_setShoes(player,(int)reader["c_shoes"],0);

            break;
        }
        return true;
    }
}