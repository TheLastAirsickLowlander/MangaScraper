using MangaScraper.Utilties;
using ScFix.Utility.Classes;
using ScFix.Utility.ViewModels;
using ScFix.Utility.WebUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MangaScraper.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private String _FolderName;

        public String FolderName
        {
            get { return _FolderName; }
            set
            {
                _FolderName = value;
                OnPropertyChanged("FolderName");
            }
        }

        private string _WebBaseAddress;

        public string WebBaseAddress
        {
            get { return _WebBaseAddress; }
            set
            {
                _WebBaseAddress = value;
                OnPropertyChanged("WebBasedAddress");
            }
        }


        #region Members
        BackgroundWorker _bgWorker = null;
        #endregion  

        #region BaseAddress
        protected String _BaseAddress;
        public String BaseAddress
        {
            get
            {
                return _BaseAddress;
            }
            set
            {
                if (value != _BaseAddress)
                {
                    _BaseAddress = value;
                    OnPropertyChanged("BaseAddress");
                }
            }
        }
        #endregion //BaseAddress


        #region Find Links Commmand
        private ICommand _FindLinks;
        public ICommand FindLinks
        {
            get
            {
                if (_FindLinks == null)
                {
                    _FindLinks = new RelayCommand(FindLinkExecute, CanFindLink);
                }
                return _FindLinks;
            }
        }

        private bool CanFindLink(object obj)
        {
            return true;
        }

        private void FindLinkExecute(object obj)
        {
            BaseAddress = WebBaseAddress.Substring(0, WebBaseAddress.IndexOf(".com") + 4);

            Debug.WriteLine("Finding links for the page: " + WebBaseAddress);
            WebManager.GetGeneratedHTML(WebBaseAddress, renderedHtmlCallback, "</table>");
            Debug.WriteLine("Waiting for the page to load");

        }
        #endregion //Find Links Command


        #region ClearList
        protected ICommand _ClearList = null;
        public ICommand ClearList
        {
            get
            {
                if (_ClearList == null)
                {
                    _ClearList = new RelayCommand(ClearListExecute, CanClearList);
                }
                return _ClearList;
            }
        }

        private bool CanClearList(object obj)
        {
            return true;
        }

        private void ClearListExecute(object obj)
        {
            //TODO Implement the ClearList Method
            Debug.WriteLine("ClearList Executed");
            _ChapterLinks.Clear();
            ChapterLinks = null;
        }
        #endregion //ClearList

        #region ChapterLinks
        protected IList<ChapterViewModel> _ChapterLinks;
        public IList<ChapterViewModel> ChapterLinks
        {
            get
            {
                return _ChapterLinks;
            }
            set
            {
                if (value != _ChapterLinks)
                {
                    _ChapterLinks = value;
                    OnPropertyChanged("ChapterLinks");
                }
            }
        }
        #endregion //ChapterLinks

        #region GenerateNames
        protected ICommand _GenerateNames = null;
        public ICommand GenerateNames
        {
            get
            {
                if (_GenerateNames == null)
                {
                    _GenerateNames = new RelayCommand(GenerateNamesExecute, CanGenerateNames);
                }
                return _GenerateNames;
            }
        }

        private bool CanGenerateNames(object obj)
        {
            return true;
        }

        private void GenerateNamesExecute(object obj)
        {
            //TODO Implement the GenerateNames Method
            Debug.WriteLine("GenerateNames Executed");
            if (ChapterLinks != null)
            {
                for (int i = ChapterLinks.Count - 1; i >= 0; i--)
                {
                    var link = ChapterLinks[i];
                    //kicks off another background thread for that processing.
                    int x = ChapterLinks.Count - i;
                    string num = "";
                    if (x < 9)
                    {
                        num = "00" + x;
                    }
                    else if (x < 99)
                    {
                        num = "0" + x;
                    }
                    else
                    {
                        num = x + "";
                    }

                    link.ChapterName = "Ch-" + num;
                }
            }
        }
        #endregion //GenerateNames

        #region ProcessLinks
        protected ICommand _ProcessLinks = null;
        public ICommand ProcessLinks
        {
            get
            {
                if (_ProcessLinks == null)
                {
                    _ProcessLinks = new RelayCommand(ProcessLinksExecute, CanProcessLinks);
                }
                return _ProcessLinks;
            }
        }

        private bool CanProcessLinks(object obj)
        {
            return true;
        }

        private void ProcessLinksExecute(object obj)
        {
            //TODO Implement the ProcessLinks Method
            Debug.WriteLine("ProcessLinks Executed");
            foreach (var link in ChapterLinks)
            {
                link.Status = ChapterProcessingStatus.Waiting;
            }

            if (_bgWorker != null)
            {
                _bgWorker.CancelAsync();
                _bgWorker = null;
            }
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += processAllLinks;
            _bgWorker.RunWorkerCompleted += AllLinksProcessed;
            _bgWorker.RunWorkerAsync();

        }

        private void AllLinksProcessed(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("Finished Processing all links");
        }

        private void processAllLinks(object sender, DoWorkEventArgs e)
        {
            for (int i = ChapterLinks.Count - 1; i >= 0; i--)
            {
                var link = ChapterLinks[i];
                //kicks off another background thread for that processing.
                link.ProcessLinkWebPage(BaseAddress);
                while (link.Status != ChapterProcessingStatus.Done)
                {
                    Thread.Sleep(1000);
                }
            }
        }
        #endregion //ProcessLinks


        #region Methods
        private void renderedHtmlCallback(String body)
        {
            Debug.WriteLine("img");
            ChapterLinks = getChapterLinks(body);
        }

        private IList<ChapterViewModel> getChapterLinks(string body)
        {

            List<ChapterViewModel> chapters = new List<ChapterViewModel>();
            if (body.Contains("<table class=\"listing\">"))
            {

                int start = body.IndexOf("<table class=\"listing\">");
                int end = body.IndexOf("</table>");
                body = body.Substring(start, end - start);

                string selectedAnchor = "";
                while (body.Contains("<a"))
                {
                    start = body.IndexOf("<a");
                    end = body.IndexOf("</a");
                    if (start >= 0 && end >= 0)
                    {
                        selectedAnchor = body.Substring(start, end - start + 3);
                        //push body along
                        body = body.Substring(end + 3);

                        chapters.Add(new ChapterViewModel() { Link = HtmlParseing.ParseHrefLink(selectedAnchor), FileLocation = FolderName });
                    }
                }
            }
            return chapters;
        }

        private IList<String> getimageURLS(string body)
        {
            List<string> urls = new List<string>();

            body = body.Substring(body.IndexOf("<div id=\"divImage\""));
            body = body.Substring(0, body.IndexOf("</div>"));
            return urls;
        }
        #endregion //Methods

    }
}
