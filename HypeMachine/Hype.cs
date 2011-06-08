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
    public class Hype : Rating
    {
        public Hype() : base() { }
        public Hype(uint id, uint gameId, uint userId, Boolean score) : base(id, gameId, userId, score) { }
        public override string ToString()
        {
            return String.Format("Hype:\n{0}", base.ToString());
        }
    }
}
