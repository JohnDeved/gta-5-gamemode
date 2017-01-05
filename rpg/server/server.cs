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

    public void onServerStart() {
        misc_importClothes();

        Console.WriteLine("JSON: {0}",misc_getClothesIndex("m_shirt","leatherjacket"));
        Console.WriteLine("Textures: {0}",misc_getClothesTextures("m_shirt","leatherjacket").Length.ToString());
        foreach(string className in misc_getAllClothes("m_shirt")) {
            Console.WriteLine(className);
        }        
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