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

        private char[] deviceUniqueId;
        [DataMember(Name = "DeviceUniqueId")]
        public String DeviceUniqueId
        {
            get
            {
                return new String(this.deviceUniqueId);
            }
            set
            {
                this.deviceUniqueId = value.ToCharArray();
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
            this.deviceUniqueId = new char[20];
            this.nickname = new char[12];
        }

        public User(char[] deviceUniqueId, char[] nickname)
        {
            this.deviceUniqueId = deviceUniqueId;
            this.nickname = nickname;
        }

        public User(String deviceUniqueId, String nickname)
        {
            this.DeviceUniqueId = deviceUniqueId;
            this.Nickname = nickname;
        }
    }
}
