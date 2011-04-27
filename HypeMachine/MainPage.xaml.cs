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
using System.ServiceModel.Syndication;
using System.Xml;
using System.Windows.Media.Imaging;

namespace HypeMachine
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            String url = "http://www.gamestop.com/gs/content/feeds/gs_cs_All.xml";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.BeginGetResponse(ResponseHandler, request);
        }

        private void ResponseHandler(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                XmlReader reader = XmlReader.Create(response.GetResponseStream());
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    List<Game> games = new List<Game>();
                    PanoramaItem panoramaItem = new PanoramaItem();
                    ListBox listBox = new ListBox();
                    foreach (SyndicationItem item in feed.Items)
                    {
                        games.Add(new Game(item.Title.Text.ToString(), item.Links[0].Uri.ToString(), item.Summary.Text.ToString(), item.Categories[0].Name.ToString()));
                    }
                    foreach (Game game in games)
                    {
                        listBox.Items.Add(new Image(){Source = new BitmapImage(game.Image)});
                        System.Diagnostics.Debug.WriteLine(game.Image.ToString());
                    }
                    panoramaItem.Content = listBox;
                    mainView.Items.Add(panoramaItem);
                });
            }
        }
    }
}