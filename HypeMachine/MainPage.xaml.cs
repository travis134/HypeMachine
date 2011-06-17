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
using System.Windows.Media.Imaging;
using Microsoft.Advertising.Mobile.UI;

namespace HypeMachine
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Boolean firstRun;
        private Dictionary<String, Game> gamesList;
        private StorageHelper<Game> gamesStorageHelper;
        static Random rand = new Random();
        private User currentUser;
        private String anid;

        private Dictionary<String, BitmapImage> barImages = new Dictionary<String, BitmapImage>()
        {
            {"minusFull", new BitmapImage(new Uri("bar/minusFull.png", UriKind.Relative))},
            {"minusThree", new BitmapImage(new Uri("bar/minusThree.png", UriKind.Relative))},
            {"minusTwo", new BitmapImage(new Uri("bar/minusTwo.png", UriKind.Relative))},
            {"minusOne", new BitmapImage(new Uri("bar/minusOne.png", UriKind.Relative))},
            {"stale", new BitmapImage(new Uri("bar/stale.png", UriKind.Relative))},
            {"plusOne", new BitmapImage(new Uri("bar/plusOne.png", UriKind.Relative))},
            {"plusTwo", new BitmapImage(new Uri("bar/plusTwo.png", UriKind.Relative))},
            {"plusThree", new BitmapImage(new Uri("bar/plusThree.png", UriKind.Relative))},
            {"plusFull", new BitmapImage(new Uri("bar/plusFull.png", UriKind.Relative))},
            {"minus", new BitmapImage(new Uri("bar/minus.png", UriKind.Relative))},
            {"plus", new BitmapImage(new Uri("bar/plus.png", UriKind.Relative))},
            {"minusPressed", new BitmapImage(new Uri("bar/minusPressed.png", UriKind.Relative))},
            {"plusPressed", new BitmapImage(new Uri("bar/plusPressed.png", UriKind.Relative))}
        };

        public MainPage()
        {
            InitializeComponent();

            AdControl hypeMachineAd = new AdControl() { ApplicationId = "4d3667ee-44c1-494c-929d-5e661b011918", AdUnitId = "10019312"};
            ((Grid)FindName("LayoutRoot")).Children.Add(hypeMachineAd);
            Grid.SetRow(hypeMachineAd, 1);

            bool darkTheme = ((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible);
            Pivot mainView = (Pivot)FindName("mainView");
            if (!darkTheme)
            {
                mainView.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri("PanoramaBackground.png", UriKind.Relative)), Stretch=Stretch.None };
                mainView.Foreground = new SolidColorBrush(Colors.Black);
            }

            object tempAnid;
            UserExtendedProperties.TryGetValue("ANID", out tempAnid);

            if (tempAnid != null && tempAnid.ToString().Length >= (32 + 2))
            {
                this.anid = tempAnid.ToString().Substring(2, 32);
            } 

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
            if (gamesStorageHelper.IsStale(new TimeSpan(0, 0, 0)))
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
            Boolean alternate = true;

            foreach (KeyValuePair<String, Game> game in this.gamesList)
            {
                StackPanel hypePanel = (StackPanel)FindName("HypePanel");
                hypePanel.Children.Add(gameToBorder(game.Value,alternate));

                alternate = !alternate;
            }
            ((Grid)FindName("LayoutRoot")).Children.Remove((Grid)FindName("Loading"));
        }

        Border gameToBorder(Game game, Boolean alternate)
        {
            Border border = new Border() { BorderThickness = new System.Windows.Thickness(2), /*CornerRadius = new System.Windows.CornerRadius(30),*/ Margin = new System.Windows.Thickness(0, 0, 0, 10) };

            RowDefinition rowTopPadding = new RowDefinition() { Height = new System.Windows.GridLength(10) };
            RowDefinition rowBottomPadding = new RowDefinition() { Height = new System.Windows.GridLength(10) };

            RowDefinition rowTop = new RowDefinition() { Height = new System.Windows.GridLength(0, GridUnitType.Auto) };
            RowDefinition rowBottom = new RowDefinition() { Height = new System.Windows.GridLength(76) };
            Grid tempRow = new Grid() { };

            tempRow.RowDefinitions.Add(rowTopPadding);
            tempRow.RowDefinitions.Add(rowTop);
            tempRow.RowDefinitions.Add(rowBottom);
            tempRow.RowDefinitions.Add(rowBottomPadding);

            ColumnDefinition gameLeftPadding = new ColumnDefinition() { Width = new System.Windows.GridLength(10) };
            ColumnDefinition gameRightPadding = new ColumnDefinition() { Width = new System.Windows.GridLength(10) };

            ColumnDefinition gameLeft = new ColumnDefinition() { Width = new System.Windows.GridLength(100) };
            ColumnDefinition gameRight = new ColumnDefinition() { Width = new System.Windows.GridLength(100, GridUnitType.Star) };
            Grid tempGame = new Grid() { Margin = new System.Windows.Thickness(0, 0, 0, 20) };
            tempGame.ColumnDefinitions.Add(gameLeftPadding);
            tempGame.ColumnDefinitions.Add(gameLeft);
            tempGame.ColumnDefinitions.Add(gameRight);
            tempGame.ColumnDefinitions.Add(gameRightPadding);

            RowDefinition infoTop = new RowDefinition() { Height = new System.Windows.GridLength(0, GridUnitType.Auto) };
            RowDefinition infoBottom = new RowDefinition() { Height = new System.Windows.GridLength(0, GridUnitType.Auto) };
            Grid tempInfo = new Grid() { Margin = new System.Windows.Thickness(10, 0, 0, 0) };
            tempInfo.RowDefinitions.Add(infoTop);
            tempInfo.RowDefinitions.Add(infoBottom);

            TextBlock tempTitle = new TextBlock { Text = game.Title.ToString().ToUpper(), FontWeight = System.Windows.FontWeights.ExtraBold, FontSize = 28, TextWrapping = System.Windows.TextWrapping.Wrap };
            TextBlock tempDescription = new TextBlock { Text = (game.ShortSummary + "Tap for more info and comments").ToLower(), FontSize = 20, TextWrapping = System.Windows.TextWrapping.Wrap };

            Grid.SetRow(tempTitle, 0);
            tempInfo.Children.Add(tempTitle);
            Grid.SetRow(tempDescription, 1);
            tempInfo.Children.Add(tempDescription);

            Border imageBorder = new Border() { Width = 100, Height = 100, BorderThickness = new System.Windows.Thickness(3), VerticalAlignment = System.Windows.VerticalAlignment.Top, Margin = new Thickness(0, 10, 0, 0) };

            if (alternate)
            {
                tempTitle.Foreground = new SolidColorBrush((Color)Resources["PhoneForegroundColor"]);
                tempDescription.Foreground = new SolidColorBrush((Color)Resources["PhoneForegroundColor"]);
                border.Background = new SolidColorBrush((Color)Resources["PhoneAccentColor"]);
                imageBorder.BorderBrush = new SolidColorBrush((Color)Resources["PhoneForegroundColor"]);
            }
            else
            {
                tempTitle.Foreground = new SolidColorBrush((Color)Resources["PhoneBackgroundColor"]);
                tempDescription.Foreground = new SolidColorBrush((Color)Resources["PhoneBackgroundColor"]);
                border.Background = new SolidColorBrush((Color)Resources["PhoneForegroundColor"]);
                imageBorder.BorderBrush = new SolidColorBrush((Color)Resources["PhoneAccentColor"]);
            }

            Image tempImage = new Image() { Source = new BitmapImage(game.Image), Stretch = System.Windows.Media.Stretch.UniformToFill };

            imageBorder.Child = tempImage;

            Grid.SetColumn(imageBorder, 1);

            tempGame.Children.Add(imageBorder);

            Grid.SetColumn(tempInfo, 2);
            tempGame.Children.Add(tempInfo);

            Grid.SetRow(tempGame, 1);
            tempRow.Children.Add(tempGame);

            ColumnDefinition barMinus = new ColumnDefinition() { Width = new System.Windows.GridLength(63) };
            ColumnDefinition barFill = new ColumnDefinition() { Width = new System.Windows.GridLength(224) };
            ColumnDefinition barPlus = new ColumnDefinition() { Width = new System.Windows.GridLength(70) };
            Grid tempBar = new Grid() { HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
            tempBar.ColumnDefinitions.Add(barMinus);
            tempBar.ColumnDefinitions.Add(barFill);
            tempBar.ColumnDefinitions.Add(barPlus);

            Button minusButton = new Button() { Width = 63, Height = 76, BorderThickness = new Thickness(0), Padding = new Thickness(-12) };
            Image minusImage = new Image() { Source = this.barImages["minus"], Width = 63, Height = 76 };
            minusButton.Content = minusImage;
            minusButton.ClickMode = ClickMode.Release;
            minusButton.Click += new RoutedEventHandler(minusButton_Click);

            Rectangle barContainer = new Rectangle() { Width = 224, Height = 76 };
            ImageBrush barImage = new ImageBrush() { ImageSource = this.barImages["stale"], Stretch = System.Windows.Media.Stretch.Fill };

            double tempRand = rand.NextDouble();
            if (game.HypeScore >= .2 && game.HypeScore < .4)
            {
                barImage = new ImageBrush() { ImageSource = this.barImages["plusOne"], Stretch = System.Windows.Media.Stretch.Fill };
            }
            else if (game.HypeScore >= .4 && game.HypeScore < .6)
            {
                barImage = new ImageBrush() { ImageSource = this.barImages["plusTwo"], Stretch = System.Windows.Media.Stretch.Fill };
            }
            else if (game.HypeScore >= .6 && game.HypeScore < .8)
            {
                barImage = new ImageBrush() { ImageSource = this.barImages["plusThree"], Stretch = System.Windows.Media.Stretch.Fill };
            }
            else if (game.HypeScore >= .8)
            {
                barImage = new ImageBrush() { ImageSource = this.barImages["plusFull"], Stretch = System.Windows.Media.Stretch.Fill };
            }
            else if ((game.HypeScore <= -.2 && game.HypeScore > -.4))
            {
                barImage = new ImageBrush() { ImageSource = this.barImages["minusOne"], Stretch = System.Windows.Media.Stretch.Fill };
            }
            else if ((game.HypeScore <= -.4 && game.HypeScore > -.6))
            {
                barImage = new ImageBrush() { ImageSource = this.barImages["minusTwo"], Stretch = System.Windows.Media.Stretch.Fill };
            }
            else if ((game.HypeScore <= -.6 && game.HypeScore > -.8))
            {
                barImage = new ImageBrush() { ImageSource = this.barImages["minusThree"], Stretch = System.Windows.Media.Stretch.Fill };
            }
            else if (game.HypeScore <= -.8)
            {
                barImage = new ImageBrush() { ImageSource = this.barImages["minusFull"], Stretch = System.Windows.Media.Stretch.Fill };
            }

            barContainer.Fill = barImage;

            Button plusButton = new Button() { Width = 70, Height = 76, BorderThickness = new Thickness(0), Padding = new Thickness(-12) };
            Image plusImage = new Image() { Source = this.barImages["plus"], Width = 70, Height = 76 };
            plusButton.Content = plusImage;

            plusButton.ClickMode = ClickMode.Release;
            plusButton.Click += new RoutedEventHandler(plusButton_Click);

            Grid.SetColumn(minusButton, 0);
            tempBar.Children.Add(minusButton);

            Grid.SetColumn(barContainer, 1);
            tempBar.Children.Add(barContainer);

            Grid.SetColumn(plusButton, 2);
            tempBar.Children.Add(plusButton);

            Grid.SetRow(tempBar, 2);
            tempRow.Children.Add(tempBar);

            border.Child = tempRow;

            return border;
        }

        void plusButton_Click(object sender, RoutedEventArgs e)
        {
            Button temp = (Button)sender;
            if (temp.IsPressed)
            {
                temp.Content = new Image() { Source = this.barImages["plusPressed"], Width = 70, Height = 76 };
            }
            else
            {
                temp.Content = new Image() { Source = this.barImages["plus"], Width = 70, Height = 76 };
            }
        }

        void minusButton_Click(object sender, RoutedEventArgs e)
        {
            Button temp = (Button)sender;
            if (temp.IsPressed)
            {
                temp.Content = new Image() { Source = this.barImages["minusPressed"], Width = 63, Height = 76 };
            }
            else
            {
                temp.Content = new Image() { Source = this.barImages["minus"], Width = 63, Height = 76 };
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

            if (!String.IsNullOrEmpty(this.anid))
            {
                tempUrl += String.Format("&user_args[anid]={0}&user_args[nickname]={1}", this.anid, "Travis");
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
                                     Anid = item.Element("anid").Value,
                                     Nickname = item.Element("nickname").Value
                                 }).ToList();
                        games = (from item in xdoc2.Descendants("Game")
                                 select new
                                 {
                                     Key = item.Element("guid").Value,
                                     Value = uint.Parse(item.Element("id").Value)
                                 }).ToDictionary(o => o.Key, o => o.Value);

                        foreach (User user in users)
                        {
                            if (user.Anid.Equals(this.anid, StringComparison.OrdinalIgnoreCase))
                            {
                                this.currentUser = user;
                                break;
                            }
                        }

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