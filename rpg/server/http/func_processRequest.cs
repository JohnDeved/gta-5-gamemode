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
    private void http_processRequest(string args_raw)
    {
        API.sendChatMessageToAll("~g~Post:",args_raw);

        try
        {
            var args = JObject.Parse(args_raw);

            if((string)args.SelectToken("session_id") == "") return;
            if((string)args.SelectToken("socialclub_id") == "") return;
            if((string)args.SelectToken("command") == "") return;

            if(!player_isSessionIDValid((string)args.SelectToken("socialclub_id"),(string)args.SelectToken("session_id"))) return;

            Client sender = misc_getPlayerFromSocialClubID((string)args.SelectToken("socialclub_id"));

            if(sender == null) return;

            switch((string)args.SelectToken("command")) {
                case "CEF_CLOSE":
                    API.triggerClientEvent(sender, "CEF_CLOSE", (string)args.SelectToken("args"));
                return;
                case "ADMIN_EVAL":
                    string code = (string)args.SelectToken("args.code");                    
                    string[] targets = ((string)args.SelectToken("args.targets")).Split(',');                    
                    
                    if(targets.Contains("global")) {
                        API.triggerClientEventForAll("ADMIN_EVAL", code);
                    } else {
                        foreach(string target in targets)
                        {
                            if(target == "local")
                            {
                                API.triggerClientEvent(sender, "ADMIN_EVAL", code);
                            }

                            Client p_target = misc_getPlayerFromSocialClubID(target);
                            if(p_target != null)
                            {
                                if(p_target != sender || !targets.Contains("local")) {
                                    API.triggerClientEvent(p_target, "ADMIN_EVAL", code);
                                }
                            }
                        }
                    }
                return;
                case "ADMIN_CLOTHES":
                    string type = (string)args.SelectToken("args.type");
                    string index = (string)args.SelectToken("args.index");

                    int index_c = int.Parse(index);
                    int index_s = 0;

                    if((string)args.SelectToken("args.index_s") != "") {
                        index_s = int.Parse((string)args.SelectToken("args.index_s"));
                    }

                    API.setPlayerClothes(sender, ClothingParts[type], index_c, index_s);
                return;
                case "REGISTER":
                    if(player_isRegistered(sender)) {
                        API.sendChatMessageToAll("~r~Cancel:","6");
                        return;
                    }

                    string firstname = (string)args.SelectToken("args.vorname");                    
                    string lastname = (string)args.SelectToken("args.nachname");                    
                    bool gender = (bool)args.SelectToken("args.gender");

                    player_register(sender,firstname,lastname,gender);
                return;
                case "PLAYER_DISCONNECT":
                    API.kickPlayer(sender, (string)args.SelectToken("args"));
                return;
            }

        }catch(Exception e){
            API.sendChatMessageToAll("~r~ERROR:",e.ToString());
            return;
        }
    }
}