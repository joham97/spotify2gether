using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Models
{
    class Device
    {
        public string id { get; set; }
        public string is_active { get; set; }
        public string is_restricted { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string volume_percent { get; set; }
    }
}
