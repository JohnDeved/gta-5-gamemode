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
    private void player_setRandomClothes(Client player)
    {
        Random c_rdm = new Random();

        JObject glasses = m_glasses[c_rdm.Next(0,m_glasses.Length)];
        JObject hair = m_hair[c_rdm.Next(0,m_hair.Length)];
        JObject headgear = m_headgear[c_rdm.Next(0,m_headgear.Length)];
        JObject mask = m_mask[c_rdm.Next(0,m_mask.Length)];
        JObject pants = m_pants[c_rdm.Next(0,m_pants.Length)];
        JObject shirt = m_shirt[c_rdm.Next(0,m_shirt.Length)];
        JObject shoes = m_shoes[c_rdm.Next(0,m_shoes.Length)];

        API.sendChatMessageToPlayer(player,"~y~GLASSES: ~w~ "+(string)glasses.SelectToken("class")+"; ~y~HAIR: ~w~ "+(string)hair.SelectToken("class")+"; ~y~HEADGEAR: ~w~ "+(string)headgear.SelectToken("class")+"; ~y~mask: ~w~ "+(string)mask.SelectToken("class")+"; ~y~PANTS: ~w~ "+(string)pants.SelectToken("class")+"; ~y~SHIRT: ~w~ "+(string)shirt.SelectToken("class")+"; ~y~SHOES: ~w~ "+(string)shoes.SelectToken("class"));

        int[] glasses_t = misc_getClothesTextures("m_glasses",(string)glasses.SelectToken("class"));
        int[] hair_t = misc_getClothesTextures("m_hair",(string)hair.SelectToken("class"));
        int[] headgear_t = misc_getClothesTextures("m_headgear",(string)headgear.SelectToken("class"));
        int[] mask_t = misc_getClothesTextures("m_mask",(string)mask.SelectToken("class"));
        int[] pants_t = misc_getClothesTextures("m_pants",(string)pants.SelectToken("class"));
        int[] shirt_t = misc_getClothesTextures("m_shirt",(string)shirt.SelectToken("class"));
        int[] shoes_t = misc_getClothesTextures("m_shoes",(string)shoes.SelectToken("class"));

        player_setGlasses(player, (string)glasses.SelectToken("class"), glasses_t[c_rdm.Next(0,glasses_t.Length)]);
        player_setHair(player, (string)hair.SelectToken("class"), hair_t[c_rdm.Next(0,hair_t.Length)]);
        player_setHeadGear(player, (string)headgear.SelectToken("class"), headgear_t[c_rdm.Next(0,headgear_t.Length)]);
        player_setMask(player, (string)mask.SelectToken("class"), mask_t[c_rdm.Next(0,mask_t.Length)]);
        player_setPants(player, (string)pants.SelectToken("class"), pants_t[c_rdm.Next(0,pants_t.Length)]);
        player_setShirt(player, (string)shirt.SelectToken("class"), shirt_t[c_rdm.Next(0,shirt_t.Length)]);
        player_setShoes(player, (string)shoes.SelectToken("class"), shoes_t[c_rdm.Next(0,shoes_t.Length)]);
    }
}