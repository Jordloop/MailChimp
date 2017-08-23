using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MailChimp.Models
{
    public class Signup
    {
        public string rel { get; set; }
        public string href { get; set; }

        public static Signup ShowSignup()
        {
            var client = new RestClient("https://us16.api.mailchimp.com/3.0/");
            var request = new RestRequest("lists?apikey=" + EnvironmentVariables.ApiKey, Method.GET);
            client.Authenticator = new HttpBasicAuthenticator(EnvironmentVariables.ApiKey, EnvironmentVariables.ListId);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            Console.WriteLine(jsonResponse["lists"][0]["_links"][14]);
            var signupLink = JsonConvert.DeserializeObject<Signup>(jsonResponse["lists"][0]["_links"][14].ToString());
            return signupLink;
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

