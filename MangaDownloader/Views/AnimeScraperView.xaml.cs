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

namespace WebScraper.Views
{
	/// <summary>
	/// Interaction logic for AnimeScraperView.xaml
	/// </summary>
	public partial class AnimeScraperView : UserControl
	{
		private enum PageState {
			LoginPage,
			AnimeIndex,
			AnimeVideo
		}
		static private string GetVideoLinks = @"function myfunc(){ var element = document.querySelectorAll('td > a'); var items = []; for(i =0; i < element.length; i++){items.push(element[i].href);}return items.toString();} myfunc();";

		private Dispatcher dispatcher = null;

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
			wb.Navigate(e.URL);
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
			{
				vm.NavigateToPage += Vm_NavigateToPage;
			}
		}

		private void wb_Navigated(object sender, NavigationEventArgs e)
		{
			if (ParsePage != null)
			{
				Task.Run(() =>
				{
					Thread.Sleep(500);
					dispatcher.Invoke(() => {
						var s = HtmlDocumentHelper.GetHTML(wb);
						var obj = HtmlDocumentHelper.InvokeScript(wb, GetVideoLinks);
						ParsePage.Execute(obj);
						HtmlDocumentHelper.InvokeScript(wb, "document.execCommand('Stop');");
					});

				});
			}
		}
	}
}
