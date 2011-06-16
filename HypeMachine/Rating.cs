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
using System.Runtime.Serialization;

namespace HypeMachine
{
    [DataContract(Name = "Rating")]
    public abstract class Rating
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

        private Boolean score;
        [DataMember(Name = "Score")]
        public Boolean Score
        {
            get
            {
                return this.score;
            }
            set
            {
                this.score = value;
            }
        }

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
