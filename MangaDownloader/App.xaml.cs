using WebScraper.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebScraper.Views;

namespace WebScraper
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var mvm = new MainViewModel();
			mvm.Tabs.Add(new MangaScraperViewModel());
			mvm.Tabs.Add(new AnimeScraperViewModel());

			mvm.SelectedTab = mvm.Tabs.First();
			MainView mv = new MainView();
			mv.DataContext = mvm;
			mv.Show();
		}
	}
}
