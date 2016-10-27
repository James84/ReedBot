using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace First_Bot.Controllers.API
{
    public class Search
    {
        //public class Jobs
        //{
        //    public int TotalResults { get; set; }
        //}

        public static void GetJobs(string keywords)
        {
            GetJobs(keywords, null);
        }

        public static void GetJobs(string keywords, string location)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://www.reed.co.uk/api/1.0/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var formatApiKey = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", "36a0ff0b-d63e-40a0-b868-ffd958f6a41c", ""));
            var authData = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(formatApiKey));
            client.DefaultRequestHeaders.Authorization = authData;

            Task<HttpResponseMessage> response = client.GetAsync("search?keywords=" + keywords + "&location=" + location);
            HttpResponseMessage result = response.Result;

            //jobs = null;

            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync();
                Console.WriteLine(data);
                var jsonData = data.Result;
                Console.WriteLine(jsonData);

                //jobs.TotalResults = jsonData;
            }
        }
    }
}