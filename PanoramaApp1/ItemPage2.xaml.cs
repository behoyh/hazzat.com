using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PanoramaApp1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;

// The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Coptic_Hymns
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summa
    public sealed partial class ItemPage2 : PhoneApplicationPage
    {
        private HymnStructNameViewModel defaultViewModel = new HymnStructNameViewModel();

        private ExpanderView Expander { get; set; }

        private string TimeName { get; set; }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            TimeName = e.Fragment;
        }


        public ItemPage2()
        {
            this.InitializeComponent();
            this.InitializeApplicationBar();
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



                if (App.NameViewModel.HazzatHymns.Count > 0)
                {
                    itemListView.ItemsSource = App.NameViewModel.HazzatHymns;
                    PopText.Text = App.NameViewModel.HazzatHymns[0].Structure_Name;
                    itemListView.BringIntoView(App.NameViewModel.HazzatHymns.First());
                }
                else if (App.NameViewModel.HazzatHymns.Count == 0)
                {
                    MessageBox.Show("No Items Found. Contact the Admin of hazzat.com to request this section be added.");
                }

                Waiter.IsRunning = false;
            }

            else if (msg.Status.Equals("DoneWithContent"))
            {

                if (App.SeasonViewModel.HymnContentInfo.Count > 0)
                {
                    if (Expander != null)
                    {
                        Expander.Items.Clear();

                        IList<string> splitText = null;
                        bool isCoptic = false;
                        bool isArabic = false;

                        if (App.CurrentLanguage == App.HymnLanguage.English)
                        {
                            splitText = TextBlockFormatterEngine.Instance.Split(App.SeasonViewModel.HymnContentInfo[0].Content_English, 100, FontWeights.Normal, Application.Current.Host.Content.ActualWidth);
                        }

                        if (App.CurrentLanguage == App.HymnLanguage.Arabic)
                        {
                            splitText = TextBlockFormatterEngine.Instance.Split(App.SeasonViewModel.HymnContentInfo[0].Content_Arabic, 100, FontWeights.Normal, Application.Current.Host.Content.ActualWidth);
                            isArabic = true;
                        }

                        if (App.CurrentLanguage == App.HymnLanguage.Coptic)
                        {
                            splitText = TextBlockFormatterEngine.Instance.Split(App.SeasonViewModel.HymnContentInfo[0].Content_Coptic, 100, FontWeights.Normal, Application.Current.Host.Content.ActualWidth);
                            isCoptic = true;
                        }

                        if (splitText.Count == 1)
                        {
                            if (String.IsNullOrEmpty(splitText[0]))
                            {
                                splitText[0] = "{No Content Found In This Language.}";
                            }
                        }

                        UpdateUI(isCoptic, isArabic, splitText);

                    }
                    Waiter.IsRunning = false;
                }
                else { MessageBox.Show("This Hymn Was Not Found..."); Waiter.IsRunning = false; }
            }
        }

        private void UpdateUI(bool isCoptic, bool isArabic, IList<string> splitText)
        {
            Expander.Items.Add(new ScrollViewer { });

            ScrollViewer vier = (ScrollViewer)Expander.Items[0];
            vier.IsEnabled = false;
            StackPanel panel = new StackPanel();


            foreach (string text in splitText)
            {
                if (!isCoptic && !isArabic)
                {
                    panel.Children.Add(new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap, FontSize = 22, Margin = new Thickness(0, 0, 10, 0) });
                }
                else if (isCoptic)
                {
                    panel.Children.Add(new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap, FontSize = 24, Margin = new Thickness(0, 0, 10, 0), FontFamily = new FontFamily(@"\Assets\CS Copt.ttf#CS Copt") });
                }
                else if (isArabic)
                {
                    panel.Children.Add(new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap, FontSize = 22, Margin = new Thickness(0, 0, 10, 0), TextAlignment = TextAlignment.Right});
                }
            }
            vier.Content = panel;
            Expander.UpdateLayout();
        }



        private void SettingsClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void SelectedItem(object sender, SelectionChangedEventArgs e)
        {
                if (itemListView.SelectedItem == null)
                    return;
        }


        private void FocusExpanderView(object sender, ListBoxItemTapEventArgs e)
        {           
                if (!Waiter.IsRunning)
                {
                    Waiter.IsRunning = true;

                    Expander = e.Item.FindChildByType<ExpanderView>();

                    var item = itemListView.SelectedItem as PanoramaApp1.HazzatService.ServiceHymnInfo;

                    itemListView.SelectedItem = null;

                    App.SeasonViewModel.createHymnTextViewModel(item.ItemId);
                }
        }


        private void ChangeGlobalLanguage(object sender, EventArgs e)
        {
            ApplicationBarIconButton appbarbtn = (ApplicationBarIconButton)sender;

            if (appbarbtn.Text.Equals("arabic"))
            {
                App.CurrentLanguage = App.HymnLanguage.Coptic;
                appbarbtn.Text = "coptic";
                appbarbtn.IconUri = new Uri("/Assets/C.png", UriKind.Relative);
            }

            else if (appbarbtn.Text.Equals("coptic"))
            {
                App.CurrentLanguage = App.HymnLanguage.English;
                appbarbtn.Text = "english";
                appbarbtn.IconUri = new Uri("/Assets/E.png", UriKind.Relative);
            }

            else if (appbarbtn.Text.Equals("english"))
            {
                App.CurrentLanguage = App.HymnLanguage.Arabic;
                appbarbtn.Text = "arabic";
                appbarbtn.IconUri = new Uri("/Assets/A.png", UriKind.Relative);
            }
        }

        protected void InitializeApplicationBar()
        {
            String IconUri = "";
            String Text = "";

            // Set the page's ApplicationBar to a new instance of ApplicationBar
            ApplicationBar = new ApplicationBar { IsVisible = true, IsMenuEnabled = false };

            if (App.CurrentLanguage == App.HymnLanguage.Arabic)
            {
                Text = "arabic";
                IconUri = "/Assets/A.png";
            }

            else if (App.CurrentLanguage == App.HymnLanguage.Coptic)
            {
                Text = "coptic";
                IconUri = "/Assets/C.png";
            }

            else if (App.CurrentLanguage == App.HymnLanguage.English)
            {
                Text = "english";
                IconUri = "/Assets/E.png";
            }

            // Create a new button and set the text value to the localized string from AppResources
            var appbarSaveProject = new ApplicationBarIconButton(new Uri(IconUri, UriKind.Relative)) { Text = Text };
            appbarSaveProject.Click += ChangeGlobalLanguage;
            ApplicationBar.Buttons.Add(appbarSaveProject);
        }
    }
}
