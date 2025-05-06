using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSearch.Core.Services.Models
{
    public interface ICreateSearchRequest
    {
        public SearchRequest CreateSearchRequest();
    }
}
