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
    public sealed class Storage : StorageHelper<Game>
    {
        private static volatile Storage instance;
        private static object syncRoot = new Object();

        private Storage(String fileName, String lastModifiedFileName) : base(fileName, lastModifiedFileName) { }

        public static Storage Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Storage("HypeMachine.xml", "Stale.txt");
                    }
                }

                return instance;
            }
        }

    }
}
