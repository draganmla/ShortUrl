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

        // Improved async implementations using proper async repository methods
        public async Task<IEnumerable<ShortURLModel>> GetCollectionFromDataStoreAsync()
        {
            return await _shortUrlRepository.GetCollectionFromDataStoreAsync();
        }

        public async Task<ShortURLModel> GetItemFromDataStoreAsync(string shortUrl)
        {
            return await _shortUrlRepository.GetItemFromDataStoreByShortUrlAsync(shortUrl);
        }

        public async Task<ShortUrlResponseModel> SaveItemToDataStoreAsync(ShortURLRequestModel model)
        {
            // Check if URL already exists using async method
            ShortURLModel previouslySaved = await _shortUrlRepository.GetItemFromDataStoreByLongUrlAsync(model.LongURL);
            if (previouslySaved != null)
            {
                return new ShortUrlResponseModel 
                { 
                    Model = previouslySaved, 
                    Success = true, 
                    Message = "This url has been saved previously" 
                };
            }

            // Save new URL using async method
            ShortURLModel modelToSave = ShortUrlModelMapper.MapRequestModelToDBModel(model);
            ShortURLModel savedModel = await _shortUrlRepository.SaveItemToDataStoreAsync(modelToSave);

            if (savedModel != null)
            {
                return new ShortUrlResponseModel
                {
                    Model = savedModel,
                    Success = true,
                    Message = "Saved successfully"
                };
            }

            return new ShortUrlResponseModel
            {
                Model = null,
                Success = false,
                Message = "Failed to save URL"
            };
        }
    }
}
