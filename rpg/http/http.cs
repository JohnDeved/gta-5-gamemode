using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
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

    private void onResourceStart()
    {
    	string a = "API.getEntityData(sender, \"session_id\")";
        
		HttpListener listener = new HttpListener();
		listener.Prefixes.Add("http://185.62.188.120:3001/");
		listener.Start();

		while (true)
		{
			HttpListenerContext ctx = listener.GetContext();
			string responseText = ctx.Request.Url.ToString();
			byte[] buf = Encoding.UTF8.GetBytes(responseText);

			ctx.Response.ContentEncoding = Encoding.UTF8;
			ctx.Response.ContentType = "text/html";
			ctx.Response.ContentLength64 = buf.Length;
			var request = ctx.Request;
			string text;
			using (var reader = new StreamReader(request.InputStream,
			                                     request.ContentEncoding))
			{
			    text = reader.ReadToEnd();
			}

			var args = HttpUtility.ParseQueryString(text);

			API.sendChatMessageToAll("~g~", "SID: " + args["session_id"] + " Social: " + args["socialclub_id"] + " CMD: " + args["command"] + "Args: " + args["args"]);			

			ctx.Response.OutputStream.Write(buf, 0, buf.Length);
			ctx.Response.Close();
		}
    }   
}