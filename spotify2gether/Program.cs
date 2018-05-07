using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotify2gether
{
    class Program
    {
        static void Main(string[] args)
        {
            SpotifyApi spotifyApi = new SpotifyApi();

            spotifyApi.RequestAccessToken();
            Console.WriteLine(spotifyApi.AccessToken);
            
            Console.ReadLine();
        }
    }
}
