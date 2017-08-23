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
        //public string href { get; set; }

        public static List<Games> FindGame(string userString)
        {
            var client = new RestClient("https://api-2445582011268.apicast.io/");
            var request = new RestRequest("games/?search=" + userString, Method.GET);
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
            var gameInfo = JsonConvert.DeserializeObject<List<Games>>(jsonResponse.ToString());
            return gameInfo;
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
