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

namespace HypeMachine
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<Game> games;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            String url = "http://www.gamestop.com/gs/content/feeds/gs_cs_All.xml";

            WebClient gamestopClient = new WebClient();
            gamestopClient.OpenReadAsync(new Uri(url));
            gamestopClient.OpenReadCompleted += new OpenReadCompletedEventHandler(request_DownloadGamesInfo);
        
        }

        void request_DownloadGamesInfo(object sender,
            OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                XDocument xdoc = XDocument.Load(e.Result);
                this.games = (from item in xdoc.Descendants("item")
                                                  select new Game()
                                                  {
                                                      Guid = new Uri(item.Element("guid").Value),
                                                      Link = new Uri(item.Element("link").Value),
                                                      Category = item.Element("category").Value,
                                                      Title = item.Element("title").Value,
                                                      Description = item.Element("description").Value,
                                                      PubDate = DateTime.Parse(item.Element("pubDate").Value)
                                                  }).ToList();

                foreach (Game game in this.games)
                {
                    System.Diagnostics.Debug.WriteLine(game.ToString());
                    System.Diagnostics.Debug.WriteLine("\n\n--------------------\n\n");
                }
            }
        }

        public void parseRSS(XmlReader gamestopReader)
        {
            while (gamestopReader.Read())
            {
                if (gamestopReader.NodeType == XmlNodeType.Element)
                {
                    switch (gamestopReader.Name)
                    {
                        case "item":
                            XmlReader gameReader = gamestopReader.ReadSubtree();
                            this.games.Add(parseGame(gameReader));
                            break;
                    }
                }
            }
        }

        public Game parseGame(XmlReader gamesReader)
        {
            Game gameResult = new Game();

            while (gamesReader.Read())
            {
                if (gamesReader.NodeType == XmlNodeType.Element)
                {
                    switch (gamesReader.Name)
                    {
                        case "guid":
                            gameResult.Guid = new Uri(gamesReader.ReadElementContentAsString());
                            break;
                        case "link":
                            gameResult.Link = new Uri(gamesReader.ReadElementContentAsString());
                            break;
                        case "category":
                            gameResult.Category = gamesReader.ReadElementContentAsString();
                            break;
                        case "title":
                            gameResult.Title = gamesReader.ReadElementContentAsString();
                            break;
                        case "description":
                            gameResult.Description = gamesReader.ReadElementContentAsString();
                            break;
                        case "pubDate":
                            gameResult.PubDate = gamesReader.ReadElementContentAsDateTime();
                            break;
                    }
                }
            }

            return gameResult;
        }
    }
}