using System.IO.IsolatedStorage;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace HypeMachine
{
    public static class Status
    {
        public static Boolean FirstRun()
        {
            Boolean result = true;
            object tempFirstRun;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("notFirstRun", out tempFirstRun))
            {
                result = false;
            }
            return result;
        }

        public static void EndTutorial()
        {
            object tempFirstRun;
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue("notFirstRun", out tempFirstRun))
            {
                IsolatedStorageSettings.ApplicationSettings.Add("notFirstRun", true);
            }
        }

        public static Boolean StorageExists()
        {
            return Storage.Instance.Exists();
        }

        public static Boolean StorageIsStale(TimeSpan shelfLife)
        {
            return Storage.Instance.IsStale(shelfLife);
        }

        public static Boolean InternetConnection()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
