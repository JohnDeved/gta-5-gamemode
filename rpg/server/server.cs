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
        dictionary.Add("m_glasses", m_glasses);
        dictionary.Add("m_hair", m_hair);
        dictionary.Add("m_headgear", m_headgear);
        dictionary.Add("m_mask", m_mask);
        dictionary.Add("m_pants", m_pants);
        dictionary.Add("m_shirt", m_shirt);
        dictionary.Add("m_shoes", m_shoes);
        dictionary.Add("c_owned", new JObject[] {
            JObject.Parse("{m_glasses:'0'}"),
            JObject.Parse("{m_hair:'0'}"),
            JObject.Parse("{m_headgear:'0'}"),
            JObject.Parse("{m_mask:'0'}"),
            JObject.Parse("{m_pants:'0'}"),
            JObject.Parse("{m_shirt:'0'}"),
            JObject.Parse("{m_shoes:'0'}")
        });
        dictionary.Add("c_wearing", new JObject[] {
            JObject.Parse("{m_glasses:'0'}"),
            JObject.Parse("{m_hair:'0'}"),
            JObject.Parse("{m_headgear:'0'}"),
            JObject.Parse("{m_mask:'0'}"),
            JObject.Parse("{m_pants:'0'}"),
            JObject.Parse("{m_shirt:'0'}"),
            JObject.Parse("{m_shoes:'0'}")
        });         

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