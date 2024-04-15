using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MySqlConnector;
using Taskel.Authentication;
using Taskel.Services;
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
            builder.Services.AddAuthenticationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddScoped<ProtectedSessionStorage>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TaskelAuthenticationStateProvider>());
            builder.Services.AddScoped<AuthenticationStateProvider, TaskelAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<CreditService>();
            var app = builder.Build();

            var connectionString = builder.Configuration["ConnectionStrings:DatabaseConnection"] ?? "";
            MySqlConnectionStringBuilder stringBuilder = new()
            {
                ConnectionString = connectionString,
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