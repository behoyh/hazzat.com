using PanoramaApp1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PanoramaApp1.HazzatService;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Diagnostics;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight.Messaging;

namespace Coptic_Hymns
{

    public class HymnStructNameViewModel : INotifyPropertyChanged
    {

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        private string _season;
        public string Season
        {
            get
            {
                return _season;
            }
            set
            {
                if (value != _season)
                {
                    _season = value;
                    RaisePropertyChanged("Season");
                }
            }
        }

        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    RaisePropertyChanged("Content");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ByNameMainViewModel
    {



        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>

        public ObservableCollection<HymnStructNameViewModel> Hymns { get; private set; }

        public ObservableCollection<PanoramaApp1.HazzatService.StructureInfo> HymnsBySeason { get; private set; }

        public ObservableCollection<PanoramaApp1.HazzatService.ServiceHymnInfo> HazzatHymns { get; private set; }

        public ByNameMainViewModel()
        {
            Hymns = new ObservableCollection<HymnStructNameViewModel>();
        }


        public bool IsDataLoaded
        {
            get;
            private set;
        }


        public void createViewModel()
        {
            HymnStructNameViewModel view = new HymnStructNameViewModel();

            this.Hymns.Add(new HymnStructNameViewModel() { Name = "Very Early Sunday Morning", Season = "Resurrection", Content = "LALALALALA, LAAAAAAA LALALA ALALALA ALALALA LA." });
            this.Hymns.Add(new HymnStructNameViewModel() { Name = "Jesus Loves Me", Season = "Normal", Content = "LALALALALA, LAAAAAAA LALALA ALALALA ALALALA LA." });
            this.Hymns.Add(new HymnStructNameViewModel() { Name = "Truly Risen Is The King Of Peace", Season = "Nativity", Content = "LALALALALA, LAAAAAAA LALALA ALALALA ALALALA LA." });
            this.Hymns.Add(new HymnStructNameViewModel() { Name = "Jesus Is Risen", Season = "Resurrection", Content = "LALALALALA, LAAAAAAA LALALA ALALALA ALALALA LA." });
            this.Hymns.Add(new HymnStructNameViewModel() { Name = "The Last Supper", Season = "Easter", Content = "LALALALALA, LAAAAAAA LALALA ALALALA ALALALA LA." });
          

            IsDataLoaded = true;
        }


        public void createViewModelBySeason(int Season)
        {
            Messenger.Default.Send(new StatusMessage("Loading", 0));

            HymnsBySeason = new ObservableCollection<PanoramaApp1.HazzatService.StructureInfo>();

            try
            {
                PanoramaApp1.HazzatService.HazzatWebServiceSoapClient client = new PanoramaApp1.HazzatService.HazzatWebServiceSoapClient();
                client.GetSeasonServicesCompleted += new EventHandler<PanoramaApp1.HazzatService.GetSeasonServicesCompletedEventArgs>(GetCompletedStructBySeason);

                client.GetSeasonServicesAsync(Season);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        public void GetCompletedStructBySeason(object sender, PanoramaApp1.HazzatService.GetSeasonServicesCompletedEventArgs e)
        {
            HymnsBySeason = e.Result;
            Messenger.Default.Send(new StatusMessage("Done", 0));
        }


        public void createViewModelHymns(int StructId)
        {

            Messenger.Default.Send(new StatusMessage("Loading", 0));

            HazzatHymns = new ObservableCollection<PanoramaApp1.HazzatService.ServiceHymnInfo>();

             try
            {
                PanoramaApp1.HazzatService.HazzatWebServiceSoapClient client = new PanoramaApp1.HazzatService.HazzatWebServiceSoapClient();
                client.GetSeasonServiceHymnsCompleted += new EventHandler<PanoramaApp1.HazzatService.GetSeasonServiceHymnsCompletedEventArgs>(GetCompletedHymnsBySeason);

                client.GetSeasonServiceHymnsAsync(StructId);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        public void GetCompletedHymnsBySeason(object sender, PanoramaApp1.HazzatService.GetSeasonServiceHymnsCompletedEventArgs e)
        {
            HazzatHymns = e.Result;
            Messenger.Default.Send(new StatusMessage("Done", 0));
        }
        

    }
}
public class BySeasonHymnInfoHymnMainViewModel 
{

    /// <summary>
    /// A collection for ItemViewModel objects.
    /// </summary>
    public ObservableCollection<PanoramaApp1.HazzatService.SeasonInfo> Seasons { get; private set; }

    public ObservableCollection<PanoramaApp1.HazzatService.ServiceHymnsContentInfo> HymnContentInfo { get; private set; }

    public BySeasonHymnInfoHymnMainViewModel()
    {
        Seasons = new ObservableCollection<PanoramaApp1.HazzatService.SeasonInfo>();
    }

    public bool IsDataLoaded
    {
        get;
        set;
    }

    public void createViewModel()
    {
        Messenger.Default.Send(new StatusMessage("Loading", 0));

        try
        {
            PanoramaApp1.HazzatService.HazzatWebServiceSoapClient client = new PanoramaApp1.HazzatService.HazzatWebServiceSoapClient();
            client.GetSeasonsCompleted += new EventHandler<PanoramaApp1.HazzatService.GetSeasonsCompletedEventArgs>(client_GetCompleted);

            client.GetSeasonsAsync(true);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    }
    public void client_GetCompleted(object sender, PanoramaApp1.HazzatService.GetSeasonsCompletedEventArgs e)
    {
        Seasons = e.Result;
        IsDataLoaded = true;
        Messenger.Default.Send(new StatusMessage("Done", 0));
    }

    public void createHymnTextViewModel(int itemId)
    {
        Messenger.Default.Send(new StatusMessage("Loading", 0));

        try
        {
            PanoramaApp1.HazzatService.HazzatWebServiceSoapClient client = new PanoramaApp1.HazzatService.HazzatWebServiceSoapClient();
            client.GetSeasonServiceHymnTextCompleted += new EventHandler<PanoramaApp1.HazzatService.GetSeasonServiceHymnTextCompletedEventArgs>(client_GetCompletedHymnInfo);

            client.GetSeasonServiceHymnTextAsync(itemId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    }


    public void client_GetCompletedHymnInfo(object sender, PanoramaApp1.HazzatService.GetSeasonServiceHymnTextCompletedEventArgs e)
    {
        HymnContentInfo = e.Result;
        IsDataLoaded = true;
        Messenger.Default.Send(new StatusMessage("DoneWithContent", 0));  
    }


  
}