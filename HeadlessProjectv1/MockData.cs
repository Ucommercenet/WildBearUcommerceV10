using System.Globalization;


namespace WildBearAdventures.API
{
    public static class MockData
    {
        public static Language GetDanishLanguage()
        {
            return new Language()
            {
                Name = "Danish",
                Culture = new CultureInfo("da-DK")
            };
        }

    }

    public class Language
    {
        public string? Name { get; set; }

        public required CultureInfo Culture { get; set; }
    }
}
