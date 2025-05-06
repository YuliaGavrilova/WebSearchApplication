using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSearch.Core.Services.Models
{
    public abstract class SearchRequest
    {
        public string EngineUrl { get; set; }
        public string RegExForMatch { get; set; }
        public int MaxPages { get; set; }

        public abstract Task<string> Search(string url);
        public abstract Task<string> ParseResult(string searchResult, string url);
    }
}
