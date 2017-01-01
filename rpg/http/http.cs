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

    	API.sendChatMessageToAll("~g~so_id","na?: "+post["socialclub_id"]);
    	API.sendChatMessageToAll("~g~se_id","na?: "+post["session_id"]);
    	API.sendChatMessageToAll("~g~result",VerifyUser(post["socialclub_id"],post["session_id"]).ToString());

    	if(VerifyUser(post["socialclub_id"],post["session_id"])) {
    		return "1";
    	} else {
    		return "0";
    	}
    }

    private void RequestReceived(string args_raw)
    {
    	var args = HttpUtility.ParseQueryString(args_raw);

    	API.sendChatMessageToAll("~g~Data", "SID: " + args["session_id"] + " Social: " + args["socialclub_id"] + " CMD: " + args["command"] + " Args: " + args["args"]);

    	if(args["session_id"] == "") return;
    	if(args["socialclub_id"] == "") return;
    	if(args["command"] == "") return;
    	if(args["args"] == "") return;

    	if(!VerifyUser(args["socialclub_id"],args["session_id"])) return;

    	Client sender = getUser(args["socialclub_id"]);

    	if(sender == null) return;

    	switch(args["command"]) {
    		case "CEF_CLOSE":
    			API.triggerClientEvent(sender, "CEF_CLOSE", args["args"]);
    		return;
    		case "ADMIN_EVAL":
    			API.triggerClientEvent(sender, "ADMIN_EVAL", args["args"]);
    		return;
    		case "ADMIN_CLOTHES":
    			string type = args["args"].split('*')[0];
    			string index = args["args"].split('*')[1];

    			int index_c = int.Parse(index);

    			API.setPlayerClothes(sender, ClothingParts[type], index_c);
    		return;
    		case "PLAYER_DISCONNECT":
    			API.kickPlayer(sender, args["args"]);
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