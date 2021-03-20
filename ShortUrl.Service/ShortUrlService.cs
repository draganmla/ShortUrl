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
        private IUriRepository shortUrlRepository;

        public ShortUrlServices(IUriRepository repository)
        {
            shortUrlRepository = repository;
        }

        public IEnumerable<ShortURLModel> GetCollectionFromDataStore()
        {
            return shortUrlRepository.GetCollectionFromDataStore();
        }

        public ShortURLModel GetItemFromDataStore(string shortUrl)
        {
            return shortUrlRepository.GetItemFromDataStoreByShortUrl(shortUrl);

        }

        public ShortUrlResponseModel SaveItemToDataStore(ShortURLRequestModel model)
        {
            ShortURLModel previouslySaved = shortUrlRepository.GetItemFromDataStoreByLongUrl(model.LongURL);
            if (previouslySaved != null)
            {
                return new ShortUrlResponseModel { Model = previouslySaved, Success = true, Message = "This url has been saved previously" };
            }
            else
            {
                ShortURLModel savedModel = shortUrlRepository.SaveItemToDataStore(ShortUrlModelMapper.MapRequestModelToDBModel(model));

                return new ShortUrlResponseModel
                {
                    Model = savedModel,
                    Success = true,
                    Message = "Saved successfully"
                };
            }

        }
    }
}
