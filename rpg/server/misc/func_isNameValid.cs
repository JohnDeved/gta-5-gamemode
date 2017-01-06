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
    private bool misc_isNameValid(string name)
    {
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
}