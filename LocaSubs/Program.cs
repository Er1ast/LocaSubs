using LocaSubs.Contracts;
using LocaSubs.DataAccess;
using LocaSubs.DataAccess.Repositories;
using LocaSubs.External.YClients;
using LocaSubs.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LocaSubs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<LocaSubsDbContext>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddTransient<IServiceReceiver, ServiceReceiver>();
            builder.Services.AddTransient<IYClientsFacade, YClientsFacade>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/sign-in");
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();            
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles();

            //app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllers();

            app.Run();
        }
    }
}
