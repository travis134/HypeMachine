using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Phone.Info;
using System.Text;

namespace HypeMachine
{
    public partial class MainPage : PhoneApplicationPage
    {

        public MainPage()
        {
            InitializeComponent();

            object tempId;
            DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out tempId);
            byte[] phoneInfo = (byte[])tempId;

            System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(phoneInfo));

            /*
            for (int i = 0; i < phoneInfo.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine(phoneInfo[i].ToString());
            }
            */

            List<Game> games = new List<Game>();

            String url = "http://www.gamestop.com/SyndicationHandler.ashx?Filter=BestSellers&platform=xbox360";

            WebClient gamestopClient = new WebClient();
            gamestopClient.OpenReadAsync(new Uri(url));
            gamestopClient.OpenReadCompleted += new OpenReadCompletedEventHandler(
                delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    if (e.Error == null)
                    {
                        games = parseRSS(e.Result);
                    }
                });
        }

        public List<Game> parseRSS(System.IO.Stream stream)
        {
            XDocument xdoc = XDocument.Load(stream);
            List<Game> games = (from item in xdoc.Descendants("item")
                          select new Game()
                          {
                              GuidUri = new Uri(item.Element("guid").Value),
                              Link = new Uri(item.Element("link").Value),
                              Category = item.Element("category").Value,
                              Title = item.Element("title").Value,
                              Description = item.Element("description").Value,
                              PubDate = DateTime.Parse(item.Element("pubDate").Value)
                          }).ToList();

            foreach (Game game in games)
            {
                //System.Diagnostics.Debug.WriteLine(game.ToString());
            }

            return games;        
        }

    }
}