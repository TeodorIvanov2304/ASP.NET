using CinemaApp.Data;
using CinemaApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.Web.Infrastructure.Extensions.ApplicationBuilderExtensions;

namespace CinemaApp.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			string connectionString = builder.Configuration.GetConnectionString("SQLServer")!;

			// Add services to the container.

			//Enable using Dependency injection on DbContext
			builder.Services.AddDbContext<CinemaDbContext>(options =>
					 options.UseSqlServer(connectionString));


			builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(configuration =>
				{
					ConfigureIdentity(builder,configuration);
				})
				.AddEntityFrameworkStores<CinemaDbContext>()
				.AddRoles<IdentityRole<Guid>>()
				.AddSignInManager<SignInManager<ApplicationUser>>()
				.AddUserManager<UserManager<ApplicationUser>>();

			builder.Services.ConfigureApplicationCookie(cfg =>
			{
				cfg.LoginPath = "/Identity/Account/Login";
			});

			builder.Services.AddControllersWithViews();
			
			builder.Services.AddRazorPages();
				
				

			WebApplication app = builder.Build();


			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.MapRazorPages();

			await app.ApplyMigrations();


			app.Run();
		}

		//Get the password properties from user secrets
		private static void ConfigureIdentity(WebApplicationBuilder builder, IdentityOptions configuration)
		{

			//Password
			configuration.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
			configuration.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
			configuration.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
			configuration.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumerical");
			configuration.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
			configuration.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueCharacters");

			//Sign in
			configuration.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SingIn:RequireConfirmedAccount");
			configuration.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SingIn:RequireConfirmedEmail");
			configuration.SignIn.RequireConfirmedPhoneNumber = builder.Configuration.GetValue<bool>("Identity:SingIn:RequireConfirmedPhoneNumber");

			//User
			configuration.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("Identity:SingIn:RequireUniqueEmail");
		}
	}
}
