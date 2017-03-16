using ScFix.Utility.Classes;
using ScFix.Utility.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper.ViewModel
{
	public class AnimeLinksViewModel : ViewModelBase
	{
		public String Link { get; set; }
		public event EventHandler<string> RequestDownload;

		private void request(object obj)
		{
			RequestDownload?.Invoke(this, Link);
		}

		private RelayCommand _Download;
		
		public RelayCommand Download
		{
			get
			{
				if (_Download == null)
				{
					_Download = new RelayCommand(request);
				}
				return _Download;
			}

		}
	}
}

