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