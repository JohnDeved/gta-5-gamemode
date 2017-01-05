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
    private void player_setHair(Client player, string hair, int texture)
    {
    	string type = "m_hair";
    	if(!API.getEntitySyncedData(player, "gender")) {
    		type = "f_hair";
    	}

    	API.setPlayerClothes(player, 2, misc_getClothesIndex(type,hair), texture);
        API.setEntitySyncedData(player, "hair", hair);
        API.setEntitySyncedData(player, "hair_t", texture);
    }
}