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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class rpg : Script
{
    public rpg()
    {
    	API.onPlayerConnected += onPlayerConnected;
    	API.onPlayerRespawn += onPlayerConnected;
        API.onClientEventTrigger += onClientEventTrigger;
        API.onResourceStart += onServerStart;
        API.onPlayerConnected += markers_onPlayerConnected;
        API.onPlayerDisconnected += markers_onPlayerDisconnected;
    }

    public void onServerStart() {
        misc_importClothes();
        DropTableIfNotExists();
        CreateTableIfNotExists();

        new Thread(new ThreadStart(HttpListener_Thread)).Start();

        var dictionary = new Dictionary<string, JObject[]>{};
        dictionary.Add("m_glasses", string.Join(",",JsonConvert.SerializeObject(m_glasses)));
        dictionary.Add("m_hair", string.Join(",",JsonConvert.SerializeObject(m_hair)));
        dictionary.Add("m_headgear", string.Join(",",JsonConvert.SerializeObject(m_headgear)));
        dictionary.Add("m_mask", string.Join(",",JsonConvert.SerializeObject(m_mask)));
        dictionary.Add("m_pants", string.Join(",",JsonConvert.SerializeObject(m_pants)));
        dictionary.Add("m_shirt", string.Join(",",JsonConvert.SerializeObject(m_shirt)));
        dictionary.Add("m_shoes", string.Join(",",JsonConvert.SerializeObject(m_shoes)));
        dictionary.Add("c_owned", "0");

        File.WriteAllText("TestFile.txt",JsonConvert.SerializeObject(dictionary));
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