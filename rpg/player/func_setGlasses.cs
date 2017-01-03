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
    private void player_setGlasses(Client player, string glasses, int texture)
    {
    	string type = "m_glasses";
    	if(!API.getEntitySyncedData(player, "gender")) {
    		type = "f_glasses";
    	}

    	if(glasses == "none") {
    		API.clearPlayerAccessory(player, 1);
    	}

    	API.setPlayerAccessory(player, 1, misc_getClothesIndex(type,glasses), texture);
        API.setEntitySyncedData(player, "glasses", glasses);
    }
}