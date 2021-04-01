using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleNaverMovieFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientID = "8MeTSKsiNxrmCj5d6GzL";
            string clientSecret = "sBaAlXPhN_";
            string search = "starwars";
            string openApiUrl = $"https://openapi.naver.com/v1/search/movie?query={search}";

            string responseJson = GetOpenApiResult(openApiUrl, clientID, clientSecret);
            JObject parsedJson = JObject.Parse(responseJson);

            int total = Convert.ToInt32( parsedJson["total"]);
            Console.WriteLine($"총 검색결과 : {total}");
            int display = Convert.ToInt32(parsedJson["display"]);
            Console.WriteLine($"화면출력 : {display}");

            JToken items = parsedJson["items"];
            JArray json_array = (JArray)items;

            foreach (var item in json_array)
            {
                Console.WriteLine($"{item["title"]}/{item["image"]}/{item["subtitle"]}/{item["actor"]}");
            }
        }

        private static string GetOpenApiResult(string openApiUrl, string clientID, string clientSecret)
        {
            throw new NotImplementedException();
        }
    }
}
