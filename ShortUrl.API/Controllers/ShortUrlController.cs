using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShortUrl.Model;
using ShortUrl.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortUrl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortUrlController : ControllerBase
    {
        private IShortUrlService shortUrlService;
        private readonly ILogger<ShortUrlController> logger;
        public ShortUrlController(IShortUrlService shortUrlService, ILogger<ShortUrlController> logger)
        {
            this.shortUrlService = shortUrlService;
            this.logger = logger;
        }

        // GET: api/Default
        [HttpGet]
        [EnableCors("MyPolicy")]
        public IActionResult Get()
        {
            IEnumerable<ShortURLModel> shortUrls = shortUrlService.GetCollectionFromDataStore();
            return Ok(shortUrls);
        }

        [HttpGet("{shorturl}", Name = "Get")]
        [EnableCors("MyPolicy")]
        public IActionResult Get(string shorturl, [FromQuery(Name = "redirect")] bool redirect = true)
        {
            ShortURLModel shortUrl = shortUrlService.GetItemFromDataStore(shorturl);

            if (shortUrl != null)
            {
                if (redirect)
                {
                    return Redirect(shortUrl.LongURL);
                }
                else
                {
                    return Ok(shortUrl);
                }
            }

            return NotFound();
        }

        // POST: api/Default
        [HttpPost]
        [EnableCors("MyPolicy")]
        public IActionResult Post([FromBody] ShortURLRequestModel model)
        {
            if (ModelState.IsValid)
            {
                ShortUrlResponseModel result = shortUrlService.SaveItemToDataStore(model);
                if (result != null)
                    return Ok(result);
            }

            return BadRequest(ModelState.Values);
        }
    }
}
