using static WebSearch.Shared.Enums;
using System.Text.Json.Serialization;

namespace WebSearch.Web.Models
{
    public class SearchResultViewModel
    {
            public int SearchId { get; set; }
            public string Result { get; set; }
            public DateTimeOffset Created{ get; set; }
    }
}