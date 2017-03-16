using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScFix.Utility.WebUtility
{
	/// <summary>
	/// For a full write up look to the stack overflow thread that this is ripped from
	/// <see cref="http://stackoverflow.com/questions/17183703/using-webclient-or-webrequest-to-login-to-a-website-and-access-data"/>
	/// </summary>
	public class CookieAwareWebClient : WebClient
	{
		public void Login(string loginPageAddress, LoginData loginData)
		{
			System.Net.ServicePointManager.Expect100Continue = false;
			CookieContainer container;

			var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

			request.Method = @"POST";
			request.ContentType = @"application/x-www-form-urlencoded";
			request.Timeout = 10000;
			request.KeepAlive = true;
			request.AutomaticDecompression = DecompressionMethods.GZip;
			request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			request.Referer = "http://kissanime.ru/Login";
			request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
			request.Host = @"kissanime.ru";
			request.UseDefaultCredentials = false;
			

			var buffer = Encoding.ASCII.GetBytes("username=" + loginData.username + "&password=" + loginData.password + "&redirect=");
			request.ContentLength = buffer.Length;
			var requestStream = request.GetRequestStream();
			requestStream.Write(buffer, 0, buffer.Length);
			requestStream.Close();

			container = request.CookieContainer = new CookieContainer();

			var response = request.GetResponse();
			response.Close();
			CookieContainer = container;
		}

		public CookieAwareWebClient(CookieContainer container)
		{
			CookieContainer = container;
		}

		public CookieAwareWebClient()
		  : this(new CookieContainer())
		{ }

		public CookieContainer CookieContainer { get; private set; }

		protected override WebRequest GetWebRequest(Uri address)
		{
			var request = (HttpWebRequest)base.GetWebRequest(address);
			request.CookieContainer = CookieContainer;
			return request;
		}
	}

	public class LoginData
	{
		public string username { get; set; }
		public string password { get; set; }
	}
}
