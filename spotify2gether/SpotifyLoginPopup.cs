using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyApi
{
    public partial class SpotifyLoginPopup : Form
    {

        public delegate void AccessGranted(string code, string state);
        public event AccessGranted AccessGrantedEventHandler;
        
        public SpotifyLoginPopup(string client_id, string redirect, string scope)
        {
            InitializeComponent();
            scope = scope.Replace(" ", "%20");
            var path = $"https://accounts.spotify.com/authorize/?client_id={client_id}&response_type=code&redirect_uri={redirect}&scope={scope}&state=34fFs29kd09";
            this.Browser.Navigate(path);
        }

        private void Browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if(e.Url.AbsoluteUri.StartsWith("http://example.com"))
            {
                // Read Arguments
                var code = string.Empty;
                var state = string.Empty;
                var error = string.Empty;
                var args = e.Url.AbsoluteUri.Split('?')[1].Split('&');
                foreach (var arg in args)
                {
                    var key = arg.Split('=')[0];
                    var value = arg.Split('=')[1];
                    if (key.Equals("code"))
                        code = value;
                    else if (key.Equals("state"))
                        state = value;
                    else if (key.Equals("error"))
                        error = value;
                }
                if(error != string.Empty)
                    Console.WriteLine($"Error: {error}");
                else
                    AccessGrantedEventHandler(code, state);
                this.Close();
            }
        }
        
    }
}
