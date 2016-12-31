using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;
using MySql.Data.MySqlClient;


public class clientID2 : Script
{
    public clientID2()
    {
        API.onResourceStart += onResourceStart;
    }

    private void onResourceStart()
    {
        ficken();
    }   
}