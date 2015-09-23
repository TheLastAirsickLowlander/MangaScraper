using MangaScraper.Utilties;
using ScFix.Utility.Classes;
using ScFix.Utility.ViewModels;
using ScFix.Utility.WebUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MangaScraper.ViewModel
{

    public enum ChapterProcessingStatus
    {
        None,
        Waiting,
        Processing,
        Done
    }
    public class ChapterViewModel : ViewModelBase
    {
        #region Properties
        #region Link
        protected string _Link;
        public string Link
        {
            get
            {
                return _Link;
            }
            set
            {
                if (value != _Link)
                {
                    _Link = value;
                    GenerateChapeterName(_Link);
                    OnPropertyChanged("Link");
                }
            }
        }

        #endregion //Link

        #region ChapterName
        protected String _ChapterName;
        public String ChapterName
        {
            get
            {
                return _ChapterName;
            }
            set
            {
                if (value != _ChapterName)
                {
                    _ChapterName = value;
                    OnPropertyChanged("ChapterName");
                }
            }
        }
        #endregion //ChapterName


        #region FileLocation
        protected String _FileLocation;
        public String FileLocation
        {
            get
            {
                return _FileLocation;
            }
            set
            {
                if (value != _FileLocation)
                {
                    _FileLocation = value;
                    OnPropertyChanged("FileLocation");
                }
            }
        }
        #endregion //FileLocation

        #region Status
        protected ChapterProcessingStatus _Status;
        public ChapterProcessingStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        #endregion //Status
        #endregion

        #region Members
        BackgroundWorker _bgWorker = null;
        bool parrallelFinished = false;
        #endregion

        #region Methods

        private void GenerateChapeterName(string LongName)
        {
            if (LongName.Contains("Ch-"))
            {
                string temp = LongName.Substring(LongName.IndexOf("Ch-"));
                //NOTE: this will generate chapters that are fine up to 999
                int length = temp.IndexOf("--");
                if (length == -1)
                {
                    length = temp.Substring(3).IndexOf("-") + 3;
                    if (length == -1)
                        length = 6;
                }
                ChapterName = temp.Substring(0, length);
            }
        }
        private List<ImageViewModel> getImages(string body)
        {
            List<ImageViewModel> images = new List<ImageViewModel>();

            if (body.Contains("<div id=\"divImage\""))
            {
                body = body.Substring(body.IndexOf(" var lstImages = new Array();"));

                int start = body.IndexOf("lstImages.push(");
                int end = body.IndexOf("$(\"#divImage img\").load(function() {  "); ;
                body = body.Substring(start, end - start);

                string selectedAnchor = "";
                while (body.Contains("lstImages.push("))
                {
                    start = body.IndexOf("lstImages.push(") + 16;
                    end = body.IndexOf("\")");
                    if (start >= 0 && end >= 0)
                    {
                        selectedAnchor = body.Substring(start, end - start);
                        //push body along
                        body = body.Substring(end + 2);

                        ImageViewModel ivm = new ImageViewModel() { Link = selectedAnchor };
                        images.Add(ivm);
                    }
                }
            }
            return images;
        }
        #endregion //Methods

        #region ProcessLink
        protected ICommand _ProcessLink = null;
        public ICommand ProcessLink
        {
            get
            {
                if (_ProcessLink == null)
                {
                    _ProcessLink = new RelayCommand(ProcessLinkExecute, CanProcessLink);
                }
                return _ProcessLink;
            }
        }

        private bool CanProcessLink(object obj)
        {
            return true;
        }

        private void ProcessLinkExecute(object obj)
        {
            //TODO Implement the ProcessLink Method
            Debug.WriteLine("ProcessLink Executed");
            Status = ChapterProcessingStatus.Waiting;
            ProcessLinkWebPage(obj.ToString());
        }
        #endregion //ProcessLink

        #region Getting body Background Thread
        public void ProcessLinkWebPage(string webBase)
        {
            if (_bgWorker != null)
            {
                _bgWorker.CancelAsync();
                _bgWorker = null;
            }
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += ProcessWebPage;
            _bgWorker.RunWorkerCompleted += WrapUpProcess;
            _bgWorker.RunWorkerAsync(webBase + Link);

        }

        private void WrapUpProcess(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("Finished With Chapter");
            Application.Current.Dispatcher.Invoke(() =>
            {
                Status = ChapterProcessingStatus.Processing;
            });
            _bgWorker = null;
        }

        private void ProcessWebPage(object sender, DoWorkEventArgs e)
        {

            string address = e.Argument.ToString();
            Application.Current.Dispatcher.Invoke(() => { this.Status = ChapterProcessingStatus.Processing; });
            WebManager.GetGeneratedHTML(address, KickOffDownloadImageThread, "$(\"#divImage img\").load(function() {");

        }
        #endregion

        #region Download Images BackgroundThread
        private void KickOffDownloadImageThread(string body)
        {
            if (_bgWorker != null)
            {
                _bgWorker.CancelAsync();
                _bgWorker = null;
            }
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += ProcessImageDownload;
            _bgWorker.RunWorkerCompleted += AllImagesDownloaded;
            _bgWorker.RunWorkerAsync(body);
        }

        private void ProcessImageDownload(object sender, DoWorkEventArgs e)
        {
            List<ImageViewModel> imageLinks = getImages(e.Argument.ToString());

            string dirPath = FileLocation + "\\" + ChapterName;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            if (!di.Exists)
            {
                di.Create();
            }

            var loopResult = Parallel.ForEach(imageLinks, img =>
            {
                try
                {
                    FileInfo fi = new FileInfo(di.ToString() + "\\" + img.FileName);
                    if (!fi.Exists)
                        WebManager.DownloadImage(img.Link, di.ToString() + "\\" + img.FileName);
                    //else
                        //Debug.WriteLine(fi.Name + " already exists");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("File Name through an excetion while processing");
                    Debug.WriteLine(ex.Message);
                }
            });
        }

        private void AllImagesDownloaded(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.Write("Chapeter Downloaded!");
            Application.Current.Dispatcher.Invoke(() =>
            {
                Status = ChapterProcessingStatus.Done;
            });
            _bgWorker = null;
        }
        #endregion

    }
}
