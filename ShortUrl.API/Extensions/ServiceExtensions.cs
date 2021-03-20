using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortUrl.Repository;
using ShortUrl.Repository.Abstract;
using ShortUrl.Service;
using ShortUrl.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortUrl.API.Extensions
{
    public static class ServiceExtensions
    {

        public static void RegisterDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            //string mongoConnectionString = Configuration.GetConnectionString("MongoConnectionString");
            services.AddTransient<IUriRepository, ShortUrlRepository>();
            services.AddTransient<IShortUrlService, ShortUrlServices>();
        }
    }
}
