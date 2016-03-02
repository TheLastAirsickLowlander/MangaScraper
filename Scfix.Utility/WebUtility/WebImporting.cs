using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Windows;

namespace ScFix.Utility.WebUtility
{
	public class WebManager
	{

		//if tmpImage is null then you did not get anything from the web

		static public Image DownloadImage(string URL)
		{
			Image tmpImage = null;
			HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
			webRequest.AllowWriteStreamBuffering = true;
			//timesout in 10 seconds
			webRequest.Timeout = 10000;
			//gets the response
			WebResponse webResponse = webRequest.GetResponse();
			//gets the strea
			Stream stream = webResponse.GetResponseStream();
			tmpImage = Image.FromStream(stream);

			return tmpImage;
		}

		static public bool DownloadImage(string URL, string FileLoaction)
		{
			try
			{
				HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
				webRequest.AllowWriteStreamBuffering = true;
				//timesout in 10 seconds
				webRequest.Timeout = 20000;
				//gets the response
				WebResponse webResponse = webRequest.GetResponse();
				//gets the strea
				Stream stream = webResponse.GetResponseStream();
				FileInfo fi = new FileInfo(FileLoaction);
				if (fi.Exists)
				{
					throw new Exception("File already Exists");
				}
				else
				{
					using (var fileStream = File.Create(FileLoaction))
					{
						stream.CopyTo(fileStream);
					}
					return true;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return false;

			}

		}

		static public string GetWebPage(string URL)
		{
			try
			{
				HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
				webRequest.AllowWriteStreamBuffering = true;
				//ten second timeout
				webRequest.Timeout = 10000;
				WebResponse WebResponse = webRequest.GetResponse();
				Stream stream = WebResponse.GetResponseStream();
				StreamReader sr = new StreamReader(stream);
				string s = sr.ReadToEnd();
				return s;
			}
			catch (Exception x)
			{
				throw new Exception("Web Page did not load", x);
			}
		}

		static public void GetGeneratedHTML(string url, Action<string> Callback, string breakCondition = "")
		{
			Thread t = new Thread(Bg_DoWork);
			t.SetApartmentState(ApartmentState.STA);
			t.Start(new GeneratedHtmlArgs() { url = url, callback = Callback, breakCondition = breakCondition });


		}

		private static void Bg_DoWork(Object obj)
		{
			GeneratedHtmlArgs args = (GeneratedHtmlArgs)obj;
			System.Windows.Forms.WebBrowser wb = new System.Windows.Forms.WebBrowser();
			var na = new navigatingArgs() { AcceptedUrl = args.url };
			wb.Navigating += na.Wb_Navigating;
			wb.DocumentCompleted += na.Wb_DocumentCompleted;
			wb.Navigate(args.url);

			//wb.FileDownload += Wb_FileDownload;
			while (!na.isLoaded || (wb.ReadyState != WebBrowserReadyState.Complete))
			{
				if (wb.DocumentText != "" && !wb.DocumentText.Contains("href=\"\""))
					if (args.breakCondition != "" && wb.DocumentText.Contains(args.breakCondition))
						break;
				System.Windows.Forms.Application.DoEvents();

			}


			//Added this line, because the final HTML takes a while to show up
			args.finalHtml = wb.DocumentText;



			System.Windows.Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { args.callback(args.finalHtml); }));

			wb.Dispose();


		}



		private static void Wb_FileDownload(object sender, EventArgs e)
		{
			Debug.WriteLine("whate aer you doing");
		}

		private static void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (!e.Cancelled && e.Error == null)
			{

			}
		}

		private class GeneratedHtmlArgs
		{
			public string url { get; set; }
			public string breakCondition { get; set; }
			public Action<String> callback { get; set; }

			public string finalHtml { get; set; }
		}


		private class navigatingArgs
		{
			public string AcceptedUrl { get; set; }
			public bool isLoaded { get; set; }

			public void Wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
			{
				if (!(sender as WebBrowser).DocumentTitle.Contains("5 seconds"))
					isLoaded = true;
			}
			public void Wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
			{
				//Debug.WriteLine("whate");
				if (e.Url.ToString() != AcceptedUrl)
				{
					//e.Cancel = true;
				}
			}
		}
	}
}
