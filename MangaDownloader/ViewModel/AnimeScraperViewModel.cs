using ScFix.Utility.Classes;
using ScFix.Utility.ViewModels;
using ScFix.Utility.WebUtility;
using System;
using System.Collections.Generic;
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


		private RelayCommand<string> _ParsePage = new RelayCommand<string>(ParseHTML);
		private static void ParseHTML(string concatedLinkes)
		{
			if (concatedLinkes.Length > 0)
			{
				//store href
				var items = concatedLinkes.Split(',');
				
			}
		}

		public RelayCommand<string> ParsePage
		{
			get => _ParsePage;
		}


		public string _Log;
		public string Log
		{
			get { return _Log; }
			set { _Log = value; }
		}

		private void ExecuteLogin(object obj)
		{
			NavigateToPage?.Invoke(this, new NavigationArgs() { URL = "http://kissanime.ru/Anime/Tales-of-Zestiria-the-X-Cross" });
		}
	}
}
