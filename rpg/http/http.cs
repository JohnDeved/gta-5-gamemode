using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

public class http : Script
{
    public http()
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

    private void RequestReceived(string args_raw)
    {
    	var args = HttpUtility.ParseQueryString(args_raw);

        API.sendChatMessageToAll("~g~Post:",args_raw);

        try
        {
            var args = JObject.Parse(args_raw);

            if((string)args.SelectToken("session_id") == "") return;
            if((string)args.SelectToken("socialclub_id") == "") return;
            if((string)args.SelectToken("command") == "") return;
            if((string)args.SelectToken("args") == "") return;

            if(!VerifyUser((string)args.SelectToken("socialclub_id"),args.SelectToken("session_id"))) return;

            Client sender = getUser((string)args.SelectToken("socialclub_id"));

            if(sender == null) return;

            switch(args["command"]) {
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
                        index_s = (string)args.SelectToken("args.index_s");
                    }

                    API.setPlayerClothes(sender, ClothingParts[type], index_c, index_s);
                return;
                case "PLAYER_DISCONNECT":
                    API.kickPlayer(sender, (string)args.SelectToken("args"));
                return;
            }

        }catch(Exception e){
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