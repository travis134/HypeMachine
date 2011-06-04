using System;
using System.Xml;
using System.Text.RegularExpressions;

namespace HypeMachine
{
    public class Game
    {
        public Uri Guid { get; set; }
        public Uri Link { get; set; }
        public String Category { get; set; }
        public String Title { get; set; }

        public Uri Image { get; set; }
        public float Price { get; set; }
        public String Summary { get; set; }
        public DateTime ReleaseDate { get; set; }

        private String description;
        public String Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
                /*Regex oRegex = new Regex("<img src=\"(?<IMAGE>.+.(?:jpg|gif|png))\".*");
                MatchCollection oMatchCollection = oRegex.Matches(this.description);
                foreach (Match oMatch in oMatchCollection)
                {
                    this.Image = new Uri(oMatch.Groups["IMAGE"].ToString());
                    this.Price = float.Parse(oMatch.Groups["PRICE"].ToString());
                    DateTime tempDate;
                    if (DateTime.TryParse(oMatch.Groups["RELEASEDATE"].ToString(), out tempDate))
                    {
                        this.ReleaseDate = tempDate;
                    }
                    this.Summary = oMatch.Groups["Summary"].ToString();
                }*/
            }
        }

        public DateTime PubDate { get; set; }

        public Game() { }

        public override string ToString()
        {
            return "GUID: " + this.Guid.ToString() + "\n" + "LINK: " + this.Link.ToString() + "\n" +
                "CATEGORY: " + this.Category + "\n" + "TITLE: " + this.Title + "\n" + "IMAGE: " + this.Image + "\n" +
                "PRICE: " + this.Price + "\n" + "SUMMARY: " + this.Summary + "\n" + "RELEASEDATE: " + this.ReleaseDate + "\n" +
                "PUBDATE: " + this.PubDate.ToShortDateString() + "\n--------------------\n\n";
        }
    }
}
