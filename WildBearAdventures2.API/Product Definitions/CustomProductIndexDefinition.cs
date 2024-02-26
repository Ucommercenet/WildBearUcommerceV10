﻿using Ucommerce.Extensions.Search.Abstractions.DefaultDefinitions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;

namespace WildBearAdventures.API.Product_Definitions
{
    public class CustomProductIndexDefinition : DefaultProductsIndexDefinition
    {

        public CustomProductIndexDefinition() : base()
        {
            this.Field(p => p["OriginCountry"], typeof(string));

            //TODO: report bug around user defined fields somethimes not geting in the index
            //this.Field(p => p.PricesInclTax["EUR 15 pct"]);


            this.Field(p => p["Kan downloades"], typeof(bool));

        }
    }
}
