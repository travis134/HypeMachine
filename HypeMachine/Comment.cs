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

namespace HypeMachine
{
    public class Comment
    {
        public uint Id { get; set; }
        public uint GameId { get; set; }
        public uint UserId { get; set; }
        public String Content { get; set; }
        public DateTime Date { get; set; }

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
