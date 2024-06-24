using Ucommerce.Web.BackOffice.DependencyInjection;
using Ucommerce.Web.Infrastructure.DependencyInjection;
using Ucommerce.Web.WebSite.DependencyInjection;
using Ucommerce.Web.Core.DependencyInjection;
using Ucommerce.Extensions.Search.Abstractions.Extensions;
using Ucommerce.Search.Elastic.Configuration;
using Ucommerce.Extensions.Payment.Abstractions.Extensions;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Common.Extensions;
using WildBearAdventures.API;
using WildBearAdventures.API.WildBearProducts;
using WildBearAdventures.API.WildBearDemoProducts;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    //Set up services
    .AddUcommerce(builder.Configuration)
    .AddBackOffice()
    .AddWebSite()
    .AddSearch()
    .UcommerceBuilder
    .AddElasticsearch()
    .AddPayments()
    //Custom pipeline Tasks
    //TODO: Scenario 3: Pipeline customization
    

    //Final builder setup
    .Build();

builder.Services.AddControllers();

builder.Services.AddUnique<IIndexDefinition<ProductSearchModel>, ProductIndexDefinition>();
builder.Services.AddTransient<DemoToolbox>();

#region Swagger Related
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.EnableAnnotations(); });
#endregion

var app = builder.Build();

#region Swagger Related
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 
#endregion

// configure and run
app.UseTestUser();
app.UseUcommerce()
    .UseEndpoints(u => { u.UseUcommerceEndpoints(); })
    .UsePayments()
    .UseBackOfficeUi();

app.Run();
