using ShortUrl.Model;
using ShortUrl.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortUrl.Service.Map
{
    public static class ShortUrlModelMapper
    {
        public static ShortURLModel MapRequestModelToDBModel(ShortURLRequestModel requestModel)
        {
            ShortURLModel result = new ShortURLModel
            {
                LongURL = requestModel.LongURL
            };

            result.ShortURL = TokenGenerator.GenerateShortUrl();

            return result;
        }
    }
}
