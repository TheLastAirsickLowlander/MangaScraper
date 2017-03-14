using ScFix.Utility.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper.ViewModel
{
    public class ImageViewModel : ViewModelBase
    {

        #region FileName
        protected String _FileName;
        public String FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                if (value != _FileName)
                {
                    _FileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }
        #endregion //FileName

        #region Link
        protected String _Link;
        public String Link
        {
            get
            {
                return _Link;
            }
            set
            {
                if (value != _Link)
                {
                    int s = value.LastIndexOf("/") + 1;
                    int l = value.Length - s;
                    FileName = value.Substring(s, l);
                    _Link = value;
                    OnPropertyChanged("Link");
                }
            }
        }
        #endregion //Link

    }
}
