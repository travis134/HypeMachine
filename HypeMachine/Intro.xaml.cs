using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Advertising.Mobile.UI;

namespace HypeMachine
{
    public partial class Intro : PhoneApplicationPage
    {
        public Intro()
        {
            InitializeComponent();

            AdControl hypeMachineAd = new AdControl() { ApplicationId = "4d3667ee-44c1-494c-929d-5e661b011918", AdUnitId = "10019312" };
            LayoutRoot.Children.Add(hypeMachineAd);
            Grid.SetRow(hypeMachineAd, 1);

            Assets.Instance.Create("IntroBackground", new BitmapImage(new Uri("backgrounds/IntroBackground.png", UriKind.Relative)));
            Assets.Instance.Create("IntroBackgroundDark", new BitmapImage(new Uri("backgrounds/IntroBackgroundDark.png", UriKind.Relative)));

            if (((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible))
            {
                mainView.Background = new ImageBrush() { ImageSource = Assets.Instance.ReadBitmapImage("IntroBackgroundDark"), Stretch = Stretch.None };
                mainView.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                mainView.Background = new ImageBrush() { ImageSource = Assets.Instance.ReadBitmapImage("IntroBackground"), Stretch = Stretch.None };
                mainView.Foreground = new SolidColorBrush(Colors.Black);
            }
            this.Loaded += new RoutedEventHandler(Intro_Loaded) ;
        }

        private void Intro_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}