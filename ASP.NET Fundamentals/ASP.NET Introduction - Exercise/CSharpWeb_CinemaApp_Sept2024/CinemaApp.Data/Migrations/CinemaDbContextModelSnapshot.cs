﻿// <auto-generated />
using System;
using CinemaApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    [DbContext(typeof(CinemaDbContext))]
    partial class CinemaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CinemaApp.Data.Models.Cinema", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(85)
                        .HasColumnType("nvarchar(85)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Cinemas", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("0d1b6f8f-1a3e-401b-8636-2fc5e99ccf30"),
                            Location = "Sofia",
                            Name = "Cinema city"
                        },
                        new
                        {
                            Id = new Guid("d037e80b-52b6-480e-9d89-44aad515af4c"),
                            Location = "Plovdiv",
                            Name = "Cinema city"
                        },
                        new
                        {
                            Id = new Guid("9fb7ca6a-0293-4fd4-828b-eee5d9b0c383"),
                            Location = "Varna",
                            Name = "Cinemax"
                        });
                });

            modelBuilder.Entity("CinemaApp.Data.Models.CinemaMovie", b =>
                {
                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CinemaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MovieId", "CinemaId");

                    b.HasIndex("CinemaId");

                    b.ToTable("CinemasMovies", (string)null);
                });

            modelBuilder.Entity("CinemaApp.Data.Models.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Movies", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("84a80e7c-cc50-48f9-aa67-7da1099f87b5"),
                            Description = "In his fourth year at Hogwarts, Harry must reluctantly compete in an ancient wizard tournament after someone mysteriously selects his name, while the Dark Lord secretly conspires something sinister.",
                            Director = "Mike Newel",
                            Duration = 157,
                            Genre = "Fantasy",
                            ReleaseDate = new DateTime(2005, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Harry Potter and the Goblet of fire"
                        },
                        new
                        {
                            Id = new Guid("7a558e42-a127-406b-93a9-530533357e52"),
                            Description = "Among motion pictures of Middle-earth in various formats, The Lord of the Rings is a trilogy of epic fantasy adventure films directed by Peter Jackson, based on the novel The Lord of the Rings by British author J. R. R. Tolkien.",
                            Director = "Peter Jackson",
                            Duration = 178,
                            Genre = "Fantasy",
                            ReleaseDate = new DateTime(2001, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Lord of the Rings"
                        });
                });

            modelBuilder.Entity("CinemaApp.Data.Models.CinemaMovie", b =>
                {
                    b.HasOne("CinemaApp.Data.Models.Cinema", "Cinema")
                        .WithMany("CinemaMovies")
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CinemaApp.Data.Models.Movie", "Movie")
                        .WithMany("MoviesCinema")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cinema");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("CinemaApp.Data.Models.Cinema", b =>
                {
                    b.Navigation("CinemaMovies");
                });

            modelBuilder.Entity("CinemaApp.Data.Models.Movie", b =>
                {
                    b.Navigation("MoviesCinema");
                });
#pragma warning restore 612, 618
        }
    }
}
