using WildBearAdventures2.API;
using Ucommerce.Web.BackOffice.DependencyInjection;
using Ucommerce.Web.Infrastructure.DependencyInjection;
using Ucommerce.Web.WebSite.DependencyInjection;
using Ucommerce.Web.Core.DependencyInjection;
using Ucommerce.Extensions.Search.Abstractions.Extensions;
using Ucommerce.Search.Elastic.Configuration;
using Ucommerce.Extensions.Payment.Abstractions.Extensions;
using WildBearAdventures.API.PipelinesExtensions;

var builder = WebApplication.CreateBuilder(args);

// Set up services
builder.Services.AddUcommerce(builder.Configuration)
    .AddBackOffice()
    .AddWebSite()
    .AddSearch()
    .UcommerceBuilder
    .AddElasticsearch()
    .AddPayments()
    .AddMyOrderProcessingExtensions()
    .Build();





builder.Services.AddControllers();

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
