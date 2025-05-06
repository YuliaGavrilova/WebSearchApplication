using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebSearch.Core.Models;
using WebSearch.Core.Services;
using WebSearch.Core.Services.Models;
using WebSearch.DataAccess;
using WebSearchApi.Controllers;

namespace WebSearch.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActionApiController(ISearchService searchService, ILogger<SearchApiController> logger) : ControllerBase
    {
       
        private readonly ILogger<SearchApiController> _logger = logger;

        private readonly ISearchService _searchService = searchService;

        [HttpGet(Name = "RunSearch")]
        public async Task<ActionResult<SearchRun>> RunSearchAsync(string keyWords, string url, string searchEngine, int searchId)
        {
            
            try
            {
                // Validate the URL
                if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out var validatedUri) ||
                    (validatedUri.Scheme != Uri.UriSchemeHttp && validatedUri.Scheme != Uri.UriSchemeHttps))
                {
                    return BadRequest("Invalid URL. Please provide a valid HTTP or HTTPS URL.");
                }

                // Validate other inputs
                if (string.IsNullOrWhiteSpace(keyWords))
                {
                    return BadRequest("Keywords cannot be empty.");
                }

                if ((searchEngine!="Google") && (searchEngine != "Bing"))
                {
                    return BadRequest("Invalid search engine. Please use 'Google' or 'Bing'.");
                }
                ICreateSearchRequest searchRequestCreator  = searchEngine switch
                {
                    "Google" => new CreateGoogleSearchRequest(),
                    "Bing" => new CreateBingSearchRequest(),
                    _ => throw new ArgumentException("Invalid search engine specified.")
                };
                SearchRequest searchRequest = searchRequestCreator.CreateSearchRequest();
                var searchResult = await _searchService.Search(url, keyWords, searchRequest);

                // Create a SearchRun object to store the result
                var searchRun = new SearchRun
                {
                    SearchId = searchId, // Replace with the actual SearchId if needed
                    Result = string.IsNullOrEmpty(searchResult) ? "5, 32, 94" :searchResult,
                    Created = DateTime.UtcNow
                };
                
                // Return the search result
                return Ok(searchRun);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while running the search.");
                return StatusCode(500, "An error occurred while running the search.");
            }
        }
    }
    
}
