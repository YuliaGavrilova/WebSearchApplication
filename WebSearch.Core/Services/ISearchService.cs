using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSearch.Core.Services.Models;

namespace WebSearch.Core.Services
{
    public interface ISearchService
    {
        public Task<string> Search(string url, string keywords, SearchRequest searchRequest);
    }
}
