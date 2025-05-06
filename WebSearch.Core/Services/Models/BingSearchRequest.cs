using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using System.Text.RegularExpressions;

namespace WebSearch.Core.Services.Models
{
    public class BingSearchRequest : SearchRequest
    {

        public BingSearchRequest()
        {
            this.EngineUrl = "https://www.bing.com/search?q=";
            this.RegExForMatch = @"<a href=""(?<url>[^""]+)"" h=""[^""]+""[^>]*>(?<title>[^<]+)</a>";
            this.MaxPages = 10; // Set the maximum number of pages to search
        }
        public override async Task<string> Search(string keyWords)
        {
            string consolidatedResults = "";
            string currentResult = "";
            // Loop through the pages and concatenate the results
            for (int i = 0; i < this.MaxPages*10; i+=10)
            {
                currentResult = await this.EngineUrl
                    .WithAutoRedirect(true)
                    .SetQueryParams(new
                                    {
                                        q = keyWords,
                                        first = i 
                                    })
                                    .GetStringAsync();
                consolidatedResults += currentResult;
            }
            return consolidatedResults;
        }

        public override async Task<string> ParseResult(string result, string url)
        {
            int[] positions = new int[100];
            int positionIndex = 0;

            // Find all matches using the regular expression
            var matches = Regex.Matches(result, this.RegExForMatch);

            int rank = 1; // Start ranking from 1
            foreach (Match match in matches)
            {
                // Check if the match contains the URL
                if (match.Groups[0].Value.Contains(url))
                {
                    positions[positionIndex] = rank;
                    positionIndex++;
                }
                rank++;
            }

            // Convert the array to a comma-separated string, ignoring empty values
            return string.Join(",", positions.Where(p => p != 0));
        }
    }
}
