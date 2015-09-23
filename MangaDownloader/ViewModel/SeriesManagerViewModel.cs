using MangaScraper.Models;
using ScFix.Utility.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaScraper.ViewModel
{
    public class SeriesManagerViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<SeriesModel> _Series = new ObservableCollection<SeriesModel>();
        ObservableCollection<SeriesModel> Series
        {
            get
            {
                return _Series;
            }
        }
        #endregion //Properties

        #region Constructor
        public SeriesManagerViewModel()
        {
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
            bgWorker.RunWorkerAsync();
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        #endregion //Constructor
    }
}
