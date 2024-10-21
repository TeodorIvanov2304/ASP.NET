using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data.Models;

namespace SoftUniBazar.Data
{
    public class BazarDbContext : IdentityDbContext
    {
        public BazarDbContext(DbContextOptions<BazarDbContext> options)
            : base(options)
        {
        }

		public virtual DbSet<Ad> Ads { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<AdBuyer> AdsBuyers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {	
			//Price precision
			modelBuilder.Entity<Ad>()
		   .Property(a => a.Price)
		   .HasColumnType("decimal(18, 2)"); 

			modelBuilder
				.Entity<Category>()
				.HasData(new Category()
				{
					Id = 1,
					Name = "Books"
				},
				new Category()
				{
					Id = 2,
					Name = "Cars"
				},
				new Category()
				{
					Id = 3,
					Name = "Clothes"
				},
				new Category()
				{
					Id = 4,
					Name = "Home"
				},
				new Category()
				{
					Id = 5,
					Name = "Technology"
				});

			base.OnModelCreating(modelBuilder);
		}
    }
}