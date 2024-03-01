using MySqlConnector;
using Taskel.Services;
using Taskel.Services.Authorization;
using TaskelDB;

namespace Taskel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<SessionService>();
            var app = builder.Build();

            MySqlConnectionStringBuilder stringBuilder = new()
            {
                UserID = "ppraxe",
                Password = "Ppraxe+01",
                Database = "praxedb",
                Server = "93.99.225.235",
                ConnectionTimeout = 10,
            };

            DBConnection conn = DBConnection.Instance;
            conn.ConnectionString = stringBuilder.ConnectionString;

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.Run();
        }
    }
}