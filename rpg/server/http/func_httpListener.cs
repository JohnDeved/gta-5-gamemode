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
                    responseText = "<HTML>\n<HEAD>\n<TITLE>404 Not Found</TITLE>\n</HEAD>\n<BODY>\n<H1>Not Found</H1>\nThe requested document was not found on this server.\n<P>\n<HR>\n<ADDRESS>\nWeb Server at 185.62.188.120\n</ADDRESS>\n</BODY>\n</HTML>\n\n<!--\n   - Unfortunately, Microsoft has added a clever new\n   - \"feature\" to Internet Explorer. If the text of\n   - an error's message is \"too small\", specifically\n   - less than 512 bytes, Internet Explorer returns\n   - its own error message. You can turn that off,\n   - but it's pretty tricky to find switch called\n   - \"smart error messages\". That means, of course,\n   - that short error messages are censored by default.\n   - IIS always returns error messages that are long\n   - enough to make Internet Explorer happy. The\n   - workaround is pretty simple: pad the error\n   - message with a big comment like this to push it\n   - over the five hundred and twelve bytes minimum.\n   - Of course, that's exactly what you're reading\n   - right now.\n   -->\n";
                    http_processRequest(post);
                    break;
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