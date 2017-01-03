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
    private string[] misc_getAllClothes(string type)
    {
        JObject[] Parts;

        switch(type)
        {
        	case"m_glasses":
        		Parts = m_glasses;
			break;
        	case"m_hair":
				Parts = m_hair;
			break;
        	case"m_headgear":
				Parts = m_headgear;
			break;
        	case"m_masc":
				Parts = m_masc;
			break;
        	case"m_pants":
				Parts = m_pants;
			break;
        	case"m_shirt":
				Parts = m_shirt;
			break;
        	case"m_shoes":
				Parts = m_shoes;
			break;
        }


        string[] Clothes = new string[Parts.Length];
        for(int i = 0;i < Parts.Length;i++) {
            Clothes[i] = Parts[i].SelectToken("class");
        }
        return Clothes;
    }
}