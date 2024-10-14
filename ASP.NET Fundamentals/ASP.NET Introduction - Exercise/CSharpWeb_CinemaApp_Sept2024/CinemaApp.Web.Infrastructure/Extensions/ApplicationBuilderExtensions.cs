using CinemaApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaApp.Web.Infrastructure.Extensions
{
	public static class ApplicationBuilderExtensions 
	{
		public static async Task<IApplicationBuilder> ApplyMigrations(this IApplicationBuilder app)
		{
			using IServiceScope serviceScope =   app.ApplicationServices.CreateAsyncScope(); 

			CinemaDbContext dbContext =  serviceScope.ServiceProvider.GetRequiredService<CinemaDbContext>()!;

			await dbContext.Database.MigrateAsync();

			return app;
		}
	}
}
