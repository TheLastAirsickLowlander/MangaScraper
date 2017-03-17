using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Enum;

namespace WebScraper.EventArgs
{
	public class NavigationArgs : System.EventArgs
	{
		public String Detail { get; set; }
		public PageType PageState { get; set; }
	}
}
