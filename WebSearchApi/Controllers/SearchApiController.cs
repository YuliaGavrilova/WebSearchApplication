using Microsoft.AspNetCore.Mvc;
using WebSearch.Core.Models;
using WebSearch.DataAccess;

namespace WebSearchApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchApiController : ControllerBase
    {
        private readonly WebSearchDbContext _dbContext;

        private readonly ILogger<SearchApiController> _logger;

        public SearchApiController(WebSearchDbContext dbContext, ILogger<SearchApiController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetAllSearches")]
        public ActionResult<IEnumerable<Search>> GetAllSearches()
        {
            try
            {
                var searches = _dbContext.Searches.ToList();
                return Ok(searches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving searches.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{searchId}/runs", Name = "AddSearchRun")]
        public ActionResult AddSearchRun(int searchId, [FromBody] SearchRun searchRun)
        {
            try
            {
                // Check if the Search with the given ID exists
                var search = _dbContext.Searches.Find(searchId);
                if (search == null)
                {
                    return NotFound($"Search with ID {searchId} not found.");
                }

                // Set the SearchId for the SearchRun
                searchRun.SearchId = searchId;
                searchRun.Created = DateTime.UtcNow; // Set the Created timestamp

                // Add the SearchRun to the database
                _dbContext.SearchRuns.Add(searchRun);
                _dbContext.SaveChanges();

                // Return the created SearchRun
                return CreatedAtRoute("GetSearchById", new { id = searchId }, searchRun);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the search run.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{searchId}/runs", Name = "GetSearchRunsBySearchId")]
        public ActionResult<IEnumerable<SearchRun>> GetSearchRunsBySearchId(int searchId)
        {
            try
            {
                // Check if the Search with the given ID exists
                var search = _dbContext.Searches.Find(searchId);
                if (search == null)
                {
                    return NotFound($"Search with ID {searchId} not found.");
                }

                // Retrieve all SearchRun records for the given SearchId
                var searchRuns = _dbContext.SearchRuns
                    .Where(sr => sr.SearchId == searchId)
                    .ToList();

                return Ok(searchRuns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving search runs.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost(Name = "CreateSearch")]
        public ActionResult<int> CreateOrGetSearch([FromBody] Search search)
        {
            try
            {
                // Check if the search already exists in the database
                var existingSearch = _dbContext.Searches
                    .FirstOrDefault(s => s.Url == search.Url
                                      && s.KeyWords == search.KeyWords
                                      && s.SearchEngine == search.SearchEngine);

                if (existingSearch != null)
                {
                    // If the search exists, return its ID
                    return Ok(existingSearch.SearchId);
                }

                // If the search doesn't exist, create a new one
                search.Created = DateTime.UtcNow; // Set the Created timestamp
                _dbContext.Searches.Add(search);
                _dbContext.SaveChanges();

                // Return the ID of the newly created search
                return CreatedAtRoute("GetSearchById", new { id = search.SearchId }, search.SearchId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating or retrieving the search.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "GetSearchById")]
        public ActionResult<Search> GetSearchById(int id)
        {
            try
            {
                var search = _dbContext.Searches.Find(id);
                if (search == null)
                {
                    return NotFound($"Search with ID {id} not found.");
                }

                return Ok(search);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the search.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
