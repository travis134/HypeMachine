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
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace HypeMachine
{
    [DataContract(Name = "Comment")]
    public class Comment
    {
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

        private uint gameId;
        [DataMember(Name = "GameId")]
        public uint GameId
        {
            get
            {
                return this.gameId;
            }
            set
            {
                this.gameId = value;
            }
        }

        private uint userId;
        [DataMember(Name = "UserId")]
        public uint UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
        }

        private String content;
        [DataMember(Name = "Content")]
        public String Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        private DateTime date;
        [DataMember(Name = "Date")]
        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
            }
        }

        public Comment() { }
        public Comment(uint id, uint gameId, uint userId, String comment, DateTime date)
        {
            this.Id = id;
            this.GameId = gameId;
            this.UserId = userId;
            this.Content = comment;
            this.Date = date;
        }

        public override string ToString()
        {
            return String.Format("Id: {0}\nGameId: {1}\nUserId: {2}\nContent: {3}\nDate: {4}\n----------", this.Id.ToString(), this.GameId.ToString(), this.UserId.ToString(), this.Content.ToString(), this.Date.ToShortDateString());
        }
    }
}
