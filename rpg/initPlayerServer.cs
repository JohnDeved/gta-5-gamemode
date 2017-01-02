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
        API.onChatMessage += onChatMessage;
    }

    public void onChatMessage(Client player, string message, CancelEventArgs e)
    {
        if (message.StartsWith("/"))
        {
            e.Cancel = true;
            return;
        }
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