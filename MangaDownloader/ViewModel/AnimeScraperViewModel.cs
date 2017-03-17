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
using WebScraper.Enum;
using WebScraper.EventArgs;

namespace WebScraper.ViewModel
{
	public class AnimeScraperViewModel : ViewModelBase, ITab
	{
		#region Properties
		public string Header { get => "Anime Scraper"; }

		public string _Log;
		public string Log
		{
			get { return _Log; }
			set { _Log = value; }
		}
		private ObservableCollection<AnimeSeriesViewModel> _Series = new ObservableCollection<AnimeSeriesViewModel>();
		public ObservableCollection<AnimeSeriesViewModel> Series
		{
			get { return _Series; }
			set { _Series = value; }
		}
		#endregion

		#region Events
		public event EventHandler<NavigationArgs> NavigateToPage;
		#endregion //events

		#region Constructor
		public AnimeScraperViewModel()
		{
			var series = new AnimeSeriesViewModel("series name", @"http://kissanime.ru/Anime/Tales-of-Zestiria-the-X");
			series.LoadPage += Series_LoadPage;
			_Series.Add(series);

		}

		private void Series_LoadPage(object sender, NavigationArgs e)
		{
			// bubble to the view
			NavigateToPage?.Invoke(this, e);
		}
		#endregion //Constructor



		#region Login
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
		private void ExecuteLogin(object obj)
		{
			NavigateToPage?.Invoke(this, new NavigationArgs() { Detail = "http://kissanime.ru/", PageState = PageType.LoginPage });
		}
		#endregion //Login

		#region Parse page
		private RelayCommand<NavigationArgs> _ParsePage;
		private void ParseHTML(NavigationArgs navArgs)
		{
			if (navArgs.PageState == PageType.AnimeIndex)
			{
				var concatedLinks = navArgs.Detail;
				if (concatedLinks.Length > 0)
				{
					//store href
					var items = concatedLinks.Split(',');
					var animeLinks = items.Reverse().ToList();
					// look up the name of the anime should already be done by this point but what the hell
					var series = Series.First();
					series.SetEpisodes(animeLinks);
				}
			}
			if (navArgs.PageState == PageType.AnimeVideo)
			{

			}
		}
		public RelayCommand<NavigationArgs> ParsePage
		{
			get
			{
				if (_ParsePage == null)
				{
					_ParsePage = new RelayCommand<NavigationArgs>(ParseHTML);
				}
				return _ParsePage;
			}
		}
		#endregion // Parse Page





	}
}
