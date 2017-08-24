using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MailChimp.Models
{
    public class Games
    {
        public string id { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
        public string rating { get; set; }
        public virtual List<screenshot> screenshots { get; set; }
      
      
        public static List<Games> FindGame(string userString)
        {     
            //Who we are requesting from       
            var client = new RestClient("https://api-2445582011268.apicast.io/");
            //What we are requesting
            var request = new RestRequest("games/?search=" + userString, Method.GET);
            //API "Username" and "Password"
            client.Authenticator = new HttpBasicAuthenticator("user-key", EnvironmentVariables.UserKey);
            //API call Header
            request.AddHeader("user-key", EnvironmentVariables.UserKey);
            //Response from API call
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(response.Content);

            var gameInfo = JsonConvert.DeserializeObject<List<Games>>(jsonResponse.ToString());

            List<Games> searchResults = new List<Games>();
            for(int i = 0; i < gameInfo.Count; i++)
            {
                searchResults.Add(gameInfo[i]);
            }
            return gameInfo;
        }

        public static List<Games> DecodeFindGame(List<Games> idArray)
        {
            var client = new RestClient("https://api-2445582011268.apicast.io/");
            Console.WriteLine(idArray);
            List<string> spot = new List<string>();
            foreach(var game in idArray)
            {
                spot.Add(game.id);
            }
            Console.WriteLine(spot.ToString());
            var request = new RestRequest("/games/"+string.Join(",", spot.ToArray())+"?fields=*" , Method.GET);
            Console.WriteLine(spot);
            client.Authenticator = new HttpBasicAuthenticator("user-key", EnvironmentVariables.UserKey);

            request.AddHeader("user-key", EnvironmentVariables.UserKey);
            //request.AddHeader("Accept", "application/json");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(response.Content);
            Console.WriteLine(jsonResponse);
            var decodedGameInfo = JsonConvert.DeserializeObject<List<Games>>(jsonResponse.ToString());
            return decodedGameInfo;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
