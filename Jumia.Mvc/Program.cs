using Jumia.Application.Contract;
using Jumia.Application.Services;
using Jumia.Context;
using Jumia.InfraStructure.Repository;
using Jumia.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<IItemServices, ItemServices>();
            builder.Services.AddScoped<IItemReposatory, ItemRepostory>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderReposatory, OrderRepository>();
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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
