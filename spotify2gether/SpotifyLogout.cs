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
    public partial class SpotifyLogout : Form
    {

        public delegate void LoggedOut();
        public event LoggedOut LoggedOutEventHandler;
        
        public SpotifyLogout()
        {
            InitializeComponent();
            var path = $"https://spotify.com/logout";
            this.Browser.Navigate(path);
        }

        private void Browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            LoggedOutEventHandler();
            this.Close();
        }
        
    }
}
