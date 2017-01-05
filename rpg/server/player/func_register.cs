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
    private void player_register(Client player, string firstname, string lastname, bool gender)
    {
       if(!VerifyNameString(firstname)||!VerifyNameString(lastname)) {
            API.sendChatMessageToPlayer(player, "~r~Der angegebene Name entspricht nicht den Anforderungen.");
            API.triggerClientEvent(player, "CEF_CLOSE", "startCEF");
            API.triggerClientEvent(player, "SESSION_SEND", "start", player.socialClubName, API.getEntityData(player, "session_id"));
        }else{
            if(player_isNameInUse(firstname,lastname)) {
                API.sendChatMessageToPlayer(player, "~r~Der angegebene Name ist leider schon vergeben.");
                API.triggerClientEvent(player, "CEF_CLOSE", "startCEF");
                API.triggerClientEvent(player, "SESSION_SEND", "start", player.socialClubName, API.getEntityData(player, "session_id"));
            }else{
                API.sendChatMessageToPlayer(player, "~g~Du wirst registriert, bitte warte.");

                /*<insertUser>*/
                    MySqlConnection db_conn = ConnectToDatabase();
                    if (db_conn == null) return;

                    int gender_s;
                    if(gender) {
                        gender_s = 1;
                    }else{
                        gender_s = 0;
                    }

                    string query = string.Format(@"INSERT INTO account SET socialclub_id='{0}',name='{1} {2}',gender='{3}',registered=CURRENT_TIMESTAMP,lastconnected=CURRENT_TIMESTAMP", player.socialClubName,firstname,lastname,gender_s);
                    new MySqlCommand(query, db_conn).ExecuteNonQuery();
                /*</insertUser>*/    

                API.triggerClientEvent(player, "CEF_CLOSE", "startCEF");
                API.triggerClientEvent(player, "ADMIN_EVAL", "API.triggerServerEvent(\"SESSION_INIT\")");
            }
        }
    }
}