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
    private void player_saveClothes(Client player)
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return;

        string query = string.Format(@"UPDATE account set c_glasses='{0}',c_hair='{1}',c_headgear='{2}',c_mask='{3}',c_pants='{4}',c_shirt='{5}',c_shoes='{6}' WHERE socialclub_id='{7}'",
            API.getEntityData(player, "glasses"),
            API.getEntityData(player, "hair"),
            API.getEntityData(player, "headgear"),
            API.getEntityData(player, "mask"),
            API.getEntityData(player, "pants"),
            API.getEntityData(player, "shirt"),
            API.getEntityData(player, "shoes"),
            player.socialClubName
        );
        new MySqlCommand(query, db_conn).ExecuteNonQuery();
    }
}