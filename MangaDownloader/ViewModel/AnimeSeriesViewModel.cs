using ScFix.Utility.Classes;
using ScFix.Utility.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.EventArgs;

namespace WebScraper.ViewModel
{
	public class AnimeSeriesViewModel : ViewModelBase
	{
		public String SeriesName { get; set; }

		public String Link { get; set; }

		private ObservableCollection<AnimeLinksViewModel> _Episodes = new ObservableCollection<AnimeLinksViewModel>();
		public ObservableCollection<AnimeLinksViewModel> Episodes { get => _Episodes; }

		private RelayCommand _Load;
		public RelayCommand Load
		{
			get
			{
				if (_Load == null)
					_Load = new RelayCommand((obj) =>
					{
						LoadPage?.Invoke(this, new NavigationArgs() { Detail = Link, PageState = Enum.PageType.AnimeIndex });
					});
				return _Load;
			}
		}

		#region Events
		public event EventHandler<NavigationArgs> LoadPage;
		#endregion

		#region Constructor
		public AnimeSeriesViewModel(string seriesName, string link)
		{
			SeriesName = seriesName;
			Link = link;
			// create anime Link View Model 
		}
		#endregion //Constructor

		public void SetEpisodes(List<string> AnimeLinks)
		{
			AnimeLinks.ForEach((link) =>
			{
				var vm = new AnimeLinksViewModel() { Link = link };
				vm.RequestDownload += RequestDownload;
				Episodes.Add(vm);
			});

		}

		private void RequestDownload(object sender, NavigationArgs e)
		{
			// bubble the event to the top most viewmodel
			LoadPage.Invoke(sender, e);
		}
	}
}
