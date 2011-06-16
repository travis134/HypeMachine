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
    [DataContract(Name = "Aftermath")]
    public class Aftermath : Rating
    {
        public Aftermath() : base() { }
        public Aftermath(uint id, uint gameId, uint userId, Boolean score) : base(id, gameId, userId, score) { }
        public override string ToString()
        {
            return String.Format("Aftermath:\n{0}", base.ToString());
        }
    }
}
