using ScFix.Utility.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Contracts;

namespace WebScraper.ViewModel
{
	public class AnimeScraperViewModel : ViewModelBase, ITab
	{
		public string Header { get => "Anime Scraper";}
	}
}
