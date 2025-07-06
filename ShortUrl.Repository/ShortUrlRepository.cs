using ShortUrl.Model;
using ShortUrl.Repository.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortUrl.Repository
{
    public class ShortUrlRepository : BaseRepository<ShortURLModel>, IUriRepository
    {
        private readonly ILogger<ShortUrlRepository> _logger;

        public ShortUrlRepository(UriDbContext context, ILogger<ShortUrlRepository> logger = null) 
            : base(context) 
        {
            _logger = logger;
        }

        #region Sync Methods (Legacy Support)

        public IEnumerable<ShortURLModel> GetCollectionFromDataStore()
        {
            try
            {
                return GetBy(null, null, "");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving all short URLs from data store");
                throw;
            }
        }

        public ShortURLModel GetItemFromDataStoreByShortUrl(string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
                throw new ArgumentException("Short URL cannot be null or empty", nameof(shortUrl));

            try
            {
                return GetBy(c => c.ShortURL == shortUrl, null, "").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving short URL: {ShortUrl}", shortUrl);
                throw;
            }
        }

        public ShortURLModel GetItemFromDataStoreByLongUrl(string longUrl)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
                throw new ArgumentException("Long URL cannot be null or empty", nameof(longUrl));

            try
            {
                return GetBy(c => c.LongURL == longUrl).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving by long URL: {LongUrl}", longUrl);
                throw;
            }
        }

        public ShortURLModel SaveItemToDataStore(ShortURLModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                Insert(model);
                return model;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error saving short URL: {ShortUrl} -> {LongUrl}", model.ShortURL, model.LongURL);
                return null;
            }
        }

        #endregion

        #region Async Methods (Recommended)

        public async Task<IEnumerable<ShortURLModel>> GetCollectionFromDataStoreAsync()
        {
            try
            {
                return await GetByAsync(null, null, "");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving all short URLs from data store async");
                throw;
            }
        }

        public async Task<ShortURLModel> GetItemFromDataStoreByShortUrlAsync(string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
                throw new ArgumentException("Short URL cannot be null or empty", nameof(shortUrl));

            try
            {
                return await GetFirstOrDefaultAsync(c => c.ShortURL == shortUrl);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving short URL async: {ShortUrl}", shortUrl);
                throw;
            }
        }

        public async Task<ShortURLModel> GetItemFromDataStoreByLongUrlAsync(string longUrl)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
                throw new ArgumentException("Long URL cannot be null or empty", nameof(longUrl));

            try
            {
                return await GetFirstOrDefaultAsync(c => c.LongURL == longUrl);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving by long URL async: {LongUrl}", longUrl);
                throw;
            }
        }

        public async Task<ShortURLModel> SaveItemToDataStoreAsync(ShortURLModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                await InsertAsync(model);
                await SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error saving short URL async: {ShortUrl} -> {LongUrl}", model.ShortURL, model.LongURL);
                return null;
            }
        }

        public async Task<bool> ShortUrlExistsAsync(string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
                return false;

            try
            {
                return await ExistsAsync(c => c.ShortURL == shortUrl);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error checking if short URL exists: {ShortUrl}", shortUrl);
                return false;
            }
        }

        public async Task<bool> LongUrlExistsAsync(string longUrl)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
                return false;

            try
            {
                return await ExistsAsync(c => c.LongURL == longUrl);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error checking if long URL exists: {LongUrl}", longUrl);
                return false;
            }
        }

        public async Task<int> GetTotalUrlCountAsync()
        {
            try
            {
                return await CountAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting total URL count");
                return 0;
            }
        }

        public async Task<ShortURLModel> UpdateItemAsync(ShortURLModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                await UpdateAsync(model);
                await SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating short URL: {ShortUrl} -> {LongUrl}", model.ShortURL, model.LongURL);
                return null;
            }
        }

        public async Task<bool> DeleteItemAsync(string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
                return false;

            try
            {
                var item = await GetItemFromDataStoreByShortUrlAsync(shortUrl);
                if (item != null)
                {
                    await DeleteAsync(item);
                    await SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting short URL: {ShortUrl}", shortUrl);
                return false;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await base.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error saving changes to database");
                throw;
            }
        }

        #endregion
    }
}
