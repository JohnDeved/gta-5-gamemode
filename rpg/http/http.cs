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
    	}
    }

    private void onResourceStart()
    {
    	string a = "API.getEntityData(sender, \"session_id\")";
        
		HttpListener listener = new HttpListener();
		listener.Prefixes.Add("http://185.62.188.120:3001/");
		listener.Start();

		while (true)
		{
			HttpListenerContext ctx = listener.GetContext();
			string responseText = "asdfg";
			byte[] buf = Encoding.UTF8.GetBytes(responseText);

			ctx.Response.ContentEncoding = Encoding.UTF8;
			ctx.Response.ContentType = "text/html";
			ctx.Response.ContentLength64 = buf.Length;
			
			API.sendChatMessageToAll("~g~URL", ctx.Request.Url.ToString());
			var request = ctx.Request;
			string text;
			using (var reader = new StreamReader(request.InputStream,
			                                     request.ContentEncoding))
			{
			    text = reader.ReadToEnd();
			}

			RequestReceived(text);

			ctx.Response.OutputStream.Write(buf, 0, buf.Length);
			ctx.Response.Close();
		}
    }   
}