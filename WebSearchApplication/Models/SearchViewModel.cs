using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebSearch.Shared;
using static WebSearch.Shared.Enums;
namespace WebSearch.Web.Models

{
    public class SearchViewModel
    {
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// to be used in search
        /// </summary>
        [Required]
        public string KeyWords { get; set; }
        /// <summary>
        /// the selected search option
        /// </summary>
        
        public string SearchEngineSelected { get; set; }
        /// <summary>
        /// All options for search engines to choose from
        /// </summary>
        public List<SelectListItem> SearchEngineOptions { get; set; }
        
        public List<SearchResultViewModel> SearchResults { get; set; }
        
        public SearchViewModel()
        {
            SearchEngineSelected = String.Empty;
            SearchEngineOptions = Utilities.GetEnumOptions<SearchEngine>(true);
        }
    }
}
