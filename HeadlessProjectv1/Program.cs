using HeadlessProjectv1;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Ucommerce.Extensions.Payment.Abstractions.Extensions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;
using Ucommerce.Search.Elastic.Configuration;
using Ucommerce.Web.BackOffice.DependencyInjection;
using Ucommerce.Web.Core.DependencyInjection;
using Ucommerce.Web.Infrastructure.DependencyInjection;
using Ucommerce.Web.WebSite.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

#region Default Ucommerce

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

#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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