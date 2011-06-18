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


            Assets.Instance.Create("robot", new BitmapImage(new Uri("intro/robot.png", UriKind.Relative)));

            List<Entry> firstPageEntries = new List<Entry>()
            {
                new Entry("WHAT?", "hype machine is a simple app that lets you see which upcoming video games people are most excited about.", "robot"),
                new Entry("WHY?", "with new titles coming out all the time, it's important that we gamers work together to seperate the great from the 'OH GOD NO!'.", "robot"),
                new Entry("WHO?", "you! well, not only you, infact there are many other users for you to interact with... but you're our favorite!.", "robot")
            };
            Section firstPage = new Section("SUP NOOB!", "thanks for downloading.", firstPageEntries, (Color)Resources["PhoneForegroundColor"], (Color)Resources["PhoneBackgroundColor"], true);

            List<Entry> secondPageEntries = new List<Entry>()
            {
                new Entry("WHAT?", "hype machine is a simple app that lets you see which upcoming video games people are most excited about.", "robot"),
                new Entry("WHY?", "with new titles coming out all the time, it's important that we gamers work together to seperate the great from the 'OH GOD NO!'.", "robot"),
                new Entry("WHO?", "you! well, not only you, infact there are many other users for you to interact with... but you're our favorite!.", "robot")
            };
            Section secondPage = new Section("HYPE", "equivalent to 4 redbulls.", secondPageEntries, (Color)Resources["PhoneBackgroundColor"], (Color)Resources["PhoneForegroundColor"], false);

            List<Entry> thirdPageEntries = new List<Entry>()
            {
                new Entry("WHAT?", "hype machine is a simple app that lets you see which upcoming video games people are most excited about.", "robot"),
                new Entry("WHY?", "with new titles coming out all the time, it's important that we gamers work together to seperate the great from the 'OH GOD NO!'.", "robot"),
                new Entry("WHO?", "you! well, not only you, infact there are many other users for you to interact with... but you're our favorite!.", "robot")
            };
            Section thirdPage = new Section("AFTERMATH", "equivalent to 4 redbulls.", thirdPageEntries, (Color)Resources["PhoneForegroundColor"], (Color)Resources["PhoneBackgroundColor"], true);

            List<Entry> fourthPageEntries = new List<Entry>()
            {
                new Entry("WHAT?", "hype machine is a simple app that lets you see which upcoming video games people are most excited about.", "robot"),
                new Entry("WHY?", "with new titles coming out all the time, it's important that we gamers work together to seperate the great from the 'OH GOD NO!'.", "robot"),
                new Entry("WHO?", "you! well, not only you, infact there are many other users for you to interact with... but you're our favorite!.", "robot")
            };
            Section fourthPage = new Section("COMMENTS", "equivalent to 4 redbulls.", fourthPageEntries, (Color)Resources["PhoneBackgroundColor"], (Color)Resources["PhoneForegroundColor"], false);


            mainView.Items.Add(panoramaItemFromSection(firstPage));
            mainView.Items.Add(panoramaItemFromSection(secondPage));
            mainView.Items.Add(panoramaItemFromSection(thirdPage));
            mainView.Items.Add(panoramaItemFromSection(fourthPage));

            this.Loaded += new RoutedEventHandler(Intro_Loaded) ;
        }

        private void Intro_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private class Entry
        {
            public String Title { get; set; }
            public String Content { get; set; }
            public String ImageAssetKey { get; set; }
            public Entry(String title, String content, String imageAssetKey)
            {
                this.Title = title;
                this.Content = content;
                this.ImageAssetKey = imageAssetKey;
            }
        }

        private class Section
        {
            public String Header { get; set; }
            public String Caption { get; set; }
            public List<Entry> Entries { get; set; }
            public Color DefaultColor { get; set; }
            public Color AlternateColor { get; set; }
            public Boolean AlternateSeed { get; set; }
            public Section(String header, String caption, List<Entry> entries, Color defaultColor, Color alternateColor, Boolean alternateSeed)
            {
                this.Header = header;
                this.Caption = caption;
                this.Entries = entries;
                this.DefaultColor = defaultColor;
                this.AlternateColor = alternateColor;
                this.AlternateSeed = alternateSeed;
            }
        }

        private PanoramaItem panoramaItemFromSection(Section section)
        {
            PanoramaItem panoramaItem = new PanoramaItem();
            ScrollViewer scrollViewer = new ScrollViewer();
            panoramaItem.Content = scrollViewer;
            Grid grid = new Grid() { Margin = new Thickness(0, 10, 0, 0) };

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            TextBlock headerText = new TextBlock() { Text = section.Header, FontFamily = new FontFamily("Segoe WP Black"), FontSize = 65, Foreground = new SolidColorBrush((Color)Resources["PhoneForegroundColor"]), HorizontalAlignment = System.Windows.HorizontalAlignment.Left };
            Grid.SetRow(headerText, 0);
            grid.Children.Add(headerText);

            TextBlock captionText = new TextBlock() { Text = section.Caption, FontFamily = new FontFamily("Segoe WP Black"), FontSize = 20, Foreground = new SolidColorBrush((Color)Resources["PhoneForegroundColor"]), HorizontalAlignment = System.Windows.HorizontalAlignment.Right};
            Grid.SetRow(captionText, 1);
            grid.Children.Add(captionText);

            Boolean alternate = section.AlternateSeed;
            
            for (int i = 0; i < section.Entries.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid.Children.Add(rowFromEntry(section.Entries[i], 2+i, alternate, section.DefaultColor, section.AlternateColor));
                alternate = !alternate;
            }

            scrollViewer.Content = grid;

            return panoramaItem;
        }

        private Border rowFromEntry(Entry entry, int rowNumber, Boolean alternate, Color defaultColor, Color alternateColor)
        {
            Border row = new Border() { Margin = new Thickness(0, 0, 0, 10) };
            Grid.SetRow(row, rowNumber);

            Grid padding = new Grid();
            padding.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });
            padding.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto } );
            padding.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });
            
            padding.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10) });
            padding.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            padding.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10) });

            row.Child = padding;

            Grid container = new Grid();
            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            
            Grid.SetRow(container, 1);
            Grid.SetColumn(container, 1);

            padding.Children.Add(container);

            Grid header = new Grid();
            header.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            ColumnDefinition leftColumn = new ColumnDefinition();
            ColumnDefinition rightColumn = new ColumnDefinition();

            header.ColumnDefinitions.Add(leftColumn);
            header.ColumnDefinitions.Add(rightColumn);

            Grid.SetRow(header, 0);

            container.Children.Add(header);

            Image image = new Image() { Width = 142, Height = 129, Source = Assets.Instance.ReadBitmapImage(entry.ImageAssetKey) };

            Grid.SetRow(image, 0);

            TextBlock titleText = new TextBlock() { Text = entry.Title, FontFamily = new FontFamily("Segoe WP Black"), FontSize = 55, VerticalAlignment = System.Windows.VerticalAlignment.Bottom, TextWrapping = TextWrapping.Wrap };

            Grid.SetRow(titleText, 0);

            header.Children.Add(image);
            header.Children.Add(titleText);

            TextBlock contentText = new TextBlock() { Text = entry.Content, FontFamily = new FontFamily("Segoe WP Black"), FontSize = 20, TextWrapping = TextWrapping.Wrap };

            Grid.SetRow(contentText, 1);

            container.Children.Add(contentText);

            if (alternate)
            {
                row.Background = new SolidColorBrush((Color)Resources["PhoneAccentColor"]);
                leftColumn.Width = new GridLength(1, GridUnitType.Star);
                rightColumn.Width = new GridLength(142);
                Grid.SetColumn(image, 1);
                titleText.Foreground = new SolidColorBrush((Color)Resources["PhoneForegroundColor"]);
                titleText.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                Grid.SetColumn(titleText, 0);
                contentText.Foreground = new SolidColorBrush((Color)Resources["PhoneForegroundColor"]);
                contentText.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            }
            else
            {
                row.Background = new SolidColorBrush(defaultColor);
                leftColumn.Width = new GridLength(142);
                rightColumn.Width = new GridLength(1, GridUnitType.Star);
                Grid.SetColumn(image, 0);
                titleText.Foreground = new SolidColorBrush(alternateColor);
                titleText.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                Grid.SetColumn(titleText, 1);
                contentText.Foreground = new SolidColorBrush(alternateColor);
                contentText.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }

            return row;
        }
    }
}