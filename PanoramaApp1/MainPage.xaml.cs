using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using Coptic_Hymns;
using System.Windows.Media;
using System.ServiceModel.Channels;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using System.Windows.Media.Imaging;

namespace PanoramaApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.NameViewModel;

            App.CurrentLanguage = App.HymnLanguage.English;

            myImg.ImageSource = RandomImg();
        }

        public BitmapImage RandomImg()
        {
            BitmapImage img;
            Random choice = new Random();
            int c = choice.Next();
            c = c % 15;
            if (c == 0)
            {
                img = new BitmapImage(new Uri("imgs/p2.jpg", UriKind.Relative));
            }
            else if (c == 1)
            {
                img = new BitmapImage(new Uri("imgs/p32.jpg", UriKind.Relative));
            }
            else if (c == 2)
            {
                img = new BitmapImage(new Uri("imgs/cross2.png", UriKind.Relative));
            }
            else if (c == 3)
            {
                img = new BitmapImage(new Uri("imgs/copts.jpg", UriKind.Relative));
            }
            else if (c == 4)
            {
                img = new BitmapImage(new Uri("imgs/m1.jpg", UriKind.Relative));
            }
            else if (c == 5)
            {
                img = new BitmapImage(new Uri("imgs/1.jpg", UriKind.Relative));
            }
            else if (c == 6)
            {
                img = new BitmapImage(new Uri("imgs/2.jpg", UriKind.Relative));
            }
            else if (c == 7)
            {
                img = new BitmapImage(new Uri("imgs/3.jpg", UriKind.Relative));
            }
            else if (c == 8)
            {
                img = new BitmapImage(new Uri("imgs/4.jpg", UriKind.Relative));
            }
            else if (c == 9)
            {
                img = new BitmapImage(new Uri("imgs/5.jpg", UriKind.Relative));
            }
            else if (c == 10)
            {
                img = new BitmapImage(new Uri("imgs/6.jpg", UriKind.Relative));
            }
            else if (c == 11)
            {
                img = new BitmapImage(new Uri("imgs/7.jpg", UriKind.Relative));
            }
            else if (c == 12)
            {
                img = new BitmapImage(new Uri("imgs/8.jpg", UriKind.Relative));
            }
            else if (c == 13)
            {
                img = new BitmapImage(new Uri("imgs/9.jpg", UriKind.Relative));
            }
            else if (c == 14)
            {
                img = new BitmapImage(new Uri("imgs/10.jpg", UriKind.Relative));
            }
            else
            {
                img = new BitmapImage(new Uri("imgs/m1.jpg", UriKind.Relative));
            }
            return img;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.NameViewModel.IsDataLoaded)
            {
                App.NameViewModel.createViewModel();
            }
            Messenger.Default.Register<StatusMessage>(
          this, HandleStatusMessage);
            base.OnNavigatedTo(e);
        }

        private void PopulateHymnsList(object sender, RoutedEventArgs e)
        {
            RadDataBoundListBox list = (RadDataBoundListBox)sender;

            list.ItemsSource = App.NameViewModel.Hymns;
            //need to put in check here to see if ObservableCollection is null or empty
            list.BringIntoView(App.NameViewModel.Hymns.First());
        }

        private void PopulateSeasonsList(object sender, RoutedEventArgs e)
        {
            LongListSelector grid = (LongListSelector)sender;

            if (!App.SeasonViewModel.IsDataLoaded)
            {
                App.SeasonViewModel.createViewModel();
            }
            grid.ItemsSource = App.SeasonViewModel.Seasons;
        }

        private void SelectedHymnByName(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Feature still being considered/revised. Check back after an app update!");
        }

        private void HymnsBySeason(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (BySeasonGrid.SelectedItem == null)
                return;

            var item = BySeasonGrid.SelectedItem as PanoramaApp1.HazzatService.SeasonInfo;

            App.NameViewModel.createViewModelBySeason(item.ItemId);

            NavigationService.Navigate(new Uri("/SectionPage.xaml#"+item.Name, UriKind.Relative));

            BySeasonGrid.SelectedItem = null;
        }

      
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Messenger.Default.Unregister<StatusMessage>(
              this, HandleStatusMessage);
            base.OnNavigatingFrom(e);
        }
        private void HandleStatusMessage(StatusMessage msg)
        {
            if (msg.Status.Equals("Done"))
            {
                this.BySeasonGrid.ItemsSource = App.SeasonViewModel.Seasons;
                if (App.SeasonViewModel.Seasons.Count == 0)
                {
                    MessageBox.Show("No Items Found. Contact the Admin of hazzat.com to request this section be added.");
                }
                Waiter.IsRunning = false;
            }

        }

        private void Reload(object sender, EventArgs e)
        {
            System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 2000); // 500 Milliseconds
            dt.Tick += new EventHandler(TimerLoad);
            dt.Start();
        }

        private void TimerLoad(object sender, EventArgs e)
        {
            this.ByNameList.ItemsSource = App.NameViewModel.Hymns;
            this.BySeasonGrid.ItemsSource = App.SeasonViewModel.Seasons;
            this.ByNameList.StopPullToRefreshLoading(true);
        }

    }
}