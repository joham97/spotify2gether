using ConsoleApp1.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotify2gether
{
    class SpotifyApi
    {

        public AccessToken AccessToken { get; set; }

        private const string CLIENT_ID = "***";
        private const string CLIENT_SECRET = "***";

        public SpotifyApi()
        {

        }

        public void RequestAccessToken()
        {
            string path = "https://accounts.spotify.com/api";
            string subpath = "/token";
            string grant_type = "client_credentials";
            string contenttype = "application/x-www-form-urlencoded";
            string auth = "Basic " + Base64.Base64Encode($"{CLIENT_ID}:{CLIENT_SECRET}");

            var client = new RestClient(path);
            var request = new RestRequest(subpath, Method.POST);
            
            request.AddHeader("Authorization", auth);
            request.AddHeader("Content-Type", contenttype);

            request.AddParameter("application/x-www-form-urlencoded", $"grant_type={grant_type}", ParameterType.RequestBody);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            this.AccessToken = JsonConvert.DeserializeObject<AccessToken>(content);
        }

    }
}
