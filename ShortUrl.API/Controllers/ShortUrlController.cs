﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IShortUrlService _shortUrlService;
        private readonly ILogger<ShortUrlController> _logger;
        private readonly IMemoryCache _cache;
        
        // Cache settings
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan ShortCacheDuration = TimeSpan.FromMinutes(5);

        public ShortUrlController(
            IShortUrlService shortUrlService, 
            ILogger<ShortUrlController> logger,
            IMemoryCache cache)
        {
            _shortUrlService = shortUrlService ?? throw new ArgumentNullException(nameof(shortUrlService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        // GET: api/ShortUrl
        [HttpGet]
        [EnableCors("MyPolicy")]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "*" })] // 5 minute cache
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                const string cacheKey = "all_short_urls";
                
                if (_cache.TryGetValue(cacheKey, out IEnumerable<ShortURLModel> cachedUrls))
                {
                    _logger.LogInformation("Retrieved {Count} URLs from cache", cachedUrls.Count());
                    return Ok(cachedUrls);
                }

                var shortUrls = await _shortUrlService.GetCollectionFromDataStoreAsync();
                
                // Cache for 5 minutes since this list changes frequently
                _cache.Set(cacheKey, shortUrls, ShortCacheDuration);
                
                _logger.LogInformation("Retrieved and cached {Count} URLs from database", shortUrls.Count());
                return Ok(shortUrls);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving short URLs collection");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "An error occurred while retrieving URLs" });
            }
        }

        [HttpGet("{shorturl}", Name = "Get")]
        [EnableCors("MyPolicy")]
        [ResponseCache(Duration = 1800, VaryByQueryKeys = new[] { "shorturl", "redirect" })] // 30 minute cache
        public async Task<IActionResult> GetAsync(string shorturl, [FromQuery(Name = "redirect")] bool redirect = true)
        {
            if (string.IsNullOrWhiteSpace(shorturl))
            {
                return BadRequest(new { error = "Short URL parameter is required" });
            }

            try
            {
                var cacheKey = $"shorturl_{shorturl}";
                
                if (!_cache.TryGetValue(cacheKey, out ShortURLModel shortUrlModel))
                {
                    shortUrlModel = await _shortUrlService.GetItemFromDataStoreAsync(shorturl);
                    
                    if (shortUrlModel != null)
                    {
                        // Cache successful lookups for longer duration
                        _cache.Set(cacheKey, shortUrlModel, CacheDuration);
                        _logger.LogInformation("Cached short URL lookup for: {ShortUrl}", shorturl);
                    }
                    else
                    {
                        // Cache negative results for shorter duration to handle eventual consistency
                        _cache.Set(cacheKey, (ShortURLModel)null, TimeSpan.FromMinutes(1));
                    }
                }
                else
                {
                    _logger.LogInformation("Retrieved short URL from cache: {ShortUrl}", shorturl);
                }

                if (shortUrlModel == null)
                {
                    _logger.LogWarning("Short URL not found: {ShortUrl}", shorturl);
                    return NotFound(new { error = $"Short URL '{shorturl}' not found" });
                }

                // Log analytics (consider moving to background service)
                _logger.LogInformation("Short URL accessed: {ShortUrl} -> {LongUrl}", shorturl, shortUrlModel.LongURL);

                if (redirect)
                {
                    // Use 301 (Permanent Redirect) for SEO benefits if URL mappings are permanent
                    // Use 302 (Temporary Redirect) if mappings might change
                    return Redirect(shortUrlModel.LongURL);
                }

                return Ok(shortUrlModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving short URL: {ShortUrl}", shorturl);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "An error occurred while retrieving the URL" });
            }
        }

        // POST: api/ShortUrl
        [HttpPost]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> PostAsync([FromBody] ShortURLRequestModel model)
        {
            if (model == null)
            {
                return BadRequest(new { error = "Request body is required" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) });
                
                return BadRequest(new { error = "Validation failed", details = errors });
            }

            // Enhanced URL validation
            if (string.IsNullOrWhiteSpace(model.LongURL))
            {
                return BadRequest(new { error = "URL is required" });
            }

            if (!Uri.TryCreate(model.LongURL, UriKind.Absolute, out var uri) || 
                (!uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) && 
                 !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { error = "Please provide a valid HTTP or HTTPS URL" });
            }

            try
            {
                var result = await _shortUrlService.SaveItemToDataStoreAsync(model);
                
                if (result == null || !result.Success)
                {
                    _logger.LogWarning("Failed to create short URL for: {LongUrl}", model.LongURL);
                    return BadRequest(new { error = result?.Message ?? "Failed to create short URL" });
                }

                // Invalidate cache entries that might be affected
                InvalidateRelatedCache(result.Model);

                _logger.LogInformation("Successfully created short URL: {ShortUrl} for {LongUrl}", 
                    result.Model.ShortURL, result.Model.LongURL);

                // Return 201 Created with location header
                return CreatedAtRoute("Get", 
                    new { shorturl = result.Model.ShortURL }, 
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating short URL for: {LongUrl}", model.LongURL);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "An error occurred while creating the short URL" });
            }
        }

        /// <summary>
        /// Health check endpoint for monitoring
        /// </summary>
        [HttpGet("health")]
        [EnableCors("MyPolicy")]
        [ResponseCache(Duration = 60)] // Cache health check for 1 minute
        public IActionResult HealthCheck()
        {
            try
            {
                return Ok(new { 
                    status = "healthy", 
                    timestamp = DateTime.UtcNow,
                    version = "1.0.0"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, 
                    new { status = "unhealthy", error = ex.Message });
            }
        }

        /// <summary>
        /// Get basic statistics about the service
        /// </summary>
        [HttpGet("stats")]
        [EnableCors("MyPolicy")]
        [ResponseCache(Duration = 3600)] // Cache stats for 1 hour
        public async Task<IActionResult> GetStatsAsync()
        {
            try
            {
                const string statsCacheKey = "service_stats";
                
                if (_cache.TryGetValue(statsCacheKey, out object cachedStats))
                {
                    return Ok(cachedStats);
                }

                var urls = await _shortUrlService.GetCollectionFromDataStoreAsync();
                var stats = new
                {
                    totalUrls = urls.Count(),
                    cacheHitRatio = "N/A", // Would need more sophisticated tracking
                    lastUpdated = DateTime.UtcNow
                };

                _cache.Set(statsCacheKey, stats, TimeSpan.FromMinutes(30));
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service statistics");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "An error occurred while retrieving statistics" });
            }
        }

        /// <summary>
        /// Invalidate cache entries related to the given model
        /// </summary>
        private void InvalidateRelatedCache(ShortURLModel model)
        {
            if (model?.ShortURL != null)
            {
                _cache.Remove($"shorturl_{model.ShortURL}");
            }
            _cache.Remove("all_short_urls");
            _cache.Remove("service_stats");
        }
    }
}
