using System.Collections.Generic;

namespace First_Bot.Controllers.API
{
    public class SearchResultsWrapper
    {
        //public IEnumerable<SearchDetailsDto> Results { get; set; }
        //public IEnumerable<string> AmbiguousLocations { get; set; }
        public string SearchUrl { get; set; }
        public int TotalResults { get; set; }
    }
}