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
using System.Threading;

namespace HypeMachine
{
    public partial class MainPage : PhoneApplicationPage
    {
        int tempCount;
        List<Game> games;

        public MainPage()
        {
            InitializeComponent();

            object tempId;
            DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out tempId);
            byte[] phoneInfo = (byte[])tempId;

            games = new List<Game>();

            String url = "http://www.gamestop.com/SyndicationHandler.ashx?Filter=ComingSoon&platform=xbox360";

            WebClient gamestopClient = new WebClient();
            gamestopClient.OpenReadAsync(new Uri(url));
            gamestopClient.OpenReadCompleted += new OpenReadCompletedEventHandler(
                delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    if (e.Error == null)
                    {
                        parseRSS(e.Result);
                    }
                });
        }

        public void downloadHype(object data)
        {
            int index = (int)data;
            String tempUrl = String.Format("http://slyduck.com/hypemachine/frontend.php?intent=2&guid={0}", games[index].GuidString);

            WebClient client = new WebClient();
            client.OpenReadAsync(new Uri(tempUrl));
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(
                delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    if (e.Error == null)
                    {
                        XDocument xdoc2 = XDocument.Load(e.Result);
                        games[index].Hype = (from item in xdoc2.Descendants("hype")
                                     select new Hype()
                                     {
                                         Id = uint.Parse(item.Element("id").Value),
                                         GameId = uint.Parse(item.Element("game_id").Value),
                                         UserId = uint.Parse(item.Element("user_id").Value),
                                         Score = (uint.Parse(item.Element("score").Value) == 1)
                                     }).ToList();
                    }
                });

            String tempUrl2 = String.Format("http://slyduck.com/hypemachine/frontend.php?intent=3&guid={0}", games[index].GuidString);

            WebClient client2 = new WebClient();
            client2.OpenReadAsync(new Uri(tempUrl));
            client2.OpenReadCompleted += new OpenReadCompletedEventHandler(
                delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    if (e.Error == null)
                    {
                        XDocument xdoc2 = XDocument.Load(e.Result);
                        games[index].Aftermath = (from item in xdoc2.Descendants("aftermath")
                                             select new Aftermath()
                                             {
                                                 Id = uint.Parse(item.Element("id").Value),
                                                 GameId = uint.Parse(item.Element("game_id").Value),
                                                 UserId = uint.Parse(item.Element("user_id").Value),
                                                 Score = (uint.Parse(item.Element("score").Value) == 1)
                                             }).ToList();
                    }
                });

            String tempUrl3 = String.Format("http://slyduck.com/hypemachine/frontend.php?intent=4&guid={0}", games[index].GuidString);

            WebClient client3 = new WebClient();
            client3.OpenReadAsync(new Uri(tempUrl));
            client3.OpenReadCompleted += new OpenReadCompletedEventHandler(
                delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    if (e.Error == null)
                    {
                        XDocument xdoc2 = XDocument.Load(e.Result);
                        games[index].Comments = (from item in xdoc2.Descendants("comment")
                                                  select new Comment()
                                                  {
                                                      Id = uint.Parse(item.Element("id").Value),
                                                      GameId = uint.Parse(item.Element("game_id").Value),
                                                      UserId = uint.Parse(item.Element("user_id").Value),
                                                      Content = item.Element("content").Value,
                                                      Date = DateTime.Parse(item.Element("date").Value)
                                                  }).ToList();
                    }
                });
        }


        public void parseRSS(System.IO.Stream stream)
        {
            XDocument xdoc = XDocument.Load(stream);
            this.games = (from item in xdoc.Descendants("item")
                          select new Game()
                          {
                              GuidUri = new Uri(item.Element("guid").Value),
                              Link = new Uri(item.Element("link").Value),
                              Category = item.Element("category").Value,
                              Title = item.Element("title").Value,
                              Description = item.Element("description").Value,
                              PubDate = DateTime.Parse(item.Element("pubDate").Value)
                          }).ToList();

            tempCount = games.Count*3;

            List<Thread> threads = new List<Thread>();
            for(int i = 0; i < games.Count; i++)
            {
                threads.Add(new Thread(downloadHype));
                threads[i].Start(i);
            }
        }

    }
}