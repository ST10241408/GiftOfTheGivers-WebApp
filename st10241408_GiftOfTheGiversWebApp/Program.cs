using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10241408_GiftOfTheGiversWebApp
    ;

namespace ST10241408_GiftOfTheGiversWebApp.Data
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create a new WebApplicationBuilder instance
            var builder = WebApplication.CreateBuilder(args);

            // Get the connection string from the configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Add the ApplicationDbContext to the services collection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add the database developer page exception filter
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add default identity services with options to require confirmed account
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add controllers with views
            builder.Services.AddControllersWithViews();

            // Build the application
            var app = builder.Build();

            // Check if the environment is development
            if (app.Environment.IsDevelopment())
            {
                // Use the migrations endpoint
                app.UseMigrationsEndPoint();
            }
            else
            {
                // Use the exception handler
                app.UseExceptionHandler("/Home/Error");

                // Use HSTS (HTTP Strict Transport Security)
                app.UseHsts();
            }

            // Use HTTPS redirection
            app.UseHttpsRedirection();

            // Use static files
            app.UseStaticFiles();

            // Use routing
            app.UseRouting();

            // Use authorization
            app.UseAuthorization();

            // Map the default controller route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Map Razor Pages
            app.MapRazorPages();

            // Create a scope for the services
            using (var scope = app.Services.CreateScope())
            {
                // Get the role manager
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Define the roles to create
                var roles = new[] { "Admin", "User" };

                // Create each role if it doesn't exist
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Get the user manager
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Define the admin user credentials
                string email = "admin@admin.com";
                string password = "Test1234,";

                // Check if the admin user exists
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    // Create the admin user
                    var user = new IdentityUser
                    {
                        UserName = email,
                        Email = email
                    };

                    // Create the user and add to the Admin role
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Run the application
            app.Run();
        }
    }
}