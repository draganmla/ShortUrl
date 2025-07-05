using ShortUrl.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShortUrl.Service.Abstract
{
    public interface IShortUrlService
    {
        IEnumerable<ShortURLModel> GetCollectionFromDataStore();
        ShortURLModel GetItemFromDataStore(string shortUrl);
        ShortUrlResponseModel SaveItemToDataStore(ShortURLRequestModel model);
        
        // Async versions for better performance
        Task<IEnumerable<ShortURLModel>> GetCollectionFromDataStoreAsync();
        Task<ShortURLModel> GetItemFromDataStoreAsync(string shortUrl);
        Task<ShortUrlResponseModel> SaveItemToDataStoreAsync(ShortURLRequestModel model);
    }
}
