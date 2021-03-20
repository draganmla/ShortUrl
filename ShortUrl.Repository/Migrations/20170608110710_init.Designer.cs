using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ShortUrl.Repository;


namespace UrlShortener.Migrations
{
    [DbContext(typeof(UriDbContext))]
    [Migration("20170608110710_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("UrlShortener.Models.ShortUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OriginalUrl");

                    b.HasKey("Id");

                    b.ToTable("ShortUrls");
                });
        }
    }
}
