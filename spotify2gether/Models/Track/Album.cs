using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Models
{
    class Album
    {
        public string album_type { get; set; }
        public string href { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public IList<string> available_markets { get; set; }
        public IList<Artist> artists { get; set; }
    }
}
