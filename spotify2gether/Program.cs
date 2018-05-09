using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotify2gether
{
    class Program
    {
        static SpotifyApi spotifyApi;

        static void Main(string[] args)
        {
            spotifyApi = new SpotifyApi();

            spotifyApi.UserTokenReceivedEventHandler += TokenReceived;
            spotifyApi.RequestUserToken();

            Console.ReadLine();
        }

        static void TokenReceived()
        {
            // Testing Endpoints
            Console.WriteLine();
        }
    }
}
