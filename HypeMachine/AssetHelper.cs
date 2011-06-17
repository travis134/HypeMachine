using System;
using System.Net;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace HypeMachine
{
    public class AssetHelper
    {
        private Dictionary<String, Object> assets;

        public AssetHelper() { assets = new Dictionary<String, Object>(); }

        public Boolean Create(String name, object asset)
        {
            Boolean result = false;
            if (!assets.ContainsKey(name))
            {
                assets.Add(name, asset);
                result = true;
            }
            return result;
        }

        public Boolean Read(String name, out object asset)
        {
            return assets.TryGetValue(name, out asset);
        }

        public BitmapImage ReadBitmapImage(String name)
        {
            return (BitmapImage)assets[name];
        }

        public Boolean Update(String name, object asset)
        {
            Boolean result = false;
            if (assets.ContainsKey(name))
            {
                assets[name] = asset;
                result = true;
            }
            return result;
        }

        public Boolean Delete(String name)
        {
             Boolean result = false;
             if (assets.ContainsKey(name))
             {
                 assets.Remove(name);
                 result = true;
             }
             return result;
        }
    }
}
