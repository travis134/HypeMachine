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
    public abstract class Rating
    {
        public uint Id { get; set; }
        public uint GameId { get; set; }
        public uint UserId { get; set; }
        public Boolean Score { get; set; }

        public Rating() { }
        public Rating(uint id, uint gameId, uint userId, Boolean score)
        {
            this.Id = id;
            this.GameId = gameId;
            this.UserId = userId;
            this.Score = score;
        }

        public override string ToString()
        {
            return String.Format("Id: {0}\nGameId: {1}\nUserId: {2}\nScore: {3}\n----------", this.Id.ToString(), this.GameId.ToString(), this.UserId.ToString(), this.Score.ToString());
        }
    }
}
