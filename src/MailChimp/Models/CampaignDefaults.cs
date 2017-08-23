using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MailChimp.Models
{
    public class CampaignDefaults
    {
        public string from_name { get; set; }
        public string from_email { get; set; }
        public string subject { get; set; }

        public static CampaignDefaults GetCampaigns()
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
            var messageList = JsonConvert.DeserializeObject<CampaignDefaults>(jsonResponse["lists"][0]["campaign_defaults"].ToString());
            return messageList;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}




