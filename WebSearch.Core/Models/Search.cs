using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace WebSearch.Core.Models
{
    public class Search
    {
        public int SearchId { get; set; }
        public string Url { get; set; }
        public string KeyWords { get; set; }
        public string SearchEngine { get; set; }
        public DateTime Created { get; set; }
        public bool Daily { get; set; }
        public ICollection<SearchRun> SearchRuns { get; set; } = new List<SearchRun>();
    }
}
