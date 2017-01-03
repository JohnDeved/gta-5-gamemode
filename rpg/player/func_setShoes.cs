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
    private void player_setShoes(Client player, string shoes, int texture)
    {
    	string type = "m_shoes";
    	if(!API.getEntitySyncedData(player, "gender")) {
    		type = "m_shoes";
    	}

    	API.setPlayerClothes(player, 6, misc_getClothesIndex(type,shoes), texture);
        API.setEntitySyncedData(player, "shoes", shoes);
    }
}