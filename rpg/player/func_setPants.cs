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
    private void player_setPants(Client player, string pants, int texture)
    {
    	string type = "m_pants";
    	if(!API.getEntitySyncedData(player, "gender")) {
    		type = "m_pants";
    	}

    	API.setPlayerClothes(player, 4, misc_getClothesIndex(type,pants), texture);
       	API.setEntitySyncedData(player, "pants", pants);
        API.setEntitySyncedData(player, "pants_t", texture);
    }
}