using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ScFix.Utility.WebUtility
{
	public static class HtmlDocumentHelper
	{
		public static void FillField(object doc, string id, string value)
		{
			var element = findElementByID(doc, id);
			element.setAttribute("value", value);
		}

		public static void ClickButton(object doc, string id)
		{
			var element = findElementByID(doc, id);
			element.click();
		}

		private static IHTMLElement findElementByID(object doc, string id)
		{
			IHTMLDocument2 thisDoc;
			if (!(doc is IHTMLDocument2))
				return null;
			else
				thisDoc = (IHTMLDocument2)doc;

			var element = thisDoc.all.OfType<IHTMLElement>()
				.Where(n => n != null && n.id != null)
				.Where(e => e.id == id).First();
			return element;
		}

		public static void ExecuteScript(object doc, string js)
		{
			IHTMLDocument2 thisDoc;
			if (!(doc is IHTMLDocument2))
				return;
			else
				thisDoc = (IHTMLDocument2)doc;
			thisDoc.parentWindow.execScript(js);
		}
		public static string GetHTML(WebBrowser webBrowser)
		{
			return (webBrowser.Document as IHTMLDocument2).body.outerHTML;
		}
		public static string InvokeScript(WebBrowser webBrowser, string js)
		{
			return webBrowser.InvokeScript("eval", js).ToString();
		}
	}
}
