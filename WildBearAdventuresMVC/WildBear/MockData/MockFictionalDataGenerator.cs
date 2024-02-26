namespace WildBearAdventures.MVC.WildBear.MockData
{
    public static class MockFictionalDataGenerator
    {
        public static MockAddress GetRandomAddress()
        {
            var random = new Random();
            var addresses = new List<MockAddress>
        {
            // Star Wars
            new MockAddress
            {
                City = "Coruscant",
                CompanyName = "",
                CountryId = "SW",
                Email = "luke@jediorder.com",
                FirstName = "Luke",
                LastName = "Skywalker",
                Line1 = "Jedi Temple",
                Line2 = "Main Street",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "100001",
                State = "Galactic Core"
            },
            // Dune
            new MockAddress
            {
                City = "Arrakis",
                CompanyName = "Spice Mining Inc.",
                CountryId = "DUN",
                Email = "paul@atreides.com",
                FirstName = "Paul",
                LastName = "Atreides",
                Line1 = "Sietch Tabr",
                Line2 = "Desert Street",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "20000",
                State = "The Imperium"
            },
            // Tron
            new MockAddress
            {
                City = "Tron City",
                CompanyName = "",
                CountryId = "TRN",
                Email = "kevin@tron.com",
                FirstName = "Kevin",
                LastName = "Flynn",
                Line1 = "ENCOM Tower",
                Line2 = "Grid Street",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "010101",
                State = "The Grid"
            },
            // Game of Thrones
            new MockAddress
            {
                City = "King's Landing",
                CompanyName = "",
                CountryId = "GOT",
                Email = "jon@snow.com",
                FirstName = "Jon",
                LastName = "Snow",
                Line1 = "Winterfell",
                Line2 = "Stark Street",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "30001",
                State = "The North"
            },
            // Blade Runner
            new MockAddress
            {
                City = "Los Angeles",
                CompanyName = "Tyrell Corporation",
                CountryId = "BLR",
                Email = "rick@blade.com",
                FirstName = "Rick",
                LastName = "Deckard",
                Line1 = "Tyrell Building",
                Line2 = "Replicant Lane",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "90001",
                State = "California"
            },
            // The Matrix
            new MockAddress
            {
                City = "Zion",
                CompanyName = "",
                CountryId = "MTR",
                Email = "neo@zion.com",
                FirstName = "Neo",
                LastName = "",
                Line1 = "Nebuchadnezzar",
                Line2 = "Zion Avenue",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "70701",
                State = "Zion"
            },
            // Star Trek
            new MockAddress
            {
                City = "San Francisco",
                CompanyName = "",
                CountryId = "STR",
                Email = "james@starfleet.com",
                FirstName = "James",
                LastName = "Kirk",
                Line1 = "Starfleet Headquarters",
                Line2 = "Starship Road",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "94101",
                State = "Earth"
            },
            // Harry Potter
            new MockAddress
            {
                City = "London",
                CompanyName = "",
                CountryId = "HPT",
                Email = "harry@hogwarts.com",
                FirstName = "Harry",
                LastName = "Potter",
                Line1 = "4 Privet Drive",
                Line2 = "",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "WC1A 1AA",
                State = "England"
            },
            // Marvel Universe
            new MockAddress
            {
                City = "New York",
                CompanyName = "",
                CountryId = "MAR",
                Email = "peter@spiderman.com",
                FirstName = "Peter",
                LastName = "Parker",
                Line1 = "Daily Bugle",
                Line2 = "Web Street",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "10001",
                State = "New York"
            },
            // DC Universe
            new MockAddress
            {
                City = "Gotham City",
                CompanyName = "",
                CountryId = "DC",
                Email = "bruce@batman.com",
                FirstName = "Bruce",
                LastName = "Wayne",
                Line1 = "Wayne Manor",
                Line2 = "Batcave Lane",
                MobileNumber = "",
                PhoneNumber = "",
                PostalCode = "12345",
                State = "New Jersey"
            }
        };

            return addresses[random.Next(addresses.Count)];
        }
    }
    public class MockAddress
    {
        public string City { get; set; }
        public string CompanyName { get; set; }
        public string CountryId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
    }




}
