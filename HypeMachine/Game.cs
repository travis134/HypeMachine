using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace HypeMachine
{
    public class Game
    {
        public String Name { get; set; }
        public Uri Image { get; set; }
        public String Price { get; set; }
        public Uri Link { get; set; }
        public String Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public String Platform { get; set; }

        public Game() { }
        public Game(String name, String link, String summary, String platform)
        {
            this.Name = name;
            this.Link = new Uri(link);

            Regex oRegex = new Regex("<img src=\"(?<IMAGE>https?://(?:[a-z-]+.)+[a-z]{2,6}(?:/[^/#?]+)+.(?:jpg|gif|png))\".*<b>Price:</b> \\$(?<PRICE>[0-9]*.[0-9]*) <b>ETA:</b> (?<RELEASEDATE>[0-9]+/[0-9]+/[0-9]+)<br>(?<DESCRIPTION>.*)");
            MatchCollection oMatchCollection = oRegex.Matches(summary);
            foreach (Match oMatch in oMatchCollection)
            {
                this.Image = new Uri(oMatch.Groups["IMAGE"].ToString());
                this.Price = oMatch.Groups["PRICE"].ToString();
                DateTime tempDate;
                if (DateTime.TryParse(oMatch.Groups["RELEASEDATE"].ToString(), out tempDate))
                {
                    this.ReleaseDate = tempDate;
                }
                this.Description = oMatch.Groups["DESCRIPTION"].ToString();
            }
            this.Platform = platform;
        }
    }
}
