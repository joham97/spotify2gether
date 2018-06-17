using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Models
{
    class Playback
    {
        public string timestamp { get; set; }
        public string progress_ms { get; set; }
        public string is_playing { get; set; }
        public string shuffle_state { get; set; }
        public string repeat_state { get; set; }
        public Device device { get; set; }
        public PlaybackContext context { get; set; }
        public Track item { get; set; }
    }
}
