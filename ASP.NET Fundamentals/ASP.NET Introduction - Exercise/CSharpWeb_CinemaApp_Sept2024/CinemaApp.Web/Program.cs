using CinemaApp.Data;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.Web.Infrastructure.Extensions.ApplicationBuilderExtensions;

namespace CinemaApp.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			string connectionString = builder.Configuration.GetConnectionString("SQLServer")!;

			// Add services to the container.

			//Enable using Dependency injection on DbContext
			builder.Services.AddDbContext<CinemaDbContext>(options =>
					 options.UseSqlServer(connectionString));

			builder.Services.AddControllersWithViews();

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

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.ApplyMigrations();

			app.Run();
		}
	}
}
