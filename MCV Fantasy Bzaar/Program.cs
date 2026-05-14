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
                // Here I set up the connection to the local SQL database
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddTreblle("PLACEHOLDER_SDK_TOKEN", "PLACEHOLDER_PROJECT_ID");
                // Above, I have placeholders for the Treblle SDK token and project ID, which should be replaced with actual values from the Treblle dashboard to enable API monitoring and analytics for the application.
                // The connection string below is configured to connect to a local SQL Server database named "FantasyBzaarDB" using Windows Authentication.
                // This setup allows the application to interact with the database for storing and retrieving comic book data, user interactions, and other relevant information needed for the encyclopedia functionality.
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