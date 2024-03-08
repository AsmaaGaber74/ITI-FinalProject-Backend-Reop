using Jumia.Application.Contract;
using Jumia.InfraStructure;
using Jumia.Application.Services;
using Jumia.Context;
using Jumia.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Jumia.InfraStructure.Repository;

namespace Jumia.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<JumiaContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("Db"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.Password.RequireDigit = true)
        .AddEntityFrameworkStores<JumiaContext>()
        .AddDefaultTokenProviders();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderReposatory,OrderRepository >();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();//to check the cockie
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=admin}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
