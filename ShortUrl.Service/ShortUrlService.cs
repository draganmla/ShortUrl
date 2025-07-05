using ShortUrl.Model;
using ShortUrl.Repository.Abstract;
using ShortUrl.Service.Abstract;
using ShortUrl.Service.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortUrl.Service
{
    public class ShortUrlServices : IShortUrlService
    {
        private readonly IUriRepository _shortUrlRepository;

        public ShortUrlServices(IUriRepository repository)
        {
            _shortUrlRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<ShortURLModel> GetCollectionFromDataStore()
        {
            return _shortUrlRepository.GetCollectionFromDataStore();
        }

        public ShortURLModel GetItemFromDataStore(string shortUrl)
        {
            return _shortUrlRepository.GetItemFromDataStoreByShortUrl(shortUrl);
        }

        public ShortUrlResponseModel SaveItemToDataStore(ShortURLRequestModel model)
        {
            ShortURLModel previouslySaved = _shortUrlRepository.GetItemFromDataStoreByLongUrl(model.LongURL);
            if (previouslySaved != null)
            {
                return new ShortUrlResponseModel { Model = previouslySaved, Success = true, Message = "This url has been saved previously" };
            }
            else
            {
                ShortURLModel savedModel = _shortUrlRepository.SaveItemToDataStore(ShortUrlModelMapper.MapRequestModelToDBModel(model));

                return new ShortUrlResponseModel
                {
                    Model = savedModel,
                    Success = true,
                    Message = "Saved successfully"
                };
            }
        }

        // Async implementations for better performance
        public async Task<IEnumerable<ShortURLModel>> GetCollectionFromDataStoreAsync()
        {
            return await Task.FromResult(_shortUrlRepository.GetCollectionFromDataStore());
        }

        public async Task<ShortURLModel> GetItemFromDataStoreAsync(string shortUrl)
        {
            return await Task.FromResult(_shortUrlRepository.GetItemFromDataStoreByShortUrl(shortUrl));
        }

        public async Task<ShortUrlResponseModel> SaveItemToDataStoreAsync(ShortURLRequestModel model)
        {
            return await Task.FromResult(SaveItemToDataStore(model));
        }
    }
}
