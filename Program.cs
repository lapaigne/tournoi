using Microsoft.EntityFrameworkCore;
using Tournoi.Battles;
using Tournoi.DB;

namespace Tournoi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlite("Data Source=database.db"));
            builder.Services.AddSingleton<BattlePlanner>();
            builder.Services.AddAuthentication().AddCookie("Cookies", options =>
            {
                options.Cookie.Name = "jac";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(3);
                options.SlidingExpiration = true;
                options.LoginPath = "/SignIn";
            });

            builder.Services.AddAuthorization();

            builder.Services.AddRazorPages();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                db.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}