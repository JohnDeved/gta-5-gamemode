using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;

public class initPlayerServer : Script
{
    public initPlayerServer()
    {
        API.onClientEventTrigger += onClientEventTrigger;
    }

    public void onClientEventTrigger(Client sender, string name, object[] args)
    {
        if (name == "ADMIN_VERIFY")
        {
            if (Array.IndexOf(new string[] {"Admin"}, API.getPlayerAclGroup(sender)) > -1)
            {
                API.sendChatMessageToPlayer(sender, "Sende Admin-Level-Verification");
                API.triggerClientEvent(sender, "ADMIN_VERIFY", API.getPlayerAclGroup(sender));
            }
        }
        if (name == "SESSION_INIT")
        {
            API.setPlayerSkin(sender, (PedHash)1885233650);
            Random rand = new Random();

            API.setPlayerClothes(sender, 0, rand.Next(0, 45), 0);
            API.setPlayerClothes(sender, 1, rand.Next(0, 0), 0);
            API.setPlayerClothes(sender, 2, rand.Next(0, 36), 4);
            API.setPlayerClothes(sender, 3, rand.Next(0, 0), 0);
            API.setPlayerClothes(sender, 4, rand.Next(0, 85), 0);
            API.setPlayerClothes(sender, 5, rand.Next(0, 69), 0);
            API.setPlayerClothes(sender, 6, rand.Next(0, 59), 0);
            API.setPlayerClothes(sender, 7, rand.Next(0, 99), 0);
            API.setPlayerClothes(sender, 8, rand.Next(0, 99), 0);
            API.setPlayerClothes(sender, 9, rand.Next(0, 99), 0);
            API.setPlayerClothes(sender, 11, rand.Next(0, 99), 0);
        }
    }    
}