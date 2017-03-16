using ScFix.Utility.Classes;
using ScFix.Utility.ViewModels;
using ScFix.Utility.WebUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Contracts;
using WebScraper.EventArgs;

namespace WebScraper.ViewModel
{
	public class AnimeScraperViewModel : ViewModelBase, ITab
	{

		private ObservableCollection<AnimeSeriesViewModel> _Series = new ObservableCollection<AnimeSeriesViewModel>();

		public ObservableCollection<AnimeSeriesViewModel> Series
		{
			get { return _Series; }
			set { _Series = value; }
		}



		public AnimeScraperViewModel()
		{
			var series = new AnimeSeriesViewModel("series name", @"http://kissanime.ru/Anime/Tales-of-Zestiria-the-X");
			series.LoadSeries += Series_LoadSeries;
		   _Series.Add(series);
		}

		private void Series_LoadSeries(object sender, string link)
		{
			NavigateToPage?.Invoke(this, new NavigationArgs() { URL = link });
		}

		public event EventHandler<NavigationArgs> NavigateToPage;

		public string Header { get => "Anime Scraper"; }

		private RelayCommand _Login;
		public RelayCommand Login
		{
			get
			{
				if (_Login == null)
				{
					_Login = new RelayCommand(ExecuteLogin);
				}
				return _Login;

			}
		}


		private RelayCommand<string> _ParsePage;
		private void ParseHTML(string concatedLinkes)
		{
			if (concatedLinkes.Length > 0)
			{
				//store href
				var items = concatedLinkes.Split(',');
				var animeLinks = items.Reverse().ToList();
				// look up the name of the anime should already be done by this point but what the hell
				var series= Series.First();
				series.SetEpisodes(animeLinks);
			}
		}
		public RelayCommand<string> ParsePage
		{
			get
			{
				if (_ParsePage == null)
				{
					_ParsePage = new RelayCommand<string>(ParseHTML);
				}
				return _ParsePage;
			}
		}


		public string _Log;
		public string Log
		{
			get { return _Log; }
			set { _Log = value; }
		}

		private void ExecuteLogin(object obj)
		{
			NavigateToPage?.Invoke(this, new NavigationArgs() { URL = "http://kissanime.ru/" });
		}
	}
}
