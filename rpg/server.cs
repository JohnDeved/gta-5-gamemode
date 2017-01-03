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
    	API.onPlayerConnected += onPlayerConnected;
    	API.onPlayerRespawn += onPlayerConnected;
        API.onClientEventTrigger += onClientEventTrigger;
        API.onResourceStart += onResourceStart;
        API.onResourceStart += onServerStart;
    }

	private string[] Clothing_glasses;
	private string[] Clothing_hair;
	private string[] Clothing_headgear;
	private string[] Clothing_masc;
	private string[] Clothing_pants;
	private string[] Clothing_shirt;
	private string[] Clothing_shoes;

    public void onServerStart() {
		Clothing_glasses = misc_getAllClothes("m_glasses");
		Clothing_hair = misc_getAllClothes("m_hair");
		Clothing_headgear = misc_getAllClothes("m_headgear");
		Clothing_masc = misc_getAllClothes("m_masc");
		Clothing_pants = misc_getAllClothes("m_pants");
		Clothing_shirt = misc_getAllClothes("m_shirt");
		Clothing_shoes = misc_getAllClothes("m_shoes");
    }

    public void onPlayerConnected(Client player) {
    	API.setEntityPosition(player, new Vector3(-77.00125885009766,-824.2607421875,326.1755676269531));
    }

    public Dictionary<Client, List<NetHandle>> VehicleHistory = new Dictionary<Client, List<NetHandle>>();

    [Command("create")]
    public void SpawnCarCommand(Client sender, VehicleHash model)
    {
        var rot = API.getEntityRotation(sender.handle);
        var veh = API.createVehicle(model, sender.position, new Vector3(0, 0, rot.Z), 0, 0);

        if (VehicleHistory.ContainsKey(sender))
        {
            VehicleHistory[sender].Add(veh);
            if (VehicleHistory[sender].Count > 3)
            {
                API.deleteEntity(VehicleHistory[sender][0]);
                VehicleHistory[sender].RemoveAt(0);
            }
        }
        else
        {
            VehicleHistory.Add(sender, new List<NetHandle> { veh });
        }
        
        API.setPlayerIntoVehicle(sender, veh, -1);        
    }   
}