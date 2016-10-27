using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace First_Bot.Controllers.API
{
    public class Search
    {
        public static SearchResultsWrapper GetJobs(string keywords)
        {
            return GetJobs(keywords, null);
        }

        public static SearchResultsWrapper GetJobs(string keywords, string location)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://www.reed.co.uk/api/1.0/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var formatApiKey = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", "36a0ff0b-d63e-40a0-b868-ffd958f6a41c", ""));
            var authData = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(formatApiKey));
            client.DefaultRequestHeaders.Authorization = authData;

            Task<HttpResponseMessage> response = client.GetAsync("search?keywords=" + keywords + "&locationname=" + location);
            HttpResponseMessage result = response.Result;

            SearchResultsWrapper jobs = null;

            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync();
                Console.WriteLine(data);
                jobs = JsonConvert.DeserializeObject<SearchResultsWrapper>(data.Result);
                Console.WriteLine(jobs);
            }

            var sanitizedKeywords = keywords.Replace(" ", "+");
            jobs.SearchUrl = "http://www.reed.co.uk/jobs?keywords="+ sanitizedKeywords + "&location=" + location;

            return jobs;
        }
    }
}