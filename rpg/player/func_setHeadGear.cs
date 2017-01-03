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
    private void player_setHeadGear(Client player, string headgear, int texture)
    {
    	string type = "m_headgear";
    	if(!API.getEntitySyncedData(player, "gender")) {
    		type = "f_headgear";
    	}

    	API.setPlayerAccessory(player, 0, misc_getClothesIndex(type,headgear), texture);
    }
}