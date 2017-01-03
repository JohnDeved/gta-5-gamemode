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
    private JObject[] m_masc;
    private JObject[] m_pants;
    private JObject[] m_shirt;
    private JObject[] m_shoes;

    private void misc_importClothes()
    {
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json","m_glasses"));
        m_glasses = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_glasses[i] = JObject.Parse(Lines[i]);
        }
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json","m_hair"));
        m_hair = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_hair[i] = JObject.Parse(Lines[i]);
        }
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json","m_headgear"));
        m_headgear = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_headgear[i] = JObject.Parse(Lines[i]);
        }
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json","m_masc"));
        m_masc = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_masc[i] = JObject.Parse(Lines[i]);
        }
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json","m_pants"));
        m_pants = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_pants[i] = JObject.Parse(Lines[i]);
        }
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json","m_shirt"));
        m_shirt = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_shirt[i] = JObject.Parse(Lines[i]);
        }
        string[] Lines = File.ReadAllLines(string.Format(@"resources\rpg\player\clothes\{0}.json","m_shoes"));
        m_shoes = new JObject[Lines.Length];
        for(int i = 0;i < Lines.Length;i++)
        {
            m_shoes[i] = JObject.Parse(Lines[i]);
        }
    }
}