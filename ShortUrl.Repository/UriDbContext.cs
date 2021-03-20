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
    }
}
