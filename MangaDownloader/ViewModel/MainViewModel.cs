using ScFix.Utility.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Contracts;

namespace WebScraper.ViewModel
{
	public class MainViewModel : ViewModelBase
	{

		private IList<ITab> _Tabs;
		public IList<ITab> Tabs
		{
			get { return _Tabs; }
			set { _Tabs = value; }
		}

		private ITab _SelectedTab;

		public ITab SelectedTab
		{
			get { return _SelectedTab; }
			set { _SelectedTab = value; }
		}


		public MainViewModel()
		{
			_Tabs = new ObservableCollection<ITab>();
		}

	}
}
