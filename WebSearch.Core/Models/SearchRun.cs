using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSearch.Core.Models
{
    public class SearchRun
    {
        public int SearchRunId { get; set; }

        public int SearchId { get; set; }

        public string Result { get; set; }

        public DateTime Created { get; set; }
    }
}
