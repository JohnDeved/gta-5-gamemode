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
    private bool markers_onPlayerConnected(Client player)
    {
        var p_marker = API.createBlip(API.getEntityPosition(player));
        API.attachEntityToEntity(p_marker, player, null, new Vector3(), new Vector3());

        API.setBlipName(p_marker, player.name);
        API.setBlipScale(p_marker, 0.8f);

        API.setEntitySyncedData(player, "p_marker", p_marker);

        API.setWorldSyncedData("p_allPlayers", API.getAllPlayers());
    }
}