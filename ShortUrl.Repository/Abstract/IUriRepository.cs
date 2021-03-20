using ShortUrl.Model;
using System.Collections.Generic;

namespace ShortUrl.Repository.Abstract
{
    public interface IUriRepository
    {
        IEnumerable<ShortURLModel> GetCollectionFromDataStore();
        ShortURLModel GetItemFromDataStoreByShortUrl(string shortUrl);
        ShortURLModel GetItemFromDataStoreByLongUrl(string shortUrl);

        ShortURLModel SaveItemToDataStore(ShortURLModel model);
    }
}
