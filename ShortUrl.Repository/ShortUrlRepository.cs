using ShortUrl.Model;
using ShortUrl.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortUrl.Repository
{
    public class ShortUrlRepository : BaseRepository<ShortURLModel>, IUriRepository
    {
        public ShortUrlRepository(UriDbContext context) : base(context) { }
        public IEnumerable<ShortURLModel> GetCollectionFromDataStore()
        {
            return GetBy(null,null,"");
        }

        public ShortURLModel GetItemFromDataStoreByShortUrl(string shortUrl)
        {
            return GetBy(c => c.ShortURL == shortUrl,null,"").FirstOrDefault();
        }


        public ShortURLModel GetItemFromDataStoreByLongUrl(string longUrl)
        {
            return GetBy(c => c.LongURL == longUrl).FirstOrDefault();
        }

        public ShortURLModel SaveItemToDataStore(ShortURLModel model)
        {
            try
            {
                this.Insert(model);
            }
            catch (Exception)
            {
                return null;
            }

            return model;
        }
    }
}
