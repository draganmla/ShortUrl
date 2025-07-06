using ShortUrl.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShortUrl.Repository.Abstract
{
    public interface IUriRepository
    {
        // Sync methods (keep for backward compatibility)
        IEnumerable<ShortURLModel> GetCollectionFromDataStore();
        ShortURLModel GetItemFromDataStoreByShortUrl(string shortUrl);
        ShortURLModel GetItemFromDataStoreByLongUrl(string longUrl);
        ShortURLModel SaveItemToDataStore(ShortURLModel model);
        
        // Async methods (recommended for better performance)
        Task<IEnumerable<ShortURLModel>> GetCollectionFromDataStoreAsync();
        Task<ShortURLModel> GetItemFromDataStoreByShortUrlAsync(string shortUrl);
        Task<ShortURLModel> GetItemFromDataStoreByLongUrlAsync(string longUrl);
        Task<ShortURLModel> SaveItemToDataStoreAsync(ShortURLModel model);
        Task<bool> ShortUrlExistsAsync(string shortUrl);
        Task<bool> LongUrlExistsAsync(string longUrl);
        Task<int> GetTotalUrlCountAsync();
        Task<ShortURLModel> UpdateItemAsync(ShortURLModel model);
        Task<bool> DeleteItemAsync(string shortUrl);
        Task<int> SaveChangesAsync();
    }
}
