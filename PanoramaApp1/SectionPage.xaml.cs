using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using PanoramaApp1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;


// The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Coptic_Hymns
{
    public sealed partial class SectionPage : PhoneApplicationPage
    {
        private string SeasonName { get; set; }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            SeasonName = e.Fragment;
        }

        private void LoadHeader(object sender, RoutedEventArgs e)
        {
            TextBlock text = (TextBlock) sender;
            text.Text = SeasonName;
        }

        public SectionPage()
        {
            this.InitializeComponent();
            DataContext = App.NameViewModel;
        }

        private void SelectedHymnByName(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (itemListView.SelectedItem == null)
                return;

            var item = itemListView.SelectedItem as PanoramaApp1.HazzatService.StructureInfo;

            if (item != null) { 

            App.NameViewModel.createViewModelHymns(item.ItemId);

            NavigationService.Navigate(new Uri("/ItemPage2.xaml", UriKind.Relative));

            itemListView.SelectedItem = null;

        }
        }

        private void SettingsClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Register<StatusMessage>(
          this, HandleStatusMessage);
            base.OnNavigatedTo(e);
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
                itemListView.ItemsSource = App.NameViewModel.HymnsBySeason;
                //need to put in check here to see if ObservableCollection is null or empty
                if (App.NameViewModel.HymnsBySeason.Count > 0)
                {
                    itemListView.BringIntoView(App.NameViewModel.HymnsBySeason.First());
                    this.itemListView.StopPullToRefreshLoading(true);
                }
                else if (App.NameViewModel.HymnsBySeason.Count == 0)
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
            this.itemListView.ItemsSource = App.NameViewModel.HymnsBySeason;
            this.itemListView.StopPullToRefreshLoading(true);
        }
    }
}
