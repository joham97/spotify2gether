using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SpotifyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyApi
{
    class SpotifyApi
    {

        private const string CLIENT_ID = "80e24d7d78304b8ebba8daab8296ec05";
        private const string CLIENT_SECRET = "bcb50ada7e6d48cda3f0131354fdfd37";

        private const string REDIRECT = "http%3A%2F%2Fexample.com";

        private const string SCOPES = "user-read-private user-read-email user-modify-playback-state user-read-playback-state user-read-currently-playing playlist-modify-public playlist-modify-private";
        
        private RestClient apiClient, accountsClient;

        public AccessToken AccessToken { get; set; }
        public AccessAndRefreshToken AccessAndRefreshToken { get; set; }
        public string UserAuthorizationCode { get; set; }

        public delegate void UserTokenReceived();
        public event UserTokenReceived UserTokenReceivedEventHandler;

        public SpotifyApi()
        {
            accountsClient = new RestClient("https://accounts.spotify.com/api");
            apiClient = new RestClient("https://api.spotify.com/v1");
        }

        #region Authorization
        public void RequestAccessToken()
        {
            var request = new RestRequest("/token", Method.POST);

            string auth = "Basic " + Base64.Base64Encode($"{CLIENT_ID}:{CLIENT_SECRET}");
            request.AddHeader("Authorization", auth);
            string contenttype = "application/x-www-form-urlencoded";
            request.AddHeader("Content-Type", contenttype);

            string grant_type = "client_credentials";
            request.AddParameter(contenttype, $"grant_type={grant_type}", ParameterType.RequestBody);

            // execute the request
            IRestResponse response = accountsClient.Execute(request);
            var content = response.Content;

            this.AccessToken = JsonConvert.DeserializeObject<AccessToken>(content);
        }

        public void RequestUserToken()
        {
            var t = new Thread(new ThreadStart(() =>
            {
                var popup = new SpotifyLoginPopup(CLIENT_ID, REDIRECT, SCOPES);
                popup.AccessGrantedEventHandler += AccessGranted;
                popup.ShowDialog();
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void AccessGranted(string code, string state)
        {
            this.UserAuthorizationCode = code;
            RequestAccessAndRefreshTokens();
            UserTokenReceivedEventHandler();
        }

        private void RequestAccessAndRefreshTokens()
        {
            var request = new RestRequest("/token", Method.POST);

            string auth = "Basic " + Base64.Base64Encode($"{CLIENT_ID}:{CLIENT_SECRET}");
            request.AddHeader("Authorization", auth);

            string contenttype = "application/x-www-form-urlencoded";
            request.AddHeader("Content-Type", contenttype);

            string grant_type = "authorization_code";
            request.AddParameter(contenttype,
                $"grant_type={grant_type}&" +
                $"code={UserAuthorizationCode}&" +
                $"redirect_uri={REDIRECT}", ParameterType.RequestBody);

            // execute the request
            IRestResponse response = accountsClient.Execute(request);
            var content = response.Content;

            this.AccessAndRefreshToken = JsonConvert.DeserializeObject<AccessAndRefreshToken>(content);
        }

        public void RefreshAccessTokens()
        {
            var refresh_token = AccessAndRefreshToken.refresh_token;
            var request = new RestRequest("/token", Method.POST);

            string auth = "Basic " + Base64.Base64Encode($"{CLIENT_ID}:{CLIENT_SECRET}");
            request.AddHeader("Authorization", auth);

            string contenttype = "application/x-www-form-urlencoded";
            request.AddHeader("Content-Type", contenttype);

            string grant_type = "refresh_token";
            request.AddParameter(contenttype,
                $"grant_type={grant_type}&" +
                $"refresh_token={refresh_token}", ParameterType.RequestBody);

            // execute the request
            IRestResponse response = accountsClient.Execute(request);
            var content = response.Content;

            this.AccessAndRefreshToken = JsonConvert.DeserializeObject<AccessAndRefreshToken>(content);
            this.AccessAndRefreshToken.refresh_token = refresh_token;
        }
        #endregion

        #region User
        public User GetMe()
        {
            var request = new RestRequest("/me", Method.GET);
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            IRestResponse response = apiClient.Execute(request);
            return JsonConvert.DeserializeObject<User>(response.Content);
        }
        #endregion

        #region Player
        public Playback CurrentPlayback()
        {
            var request = new RestRequest("/me/player", Method.GET);
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            IRestResponse response = apiClient.Execute(request);
            return JsonConvert.DeserializeObject<Playback>(response.Content);
        }
        public void SkipToNextTrack()
        {
            var request = new RestRequest("/me/player/next", Method.POST);
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            apiClient.Execute(request);
        }
        public void SeekToPosition(string position_ms)
        {
            var request = new RestRequest($"/me/player/seek?position_ms={position_ms}", Method.PUT);
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            apiClient.Execute(request);
        }
        public IList<Device> GetDevices()
        {
            var request = new RestRequest($"/me/player/devices", Method.GET);
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            IRestResponse response = apiClient.Execute(request);
            return JsonConvert.DeserializeObject<Devices>(response.Content).devices;
        }
        #endregion
        
        #region Playlists
        public IList<Playlist> GetPlaylists(string id)
        {
            var request = new RestRequest($"/users/{id}/playlists", Method.GET);
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            IRestResponse response = apiClient.Execute(request);
            return JsonConvert.DeserializeObject<Playlists>(response.Content).items;
        }
        public List<Track> GetTracksOfPlaylist(string user_id, string playlist_id)
        {
            var request = new RestRequest($"/users/{user_id}/playlists/{playlist_id}/tracks", Method.GET);
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            IRestResponse response = apiClient.Execute(request);

            var obj = new { items = new List<TrackWrapper>() };

            var items = JsonConvert.DeserializeAnonymousType(response.Content, obj).items;

            var tracks = new List<Track>();
            items.ForEach(tw => tracks.Add(tw.track));

            return tracks;
        }

        public void AddTrackToPlaylist(string user_id, string playlist_id, string track_id)
        {
            var request = new RestRequest($"/users/{user_id}/playlists/{playlist_id}/tracks?uris=spotify%3Atrack%3A{track_id}", Method.POST);
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            IRestResponse response = apiClient.Execute(request);
            Console.WriteLine(response.IsSuccessful);
        }
        #endregion

        #region Search
        public IList<Track> SearchTrack(string track)
        {
            var request = new RestRequest($"/search", Method.GET);
            request.AddParameter("q", track);
            request.AddParameter("type", "track");
            request.AddParameter("market", "DE");
            request.AddHeader("Authorization", $"Bearer {AccessAndRefreshToken.access_token}");

            // execute the request
            IRestResponse response = apiClient.Execute(request);

            var obj = new { tracks = new { items = new List<Track>() } };

            return JsonConvert.DeserializeAnonymousType(response.Content, obj).tracks.items;
        }
        #endregion

    }
}
