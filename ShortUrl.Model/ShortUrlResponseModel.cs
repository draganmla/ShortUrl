using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortUrl.Model
{
    public class ShortUrlResponseModel
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public ShortURLModel Model { get; set; }
    }
}
