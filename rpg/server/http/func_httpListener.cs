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

            switch(ctx.Request.Url.ToString())
            {
                case "http://185.62.188.120:3001/VerifyUser":
                    responseText = http_verifySessionData(post);
                    break;
                case "http://185.62.188.120:3001/getAllPlayers":
                    responseText = http_returnAllPlayers(post);
                    break;
                case "http://185.62.188.120:3001/getAllClothes":
                    responseText = http_returnAllClothes(post);
                    break;
                default:
                    responseText = "asdfg";
                    http_processRequest(post);
                    break;
            }

            Console.Log("URL: " + ctx.Request.Url.ToString());
            
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