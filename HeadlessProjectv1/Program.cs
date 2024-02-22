using Microsoft.AspNetCore.Authorization.Infrastructure;
using Ucommerce.Extensions.Payment.Abstractions.Extensions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Search.Elastic.Configuration;
using Ucommerce.Web.BackOffice.DependencyInjection;
using Ucommerce.Web.Common.Extensions;
using Ucommerce.Web.Core.DependencyInjection;
using Ucommerce.Web.Infrastructure.DependencyInjection;
using Ucommerce.Web.WebSite.DependencyInjection;
using WildBearAdventures.API;
using WildBearAdventures.API.Product_Definitions;

var builder = WebApplication.CreateBuilder(args);

#region Default Ucommerce Related

// Set up services
builder.Services.AddUcommerce(builder.Configuration)
    .AddBackOffice(opt => { opt.Security.Requirements.Add(new AssertionRequirement(_ => true)); })
    .AddWebSite()
    .AddSearch()
    .AddElasticSearch()
    .WithSingleNodeSetup()
    .AddPayments()
    .Build();
#endregion

#region Further Customizing Ucommerce 

//New Coffee Product Definition
builder.Services.AddUnique<IIndexDefinition<ProductSearchModel>, CustomProductIndexDefinition>(ServiceLifetime.Scoped);
builder.Services.AddHostedService<SetupDefinitions>(); 
#endregion

#region EndPoints Related

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.EnableAnnotations(); }); 
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Default setup from ASP.net Core API Template 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

#region Default Ucommerce

// configure and run
app.UseTestUser();
app.UseUcommerce()
    .UseBackOfficeUi()
    .UsePayments()
    .UseEndpoints(u => { u.UseUcommerceEndpoints(); });

#endregion

app.Run();