using WildBear.Shop.Coffee.WildBear.Context;
using WildBear.Shop.Coffee.WildBear.TransactionApi;
using WildBear.Shop.Coffee.WildBear.WildBearApi;

namespace WildBear.Shop.Coffee
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Custom services                      
            builder.Services.AddTransient<IStoreApiClient, WildBearClient>();

            builder.Services.AddTransient<IContextHelper, ContextHelper>();
            builder.Services.AddTransient<TransactionClient>();

            //Only ever need one StoreAuthDetails
            builder.Services.AddSingleton<StoreAuthDetails>();
            builder.Services.AddSingleton<StoreAuthorization>();



            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();
            builder.Services.AddHttpClient();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseAuthorization();

            app.UseSession();



            app.MapDefaultControllerRoute();
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
