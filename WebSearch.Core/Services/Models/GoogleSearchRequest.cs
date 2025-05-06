using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
namespace WebSearch.Core.Services.Models
{
    public class GoogleSearchRequest : SearchRequest
    {
        public GoogleSearchRequest()
        {
            this.EngineUrl = "https://www.google.com/search";
            this.RegExForMatch = @"<a href=""(?<url>[^""]+)"" h=""[^""]+""[^>]*>(?<title>[^<]+)</a>";
        }

        public override async Task<string> Search(string keyWords)
        {
            return await this.EngineUrl
                .WithAutoRedirect(true)
                .SetQueryParams(new
                {
                    q = keyWords,
                    num = 100 // Number of results to return
                })
                .GetStringAsync();
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
