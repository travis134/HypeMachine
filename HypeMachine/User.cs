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
    [DataContract(Name = "User")]
    public class User
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

        private char[] anid;
        [DataMember(Name = "Anid")]
        public String Anid
        {
            get
            {
                return new String(this.anid);
            }
            set
            {
                this.anid = value.ToCharArray();
            }
        }

        private char[] nickname;
        [DataMember(Name = "Nickname")]
        public String Nickname
        {
            get
            {
                return new String(this.nickname);
            }
            set
            {
                this.nickname = value.ToCharArray();
            }
        }

        public User()
        {
            this.anid = new char[32];
            this.nickname = new char[12];
        }

        public User(char[] anid, char[] nickname)
        {
            this.anid = anid;
            this.nickname = nickname;
        }

        public User(String anid, String nickname)
        {
            this.Anid = anid;
            this.Nickname = nickname;
        }
    }
}
