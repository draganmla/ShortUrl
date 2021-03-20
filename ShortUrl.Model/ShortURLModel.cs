using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortUrl.Model
{
    public class ShortURLModel
    {
        public int Id { get; set; }
        public string ShortURL { get; set; }
        public string LongURL { get; set; }

    }
}
