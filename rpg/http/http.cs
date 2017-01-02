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
    public rpg()
    {
        API.onResourceStart += onResourceStart;
    }

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

    private Dictionary<string, string> config = clientID.config;
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

    private bool VerifyUser(string socialclub_id,string session_id)
    {
        foreach (Client player in API.getAllPlayers()) {
            if(player.socialClubName == socialclub_id) {
                return(API.getEntityData(player, "session_id") == session_id);
            }
        }
        return false;
    }

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

        if(VerifyUser(post["socialclub_id"],post["session_id"])) {
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

    private bool NameInUse(string firstname, string lastname) {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return false;

        string query = string.Format(@"SELECT IFNULL((SELECT 1 FROM account WHERE name='{0}'),0)", firstname + " " + lastname);
        string inuse;
        object result = new MySqlCommand(query, db_conn).ExecuteScalar();

        if (result != DBNull.Value)
        {
            inuse = result.ToString();
            db_conn.Close();
            return inuse == "1";
        }
        else
        {
            return false;
        } 
    }

    private void InsertUser(string socialclub_id,string firstname,string lastname,bool gender)
    {
        MySqlConnection db_conn = ConnectToDatabase();
        if (db_conn == null) return;

        int gender_s;
        if(gender) {
            gender_s = 1;
        }else{
            gender_s = 0;
        }

        string query = string.Format(@"INSERT INTO account SET socialclub_id='{0}',name='{1} {2}',gender='{3}',registered=CURRENT_TIMESTAMP,lastconnected=CURRENT_TIMESTAMP", socialclub_id,firstname,lastname,gender_s);
        new MySqlCommand(query, db_conn).ExecuteNonQuery();
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

            if(!VerifyUser((string)args.SelectToken("socialclub_id"),(string)args.SelectToken("session_id"))) return;

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
                    if(Player_isRegistered(sender.socialClubName)) {
                        API.sendChatMessageToAll("~r~Cancel:","6");
                        return;
                    }

                    string firstname = (string)args.SelectToken("args.vorname");                    
                    string lastname = (string)args.SelectToken("args.nachname");                    
                    bool gender = (bool)args.SelectToken("args.gender");

                    if(!VerifyNameString(firstname)||!VerifyNameString(lastname)) {
                        API.sendChatMessageToPlayer(sender, "~r~Der angegebene Name entspricht nicht den Anforderungen.");
                        API.triggerClientEvent(sender, "CEF_CLOSE", "startCEF");
                        API.triggerClientEvent(sender, "SESSION_SEND", "start", sender.socialClubName, API.getEntityData(sender, "session_id"));
                    }else{
                        if(NameInUse(firstname,lastname)) {
                            API.sendChatMessageToPlayer(sender, "~r~Der angegebene Name ist leider schon vergeben.");
                            API.triggerClientEvent(sender, "CEF_CLOSE", "startCEF");
                            API.triggerClientEvent(sender, "SESSION_SEND", "start", sender.socialClubName, API.getEntityData(sender, "session_id"));
                        }else{
                            API.sendChatMessageToPlayer(sender, "~g~Du wirst registriert, bitte warte.");
                            InsertUser(sender.socialClubName,firstname,lastname,gender);
                            API.triggerClientEvent(sender, "CEF_CLOSE", "startCEF");
                            API.triggerClientEvent(sender, "ADMIN_EVAL", "API.triggerServerEvent(\"SESSION_INIT\")");
                        }
                    }
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

    private void onResourceStart()
    {
        string a = "API.getEntityData(sender, \"session_id\")";
        
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://185.62.188.120:3001/");
        listener.Start();

        clientID._random.Next(0,3);

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
}