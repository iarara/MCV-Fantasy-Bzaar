using MCV_Fantasy_Bzaar.Models;
using MCV_Fantasy_Bzaar.Services;
using Microsoft.EntityFrameworkCore;

namespace MCV_Fantasy_Bzaar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=FantasyBzaarDB;Trusted_Connection=True;MultipleActiveResultSets=true";

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<EncyclopediaService>();
            builder.Services.AddControllersWithViews();
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
