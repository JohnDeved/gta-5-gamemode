using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;

public partial class rpg : Script
{
    public rpg()
    {
        API.onClientEventTrigger += onClientEventTrigger;
        API.onResourceStart += onResourceStart;
    }

	private string[] Clothing_glasses = misc_getAllClothes("m_glasses");
	private string[] Clothing_hair = misc_getAllClothes("m_hair");
	private string[] Clothing_headgear = misc_getAllClothes("m_headgear");
	private string[] Clothing_masc = misc_getAllClothes("m_masc");
	private string[] Clothing_pants = misc_getAllClothes("m_pants");
	private string[] Clothing_shirt = misc_getAllClothes("m_shirt");
	private string[] Clothing_shoes = misc_getAllClothes("m_shoes");
}