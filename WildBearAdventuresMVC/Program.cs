using WildBearAdventuresMVC.WildBear;

namespace WildBearAdventuresMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddControllersWithViews();

            // Custom services
            //QUEST: AddTransient or AddScoped, need to understand scopes
            builder.Services.AddTransient<IWildBearApiClient, WildBearApiClient>();
            builder.Services.AddTransient<IContextHelper, ContextHelper>();


            //For now Sessions are used for the cart
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();





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
            app.UseAuthorization();

            app.UseSession();



            app.MapDefaultControllerRoute();
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
