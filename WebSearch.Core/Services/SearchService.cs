using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSearch.Core.Services.Models;


namespace WebSearch.Core.Services
{
    public class SearchService : ISearchService
    {
        
        public async Task<string> Search(string url, string keywords, SearchRequest request)
        {

            string  result = await request.Search(keywords);
            return await request.ParseResult(result,url);
        }
        
    }
}
