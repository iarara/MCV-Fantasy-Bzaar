using Treblle.Net.Core;
using MCV_Fantasy_Bzaar.Models;
using MCV_Fantasy_Bzaar.Services;
using Microsoft.EntityFrameworkCore;

namespace MCV_Fantasy_Bzaar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            {
                // Here I set up the connection to the local SQL database,
                // add the necessary services for dependency injection, and configure the app's request pipeline with routing and error handling

                var builder = WebApplication.CreateBuilder(args);
                builder.Services.AddTreblle("PLACEHOLDER_SDK_TOKEN", "PLACEHOLDER_PROJECT_ID");
                var connectionString = "Server=(localdb)\\mssqllocaldb;Database=FantasyBzaarDB;Trusted_Connection=True;MultipleActiveResultSets=true";

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));

                builder.Services.AddSingleton<EncyclopediaService>();

                builder.Services.AddControllersWithViews();
                var app = builder.Build();

                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseTreblle();
                app.UseRouting();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Encyclopedia}/{action=Index}/{id?}");

                app.Run();
            }
        }
    }
}