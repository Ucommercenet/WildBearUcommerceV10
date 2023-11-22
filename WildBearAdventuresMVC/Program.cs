using WildBearAdventuresMVC.WildBear;
using WildBearAdventuresMVC.WildBear.Interfaces;
using WildBearAdventuresMVC.WildBear.TransactionApi;

namespace WildBearAdventuresMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);






            // Custom services                      
            builder.Services.AddTransient<IStoreApiClient, WildBearApiClient>();

            builder.Services.AddTransient<IContextHelper, ContextHelper>();
            builder.Services.AddTransient<TransactionClient>();

            //Only ever need one StoreAuthentication
            builder.Services.AddSingleton<StoreAuthentication>();
            builder.Services.AddSingleton<StoreAuthorizationFlow>();



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
