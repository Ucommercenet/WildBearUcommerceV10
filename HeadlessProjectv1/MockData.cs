using System.Globalization;
using Ucommerce.Web.Infrastructure.Core.Models;

namespace HeadlessProjectv1
{
    public static class MockData
    {
        public static Language GetDanishLangauge()
        {
            return new Language()
            {
                Name = "Danish",
                Culture = new CultureInfo("da-DK")
            };
        }






    }
}
