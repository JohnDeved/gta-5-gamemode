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
    private void player_setShirt(Client player, string shirt, int texture)
    {
    	string type = "m_shirt";
    	if(!API.getEntitySyncedData(player, "gender")) {
    		type = "m_shirt";
    	}

    	API.setPlayerClothes(player, 11, misc_getClothesIndex(type,shirt), texture);
    	API.setPlayerClothes(player, 8, misc_getClothesProperty(type,shirt,"inner"), 0);
    	API.setPlayerClothes(player, 3, misc_getClothesProperty(type,shirt,"arms"), 0);
        API.setEntitySyncedData(player, "shirt", shirt);
    }
}