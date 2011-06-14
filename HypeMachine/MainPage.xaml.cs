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
using System.IO.IsolatedStorage;

namespace HypeMachine
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Boolean firstRun;
        private Dictionary<String, Game> gamesList;
        private StorageHelper<Game> gamesStorageHelper;

        public MainPage()
        {
            InitializeComponent();

            object tempId;
            DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out tempId);
            byte[] phoneInfo = (byte[])tempId;

            this.gamesList = new Dictionary<String, Game>();
            gamesStorageHelper = new StorageHelper<Game>("Games.xml", "Stale.txt");

            /*
            object tempFirstRun;

            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("firstRun", out tempFirstRun))
            {
                IsolatedStorageSettings.ApplicationSettings["firstRun"] = false;
                firstRun = (Boolean)tempFirstRun;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add("firstRun", false);
                firstRun = true;
            }

            if (firstRun)
            {
                this.NavigationService.Navigate(new Uri("/FirstRun.xaml", UriKind.Relative));
            }*/

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (gamesStorageHelper.IsStale(new TimeSpan(4, 0, 0)))
            {
                refreshInfo();
            }
            else
            {
                this.loadFromStorage();
            }
        }

        private void updateUi()
        {
            foreach (KeyValuePair<String, Game> game in this.gamesList)
            {
                System.Diagnostics.Debug.WriteLine(game.Value.ToString());
            }
        }

        public void refreshInfo()
        {
            String url = "http://www.gamestop.com/SyndicationHandler.ashx?Filter=ComingSoon&platform=xbox360";

            WebClient gamestopClient = new WebClient();
            gamestopClient.OpenReadAsync(new Uri(url));
            gamestopClient.OpenReadCompleted += new OpenReadCompletedEventHandler(
                delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    if (e.Error == null)
                    {
                        parseGames(e.Result);
                    }
                    else
                    {
                        this.loadFromStorage();
                    }
                });
        }

        private void loadFromStorage()
        {
            System.Diagnostics.Debug.WriteLine("Reading from storage");
            List<Game> tempGames = new List<Game>();
            tempGames = this.gamesStorageHelper.LoadAll();
            foreach (Game game in tempGames)
            {
                if (!String.IsNullOrEmpty(game.GuidString))
                {
                    this.gamesList.Add(game.GuidString, game);
                }
            }
            this.updateUi();
        }

        public void parseGames(System.IO.Stream stream)
        {
            XDocument xdoc = XDocument.Load(stream);
            List<Game> tempGamesList = new List<Game>();
            tempGamesList = (from item in xdoc.Descendants("item")
                          select new Game()
                          {
                              GuidUri = new Uri(item.Element("guid").Value),
                              Link = new Uri(item.Element("link").Value),
                              Category = item.Element("category").Value,
                              Title = item.Element("title").Value,
                              Description = item.Element("description").Value,
                              PubDate = DateTime.Parse(item.Element("pubDate").Value)
                          }).ToList();


            String tempUrl = "http://slyduck.com/hypemachine/driver.php?intent=readAll";
            foreach (Game game in tempGamesList)
            {
                if (!String.IsNullOrEmpty(game.GuidString))
                {
                    this.gamesList.Add(game.GuidString, game);
                    tempUrl += String.Format("&guids[]={0}", game.GuidString);
                }
            }

            List<Hype> hypes = new List<Hype>();
            List<Aftermath> aftermaths = new List<Aftermath>();
            List<Comment> comments = new List<Comment>();
            List<User> users = new List<User>();
            Dictionary<String, uint> games = new Dictionary<String, uint>();


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
                                 select new
                                 {
                                     Key = item.Element("guid").Value,
                                     Value = uint.Parse(item.Element("id").Value)
                                 }).ToDictionary(o => o.Key, o => o.Value);

                        if (this.gamesList.Count > 0)
                        {
                            List<Game> tempGames = new List<Game>();
                            foreach (KeyValuePair<String, Game> game in this.gamesList)
                            {
                                if (games.ContainsKey(game.Key))
                                {
                                    this.gamesList[game.Key].Id = games[game.Key];
                                    foreach (Hype hype in hypes)
                                    {
                                        if (hype.GameId == this.gamesList[game.Key].Id)
                                        {
                                            this.gamesList[game.Key].Hype.Add(hype);
                                        }
                                    }
                                    foreach (Aftermath aftermath in aftermaths)
                                    {
                                        if (aftermath.GameId == this.gamesList[game.Key].Id)
                                        {
                                            this.gamesList[game.Key].Aftermath.Add(aftermath);
                                        }
                                    }
                                    foreach (Comment comment in comments)
                                    {
                                        if (comment.GameId == this.gamesList[game.Key].Id)
                                        {
                                            this.gamesList[game.Key].Comments.Add(comment);
                                        }
                                    }
                                    tempGames.Add(game.Value);
                                }
                            }
                            this.gamesStorageHelper.SaveAll(tempGames);
                            this.updateUi();
                        }
                        else
                        {
                            loadFromStorage();
                        }
                    }
                    else
                    {
                        loadFromStorage();
                    }
                });
        }

    }
}