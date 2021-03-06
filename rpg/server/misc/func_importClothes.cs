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
using Newtonsoft.Json.Linq;

public partial class rpg : Script
{
    private JObject[] m_glasses;
    private JObject[] m_hair;
    private JObject[] m_headgear;
    private JObject[] m_mask;
    private JObject[] m_pants;
    private JObject[] m_shirt;
    private JObject[] m_shoes;

    private Dictionary<string, int> ClothingParts = new Dictionary<string, int>
    {
        {"face", 0},
        {"mask", 1},
        {"hair", 2},
        {"arms", 3},
        {"legs", 4},
        {"back", 5},
        {"shoes", 6},
        {"ties", 7},
        {"inner", 8},
        {"vest", 9},
        {"decals", 10},
        {"shirt", 11}
    };    

    private void misc_importClothes()
    {
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\server\player\clothes\{0}.json","m_glasses"));
        m_glasses = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_glasses[i] = JObject.Parse(Lines[i]);
        }
        Lines = File.ReadAllLines(string.Format(@"resources\rpg\server\player\clothes\{0}.json","m_hair"));
        m_hair = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_hair[i] = JObject.Parse(Lines[i]);
        }
        Lines = File.ReadAllLines(string.Format(@"resources\rpg\server\player\clothes\{0}.json","m_headgear"));
        m_headgear = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_headgear[i] = JObject.Parse(Lines[i]);
        }
        Lines = File.ReadAllLines(string.Format(@"resources\rpg\server\player\clothes\{0}.json","m_mask"));
        m_mask = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_mask[i] = JObject.Parse(Lines[i]);
        }
        Lines = File.ReadAllLines(string.Format(@"resources\rpg\server\player\clothes\{0}.json","m_pants"));
        m_pants = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_pants[i] = JObject.Parse(Lines[i]);
        }
        Lines = File.ReadAllLines(string.Format(@"resources\rpg\server\player\clothes\{0}.json","m_shirt"));
        m_shirt = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_shirt[i] = JObject.Parse(Lines[i]);
        }
        Lines = File.ReadAllLines(string.Format(@"resources\rpg\server\player\clothes\{0}.json","m_shoes"));
        m_shoes = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_shoes[i] = JObject.Parse(Lines[i]);
        }
    }
}