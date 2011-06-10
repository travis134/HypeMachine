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

            String tempUrl = "http://slyduck.com/hypemachine/test_driver.php";

            List<Hype> hypes = new List<Hype>();
            List<Aftermath> aftermaths = new List<Aftermath>();
            List<Comment> comments = new List<Comment>();
            List<User> users = new List<User>();
            List<Game> games = new List<Game>();

            WebClient client = new WebClient();
            client.OpenReadAsync(new Uri(tempUrl));
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(
                delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    if (e.Error == null)
                    {
                        XDocument xdoc2 = XDocument.Load(e.Result);
                        hypes = (from item in xdoc2.Descendants("Hype")
                                select new Hype()
                                {
                                    Id = uint.Parse(item.Element("id").Value),
                                    GameId = uint.Parse(item.Element("game_id").Value),
                                    UserId = uint.Parse(item.Element("user_id").Value),
                                    Score = (uint.Parse(item.Element("score").Value) == 1)
                                }).ToList();
                        aftermaths = (from item in xdoc2.Descendants("Aftermath")
                                 select new Aftermath()
                                 {
                                     Id = uint.Parse(item.Element("id").Value),
                                     GameId = uint.Parse(item.Element("game_id").Value),
                                     UserId = uint.Parse(item.Element("user_id").Value),
                                     Score = (uint.Parse(item.Element("score").Value) == 1)
                                 }).ToList();
                        comments = (from item in xdoc2.Descendants("Comment")
                                 select new Comment()
                                 {
                                     Id = uint.Parse(item.Element("id").Value),
                                     GameId = uint.Parse(item.Element("game_id").Value),
                                     UserId = uint.Parse(item.Element("user_id").Value),
                                     Content = item.Element("content").Value,
                                     Date = DateTime.Parse(item.Element("date").Value)
                                 }).ToList();
                        users = (from item in xdoc2.Descendants("User")
                                select new User()
                                {
                                    Id = uint.Parse(item.Element("id").Value),
                                    DeviceUniqueId = item.Element("device_unique_id").Value,
                                    Nickname = item.Element("nickname").Value
                                }).ToList();
                        games = (from item in xdoc2.Descendants("Game")
                                 select new Game()
                                 {
                                     Id = uint.Parse(item.Element("id").Value),
                                     GuidString = item.Element("guid").Value
                                 }).ToList();
                    }

                    foreach (Game game in this.games)
                    {
                        foreach (Game game2 in games)
                        {
                            if(game.GuidString.Equals(game2.GuidString, StringComparison.OrdinalIgnoreCase))
                            {
                                game.Id = game2.Id;
                                foreach (Hype hype in hypes)
                                {
                                    if (hype.GameId == game.Id)
                                    {
                                        game.Hype.Add(hype);
                                    }
                                }
                                foreach (Aftermath aftermath in aftermaths)
                                {
                                    if (aftermath.GameId == game.Id)
                                    {
                                        game.Aftermath.Add(aftermath);
                                    }
                                }
                                foreach (Comment comment in comments)
                                {
                                    if (comment.GameId == game.Id)
                                    {
                                        game.Comments.Add(comment);
                                    }
                                }
                                System.Diagnostics.Debug.WriteLine(game.ToString());
                                break;
                            }
                        }
                        
                    }
                });
        }

    }
}