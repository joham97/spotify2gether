using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Models
{
    class Playlist
    {

        public bool collaborative { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        [JsonProperty("public")]
        public string _public { get; set; }
        public string snapshot_id { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public User owner { get; set; }

    }
}
