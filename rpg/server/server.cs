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
    }

    public void onPlayerConnected(Client player) {
        API.givePlayerWeapon(player,WeaponHash.SawnoffShotgun, 10000, true,true);
    	API.setEntityPosition(player, new Vector3(441.3901062011619,-976.644287109375,30.689605712890625));
    }

    public Dictionary<Client, List<NetHandle>> VehicleHistory = new Dictionary<Client, List<NetHandle>>();

    [Command("v")]
    public void SpawnCarCommand(Client sender, VehicleHash model)
    {
        if (API.isPlayerInAnyVehicle(sender)) {
            var oldVeh = API.getPlayerVehicle(sender);
            API.deleteEntity(oldVeh);
        }

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
        API.sendChatMessageToPlayer(sender, "~o~SERVER: ~c~Fahrzeug wurde erstellt!");
    }

    [Command("vsav")]
    public void SaveCarCommand(Client sender, int tier)
    {
        if (API.isPlayerInAnyVehicle(sender)) {
            var veh = API.getPlayerVehicle(sender);
            var pos = API.getEntityPosition(veh);
            var rot = API.getEntityRotation(veh);
            var model = API.getEntityModel(veh);
            API.sendChatMessageToPlayer(sender, "Position " + pos);
            API.sendChatMessageToPlayer(sender, "Rotation " + rot);
            API.sendChatMessageToPlayer(sender, "Model " + model);
            API.sendChatMessageToPlayer(sender, "Tier " + tier);
            /*<insertCar>*/
                MySqlConnection db_conn = ConnectToDatabase();
                if (db_conn == null) return;

                string query = string.Format(@"INSERT INTO vehicleshop SET x={0}, y={1}, z={2}, rotation={3}, tier={4}", pos.X, pos.Y, pos.Z, rot.Z, tier);
                new MySqlCommand(query, db_conn).ExecuteNonQuery();
            /*</insertCar>*/
            API.sendChatMessageToPlayer(sender, "~o~SERVER: ~c~Fahrzeug position wurde gespeichert!");
        } else {
            API.sendChatMessageToPlayer(sender, "~r~ERR: ~c~Du bist in keinen Fahrzeug!");
        }
    }

    [Command("vdel")]
    public void DeleteCarCommand(Client sender)
    {
        if (API.isPlayerInAnyVehicle(sender)) {
            var oldVeh = API.getPlayerVehicle(sender);
            API.deleteEntity(oldVeh);
            API.sendChatMessageToPlayer(sender, "~o~SERVER: ~c~Fahrzeug wurde gelöscht!");
        } else {
            API.sendChatMessageToPlayer(sender, "~r~ERR: ~c~Du bist in keinen Fahrzeug!");
        }
    }

    [Command("spawn")]
    public void SpawnCarsCommand(Client sender)
    {
        /*<getCars>*/
            MySqlConnection db_conn = ConnectToDatabase();
            if (db_conn == null) return;

            string query = "SELECT * FROM vehicleshop WHERE tier=1";
            var reader = new MySqlCommand(query, db_conn).ExecuteReader();
            while(reader.Read())
            {
                API.sendChatMessageToPlayer(sender, "~o~SERVER: ~c~Rotation: " + reader["rotation"]);
                var veh = API.createVehicle(1039032026, new Vector3((float)reader["x"], (float)reader["y"], (float)reader["z"]), new Vector3(0, 0, (float)reader["rotation"]), 0, 0);
            }
        /*</getCars>*/
        API.sendChatMessageToPlayer(sender, "~o~SERVER: ~c~Fahrzeuge wurden Erstellt!");
    }
}
