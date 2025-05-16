using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Msar_Gded.Models;

namespace Msar_Gded
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure DbContext with SQL Server
            builder.Services.AddDbContextPool<AppDbContext>(options =>
      options.UseSqlServer(
          builder.Configuration.GetConnectionString("Msar_Gded"),
          sqlOptions => sqlOptions.CommandTimeout(10) // Optional: increase timeout
      )
      .EnableSensitiveDataLogging() // Optional: for debugging SQL and parameters
  );



            //Cookies properties handling
            #region
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                // options.Secure = CookieSecurePolicy.Always; // Ensures cookies are only sent over HTTPS
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true; // Prevent JavaScript access
                                                // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Require HTTPS
                options.Cookie.SameSite = SameSiteMode.Strict; // Prevent CSRF attacks
                options.Cookie.IsEssential = true; // Ensure it's always used
                options.ExpireTimeSpan = TimeSpan.FromDays(1); // Temporary expiration, browser will ignore it if session-based

                options.SlidingExpiration = false; // Allow renewal while the session is active
                options.Cookie.Expiration = null; // Ensures browser handles session correctly (Moftaaaaaaaaa7 el faraaaaaag)
                options.Cookie.MaxAge = null; // No fixed expiration

                options.LoginPath = "/Auth/Index"; // Redirect to login if authentication is missing
                options.AccessDeniedPath = "/Auth/AccessDenied"; // Redirect if user lacks permissions
            });
            #endregion


            //Register a service for Identity userIdentity , RoleIdentity , SignIn<anager
            #region
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                // if you want to customize instructions of passwod you cannot walk with userManger
                // you specify the instuructions in the delegate of AddIdentity service
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

            }

            ).AddEntityFrameworkStores<AppDbContext>();
            #endregion

            // Add services to the container
            // (Including enforce [Authorize] globally for all controllers in your ASP.NET Core application).
            builder.Services.AddControllersWithViews(options =>
            {
                // Require authenticated users globally
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
