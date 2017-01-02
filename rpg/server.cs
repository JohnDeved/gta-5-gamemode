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
}