using Microsoft.AspNetCore.Mvc;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Persistence;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WildBearAdventures.API.ApiControllersForScenarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class WildBearStartupController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public WildBearStartupController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }



        //PUT requests are idempotent, meaning that sending the same request multiple times will have the same effect as sending it once.
        // PUT api/<WildBearStartupController>/5
        [HttpPut]
        public IActionResult Put()
        {
            //Step 1 Create product definition
            //Step 2 Check if definition already exists

            using var asyncScope = _serviceProvider.CreateAsyncScope();
            var dbContext = asyncScope.ServiceProvider.GetRequiredService<UcommerceDbContext>();

            const string ProductDefinitionName = "WildCoffee";
            const string ProductDefinitionDescription = "Definition for Coffee type products";
            const string ProductDefinitionFieldName = "OriginCountry";

            //Check if definition already exists
            //TODO Improvement: Make it works for updateing ProductDefinitions as well.

            //TODO: this is never true because ProductDefinitionName is random later, but useful for next iteration.
            var exists = dbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == ProductDefinitionName);
            if (exists) { return Ok("WildBearStartup products already exists"); }


            var WildCoffeeProductDefinition = new ProductDefinitionEntity()
            {
                Name = $"{ProductDefinitionName}{RandomLetterAndNumber()}",
                Description = ProductDefinitionDescription,
                Deleted = false
            };

            dbContext.Add(WildCoffeeProductDefinition);
            dbContext.SaveChanges();


            return Ok($"{WildCoffeeProductDefinition.Name} added");
        }





        // POST api/<WildBearStartupController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        private string RandomLetterAndNumber()
        {
            var random = new Random();

            // Generate a random letter (A-Z)
            char letter = (char)('A' + random.Next(0, 26));

            // Generate a random number (0-9)
            int number = random.Next(0, 10);

            // Concatenate the letter and number and return as a string
            return letter.ToString() + number.ToString();
        }



    }
}
