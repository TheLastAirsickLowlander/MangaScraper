using ScFix.Utility.Classes;
using ScFix.Utility.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
						LoadSeries?.Invoke(this, Link);
					});
				return _Load;
			}
		}

		public event EventHandler<string> LoadSeries;


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
			AnimeLinks.ForEach((link) => { Episodes.Add(new AnimeLinksViewModel() { Link = link }); });

		}



	}
}
