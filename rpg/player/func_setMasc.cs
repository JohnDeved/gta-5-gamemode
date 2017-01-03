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
    private void player_setMasc(Client player, string masc, int texture)
    {
    	string type = "m_masc";
    	if(!API.getEntitySyncedData(player, "gender")) {
    		type = "f_masc";
    	}

    	API.setPlayerClothes(player, 1, misc_getClothesIndex(type,masc), texture);
    }
}