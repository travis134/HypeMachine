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
    public sealed class Assets : AssetHelper
    {
        private static volatile Assets instance;
        private static object syncRoot = new Object();

        private Assets() : base() { }

        public static Assets Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Assets();
                    }
                }

                return instance;
            }
        }

    }
}
