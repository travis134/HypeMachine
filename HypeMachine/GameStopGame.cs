using System;
using System.Xml;
using System.Text.RegularExpressions;

namespace HypeMachine
{
    public class Game
    {
        private static readonly Uri BASE_URI = new Uri("http://www.gamestop.com");
        private static readonly Uri BASE_PRODUCT_URI = new Uri("http://www.gamestop.com/Catalog/");
        private static readonly String PRODUCT_SCRIPT = "ProductDetails.aspx?sku=";
        private static readonly Regex IMAGE_REGEX = new Regex(@"(?<=img+.+src\=[\x27\x22])(?<IMAGE>[^\x27\x22]*)(?=[\x27\x22])");
        private static readonly Regex PRICE_REGEX = new Regex(@"(?<PRICE>((\d{1,3},)*\d+)\.(\d{2}))");
        private static readonly Regex SUMMARY_REGEX = new Regex(@"<br> (?<SUMMARY>.*)");

        private String guidString;
        public String GuidString
        {
            get
            {
                return this.guidString;
            }
            set
            {
                this.guidString = value;
                this.guidUri = new Uri(Game.BASE_PRODUCT_URI, Game.PRODUCT_SCRIPT + this.guidString);
            }
        }
        private Uri guidUri;
        public Uri GuidUri
        {
            get
            {
                return this.guidUri;
            }
            set
            {
                this.guidUri = value;
                this.guidString = this.guidUri.ToString().Replace("http://www.gamestop.com/Catalog/ProductDetails.aspx?sku=", "");
            }
        }
        public Uri Link { get; set; }
        public String Category { get; set; }

        private String title;
        public String Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value.Replace("- " + this.Category, String.Empty).Trim();
            }
        }

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

                Match imageMatch = Game.IMAGE_REGEX.Match(this.description);
                this.Image = new Uri(Game.BASE_URI, imageMatch.Groups["IMAGE"].ToString());

                Match priceMatch = Game.PRICE_REGEX.Match(this.description);
                this.Price = float.Parse(priceMatch.Groups["PRICE"].ToString());

                Match summaryMatch = Game.SUMMARY_REGEX.Match(this.description);
                this.Summary = summaryMatch.Groups["SUMMARY"].ToString();

                DateTime tempDate;
                if (DateTime.TryParse(this.description, out tempDate))
                {
                    this.ReleaseDate = tempDate;
                }
            }
        }

        public DateTime PubDate { get; set; }

        public Game() 
        {
        }

        public override string ToString()
        {
            return String.Format("GUID: {0}\nLINK: {1}\nCATEGORY: {2}\nTITLE: {3}\nIMAGE: {4}\nPRICE: {5}\nSUMMARY: {6}\nRELEASEDATE: {7}\nPUBDATE: {8}\n----------\n\n", this.GuidString.Length.ToString(), this.Link.ToString(), this.Category, this.Title, this.Image.ToString(), this.Price.ToString(), this.Summary.Substring(0, Math.Min(100, this.Summary.Length)) + "...", this.ReleaseDate.ToShortDateString(), this.PubDate.ToShortDateString());
        }
    }
}
