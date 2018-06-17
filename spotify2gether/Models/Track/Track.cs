using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Models
{
    class Track
    {
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        [JsonProperty("explicit")]
        public bool _explicit { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public IList<string> available_markets { get; set; }
        public IList<Artist> artists { get; set; }
        public Album album { get; set; }
    }
}
