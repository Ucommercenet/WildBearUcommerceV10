using System.Globalization;

namespace WildBearAdventuresMVC.WildBear
{
    public static class ShopInfo
    {
        //Danish is default culture on a Ucommerce v10 install
        public static readonly CultureInfo Default = new("da-DK");


        //Example "https://localhost:12345"
        public static readonly string UriForAPIs = "https://localhost:44381/";


    }


}
