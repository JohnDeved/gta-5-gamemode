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
    public Dictionary<string, int> ClothingParts = new Dictionary<string, int>
    {
        {"face", 0},
        {"masc", 1},
        {"hair", 2},
        {"arms", 3},
        {"legs", 4},
        {"back", 5},
        {"shoes", 6},
        {"ties", 7},
        {"inner", 8},
        {"vest", 9},
        {"decals", 10},
        {"shirt", 11}
    };

    private Client getUser(string socialclub_id)
    {
        foreach (Client player in API.getAllPlayers()) {
            if(player.socialClubName == socialclub_id) {
                return player;
            }
        }
        return null;
    }

    private string VerifyUserWeb(string post_raw) {
        API.sendChatMessageToAll("~g~Post",post_raw);
        var post = HttpUtility.ParseQueryString(post_raw);

        if(post["socialclub_id"] == "") return "0";
        if(post["session_id"] == "") return "0";

        if(player_isSessionIDValid(post["socialclub_id"],post["session_id"])) {
            return "1";
        } else {
            return "0";
        }
    }

    private bool VerifyNameString(string name) {
        Regex namestring = new Regex("-?[A-zÄäÜüÖöß]{3,15}(.[A-zÄäÜüÖöß]+)?");
        Match namematch = namestring.Match(name);
        int matches = 0;
        while (namematch.Success)
        {
            namematch = namematch.NextMatch();
            matches++;
        }

        return(matches == 1);
    }

    private void RequestReceived(string args_raw)
    {
        API.sendChatMessageToAll("~g~Post:",args_raw);

        try
        {
            var args = JObject.Parse(args_raw);

            if((string)args.SelectToken("session_id") == "") return;
            if((string)args.SelectToken("socialclub_id") == "") return;
            if((string)args.SelectToken("command") == "") return;

            if(!player_isSessionIDValid((string)args.SelectToken("socialclub_id"),(string)args.SelectToken("session_id"))) return;

            Client sender = getUser((string)args.SelectToken("socialclub_id"));

            if(sender == null) return;

            switch((string)args.SelectToken("command")) {
                case "CEF_CLOSE":
                    API.triggerClientEvent(sender, "CEF_CLOSE", (string)args.SelectToken("args"));
                return;
                case "ADMIN_EVAL":
                    API.triggerClientEvent(sender, "ADMIN_EVAL", (string)args.SelectToken("args"));
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

    private void HttpListener_Thread()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://185.62.188.120:3001/");
        listener.Start();

        while (true)
        {
            HttpListenerContext ctx = listener.GetContext();

            var request = ctx.Request;
            string post;
            using (var reader = new StreamReader(request.InputStream,request.ContentEncoding))
            {
                post = reader.ReadToEnd();
            }

            string responseText;

            if(ctx.Request.Url.ToString() == "http://185.62.188.120:3001/VerifyUser") {
                responseText = VerifyUserWeb(post);     
            }else{
                responseText = "asdfg";
                RequestReceived(post);  
            }

            byte[] buf = Encoding.UTF8.GetBytes(responseText);
            ctx.Response.ContentEncoding = Encoding.UTF8;
            ctx.Response.ContentType = "text/html";
            ctx.Response.ContentLength64 = buf.Length;      
            
            API.sendChatMessageToAll("~g~URL", ctx.Request.Url.ToString());
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            ctx.Response.Close();
        }
    }

    private void onResourceStart()
    {
        DropTableIfNotExists();
        CreateTableIfNotExists();

        Console.WriteLine("JSON: {0}",misc_getClothesIndex("m_shirt","leatherjacket"));
        Console.WriteLine("Textures: {0}",misc_getClothesTextures("m_shirt","leatherjacket").Length.ToString());

        foreach(string className in misc_getAllClothes("m_shirt")) {
            Console.WriteLine(className);
        }

        new Thread(new ThreadStart(HttpListener_Thread)).Start();
    }   
}