using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebScraper.ViewModel;
using ScFix.Utility.WebUtility;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;
using WebScraper.Enum;
using WebScraper.EventArgs;

namespace WebScraper.Views
{
	/// <summary>
	/// Interaction logic for AnimeScraperView.xaml
	/// </summary>
	public partial class AnimeScraperView : UserControl
	{

		static private string GetIndexLinks = @"function myfunc(){ var element = document.querySelectorAll('td > a'); var items = []; for(i =0; i < element.length; i++){items.push(element[i].href);}return items.toString();} myfunc();";
		static private string GetVideoLink = @"function myfunc(){var element = document.querySelectorAll('#divDownload > a'); var items = []; for (i = 0; i < element.length; i++){items.push(element[i]);} return items[0].href;} myfunc();";

		private Dispatcher dispatcher = null;
		private static PageType state = PageType.LoginPage;

		public ICommand ParsePage
		{
			get { return (ICommand)GetValue(ParsePageProperty); }
			set { SetValue(ParsePageProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ParsePage.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ParsePageProperty =
			DependencyProperty.Register("ParsePage", typeof(ICommand), typeof(AnimeScraperView));

		public AnimeScraperView()
		{
			InitializeComponent();
			wb.Navigate("http://google.com");
			dynamic activeX = this.wb.GetType().InvokeMember("ActiveXInstance",
					BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
					null, this.wb, new object[] { });

			activeX.Silent = true;

			this.dispatcher = Dispatcher;
		}

		private void Vm_NavigateToPage(object sender, EventArgs.NavigationArgs e)
		{
			state = e.PageState;
			wb.Navigate(e.Detail);

		}

		private void wb_LoadCompleted(object sender, NavigationEventArgs e)
		{
			//if (ParsePage != null)
			//{
			//	var s = HtmlDocumentHelper.GetHTML(wb);
			//	var obj = HtmlDocumentHelper.InvokeScript(wb, GetVideoLinks);
			//	ParsePage.Execute(obj);
			//}

		}

		private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var vm = DataContext as AnimeScraperViewModel;

			if (vm != null)

				vm.NavigateToPage += Vm_NavigateToPage;
		}


		private void wb_Navigated(object sender, NavigationEventArgs e)
		{
			if (ParsePage != null && state != PageType.LoginPage)
			{
				Task.Run(() =>
				{
					Thread.Sleep(500);
					dispatcher.Invoke(() =>
					{
						var s = HtmlDocumentHelper.GetHTML(wb);
						GetJs(state, out string js);
						var jsObjString = HtmlDocumentHelper.InvokeScript(wb, js);
						ParsePage.Execute(new NavigationArgs() { Detail = jsObjString, PageState = state });
						HtmlDocumentHelper.InvokeScript(wb, "document.execCommand('Stop');");
					});

				});
			}
		}

		private void GetJs(PageType state, out string js)
		{
			js = "";
			switch (state)
			{
				case PageType.LoginPage:
					break;
				case PageType.AnimeIndex:
					js = GetIndexLinks;
					break;
				case PageType.AnimeVideo:
					js = GetVideoLink;
					break;
				default:
					break;
			}
		}
	}
}
