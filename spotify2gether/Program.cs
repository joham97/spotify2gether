using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyApi
{
    class Program
    {
        static SpotifyApi spotifyApi;

        static void Main(string[] args)
        {
            //Logout();
            LoadApi();

            Console.ReadLine();
        }

        static void Logout()
        {
            var t = new Thread(new ThreadStart(() =>
            {
                SpotifyLogout logout = new SpotifyLogout();
                logout.LoggedOutEventHandler += LoggedOut;
                logout.ShowDialog();
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        static void LoggedOut()
        {
            LoadApi()
        }

        static void LoadApi() {
            spotifyApi = new SpotifyApi();

            spotifyApi.UserTokenReceivedEventHandler += TokenReceived;
            spotifyApi.RequestUserToken();
        }

        static void TokenReceived()
        {
            // Result Playlist: spotify:user:1152600692:playlist:2HZrXwQ7A7bnPzewh79jak
            // Source Playlist: spotify:user:1152600692:playlist:1kC3P343lXm3q9hzSQtExY
            var tracks = spotifyApi.GetTracksOfPlaylist("sylke.hammerschmidt", "2oiQ7wKxe2uI1yeBcNy4WA");

            tracks.ForEach(track => {
                var shortName = track.name.Split(new char[] { '-', '(' })[0].Trim();
                var firstArtist = track.artists.First().name;
                var bestRemix = spotifyApi.SearchTrack($"{firstArtist} {shortName} Remix").FirstOrDefault();
                if (bestRemix != null)
                {
                    spotifyApi.AddTrackToPlaylist("sylke.hammerschmidt", "7ogOO4d5jEkadiKU8d3STw", bestRemix.id);
                    Thread.Sleep(150);
                }
            });
        }
    }
}
