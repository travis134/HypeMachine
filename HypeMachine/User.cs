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
    public class User
    {
        public uint Id { get; set; }
        private char[] deviceUniqueId;
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
