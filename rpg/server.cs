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
    public partial rpg()
    {
        API.onClientEventTrigger += onClientEventTrigger;
        API.onResourceStart += onResourceStart;
        API.onClientEventTrigger += onClientEventTrigger;
        API.onResourceStart += onResourceStart;
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
    }    
}