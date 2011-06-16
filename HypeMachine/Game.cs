using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace HypeMachine
{
    [DataContract(Name = "Game")]
    public class Game
    {
        private static readonly Uri BASE_URI = new Uri("http://www.gamestop.com");
        private static readonly Uri BASE_PRODUCT_URI = new Uri("http://www.gamestop.com/Catalog/");
        private static readonly String PRODUCT_SCRIPT = "ProductDetails.aspx?sku=";
        private static readonly Regex IMAGE_REGEX = new Regex(@"(?<=img+.+src\=[\x27\x22])(?<IMAGE>[^\x27\x22]*)(?=[\x27\x22])");
        private static readonly Regex PRICE_REGEX = new Regex(@"(?<PRICE>((\d{1,3},)*\d+)\.(\d{2}))");
        private static readonly Regex SUMMARY_REGEX = new Regex(@"<br> (?<SUMMARY>.*)");

        private uint id;
        [DataMember(Name = "Id")]
        public uint Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        private List<Hype> hype;
        [DataMember(Name = "Hype")]
        public List<Hype> Hype
        {
            get
            {
                return this.hype;
            }
            set
            {
                this.hype = value;
            }
        }

        private List<Aftermath> aftermath;
        [DataMember(Name = "Aftermath")]
        public List<Aftermath> Aftermath
        {
            get
            {
                return this.aftermath;
            }
            set
            {
                this.aftermath = value;
            }
        }

        public float HypeScore
        {
            get
            {
                float temp = 0.0f;
                float result = 0.0f;
                foreach (Hype hype in this.Hype)
                {
                    if (hype.Score)
                    {
                        temp += 1;
                    }
                    else
                    {
                        temp -= 1;
                    }
                }

                if (this.Hype.Count > 0)
                {
                    result = temp / (float)this.Hype.Count;
                }

                return result;
            }
        }

        public float AftermathScore
        {
            get
            {
                float temp = 0.0f;
                float result = 0.0f;
                foreach (Aftermath aftermath in this.Aftermath)
                {
                    if (aftermath.Score)
                    {
                        temp += 1;
                    }
                    else
                    {
                        temp -= 1;
                    }
                }

                if (this.Aftermath.Count > 0)
                {
                    result = temp / (float)this.Aftermath.Count;
                }

                return result;
            }
        }

        private List<Comment> comments;
        [DataMember(Name = "Comments")]
        public List<Comment> Comments
        {
            get
            {
                return this.comments;
            }
            set
            {
                this.comments = value;
            }
        }

        private String guidString;
        [DataMember(Name = "GuidString")]
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
        [DataMember(Name = "GuidUri")]
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

        private Uri link;
        [DataMember(Name = "Link")]
        public Uri Link
        {
            get
            {
                return this.link;
            }
            set
            {
                this.link = value;
            }
        }

        private String category;
        [DataMember(Name = "Category")]
        public String Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        private String title;
        [DataMember(Name = "Title")]
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

        private Uri image;
        [DataMember(Name = "Image")]
        public Uri Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
            }
        }

        private float price;
        [DataMember(Name = "Price")]
        public float Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }

        private string summary;
        [DataMember(Name = "Summary")]
        public String Summary
        {
            get
            {
                return this.summary;
            }
            set
            {
                this.summary = value;
            }
        }

        public String ShortSummary
        {
            get
            {
                return this.Summary.Substring(0, Math.Min(70, this.Summary.Length)) + "...";
            }
        }

        private DateTime releaseDate;
        [DataMember(Name = "ReleaseDate")]
        public DateTime ReleaseDate
        {
            get
            {
                return this.releaseDate;
            }
            set
            {
                this.releaseDate = value;
            }
        }

        private String description;
        [DataMember(Name = "Description")]
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

        private DateTime pubDate;
        [DataMember(Name = "PubDate")]
        public DateTime PubDate
        {
            get
            {
                return this.pubDate;
            }
            set
            {
                this.pubDate = value;
            }
        }

        public Game() 
        {
            this.Hype = new List<Hype>();
            this.Aftermath = new List<Aftermath>();
            this.Comments = new List<Comment>();
        }

        public override string ToString()
        {
            return String.Format("ID: {0}\nGUID: {1}\nLINK: {2}\nCATEGORY: {3}\nTITLE: {4}\nIMAGE: {5}\nPRICE: {6}\nSUMMARY: {7}\nRELEASEDATE: {8}\nPUBDATE: {9}\nHYPESCORE: {10}\nAFTERMATHSCORE: {11}\nCOMMENTS: {12}\n----------", this.Id.ToString(), this.GuidString.ToString(), this.Link.ToString(), this.Category, this.Title, this.Image.ToString(), this.Price.ToString(), this.ShortSummary, this.ReleaseDate.ToShortDateString(), this.PubDate.ToShortDateString(), this.HypeScore.ToString(), this.AftermathScore.ToString(), this.Comments.Count.ToString());
        }
    }
}
