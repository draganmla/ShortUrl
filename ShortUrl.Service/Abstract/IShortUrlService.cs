using ShortUrl.Model;
using System.Collections.Generic;

namespace ShortUrl.Service.Abstract
{
    public interface IShortUrlService
    {
        IEnumerable<ShortURLModel> GetCollectionFromDataStore();
        ShortURLModel GetItemFromDataStore(string shortUrl);
        ShortUrlResponseModel SaveItemToDataStore(ShortURLRequestModel model);
    }
}
