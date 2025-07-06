using Microsoft.EntityFrameworkCore;
using ShortUrl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortUrl.Repository
{
    public class UriDbContext : DbContext
    {
        public UriDbContext(DbContextOptions<UriDbContext> options)
            : base(options)
        {
        }

        public DbSet<ShortURLModel> ShortUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure ShortURLModel entity
            modelBuilder.Entity<ShortURLModel>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.Id);
                
                // Configure properties
                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();
                    
                entity.Property(e => e.ShortURL)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("VARCHAR(50)");
                    
                entity.Property(e => e.LongURL)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnType("VARCHAR(2000)");
                
                // Create indexes for performance
                entity.HasIndex(e => e.ShortURL)
                    .HasDatabaseName("IX_ShortUrls_ShortURL")
                    .IsUnique();
                    
                entity.HasIndex(e => e.LongURL)
                    .HasDatabaseName("IX_ShortUrls_LongURL");
                
                // Table configuration
                entity.ToTable("ShortUrls");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Enable query splitting for better performance
            optionsBuilder.UseSqlite(options => options.CommandTimeout(30));
            
            // Enable sensitive data logging only in development
            #if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            #endif
            
            // Configure query behavior
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }
    }
}
