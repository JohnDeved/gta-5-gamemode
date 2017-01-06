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
    private bool markers_onPlayerDisconnected(Client player)
    {
        var p_marker = API.getEntitySyncedData(player, "p_marker");

        if (p_marker != null)
        {
            API.deleteEntity(p_marker);

			API.setWorldSyncedData("p_allPlayers", API.getAllPlayers());
	    }
    }
}