using Microsoft.AspNetCore.Authorization.Infrastructure;
using HeadlessProjectv1;
using Ucommerce.Web.BackOffice.DependencyInjection;
using Ucommerce.Web.Infrastructure.DependencyInjection;
using Ucommerce.Web.WebSite.DependencyInjection;
using Ucommerce.Web.Core.DependencyInjection;
using Ucommerce.Extensions.Search.Abstractions.Extensions;
using Ucommerce.Search.Elastic.Configuration;
using Ucommerce.Extensions.Payment.Abstractions.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Set up services
builder.Services.AddUcommerce(builder.Configuration)
    .AddBackOffice(opt => { opt.Security.Requirements.Add(new AssertionRequirement(_ => true)); })
    .AddWebSite()
    .AddSearch()
        .AddElasticSearch()
        .WithSingleNodeSetup()
    .AddPayments()
    .Build();

builder.Services.AddControllers();

var app = builder.Build();

// configure and run
app.UseTestUser();
app.UseUcommerce()
    .UseBackOfficeUi()
    .UsePayments()
    .UseEndpoints(u => { u.UseUcommerceEndpoints(); });

app.Run();
