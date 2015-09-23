using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaScraper.Utilties
{
    public class HtmlParseing
    {
        public static string ParseHrefLink(string anchor)
        {
            anchor = anchor.Replace(" ", "");
            string temp = "";
            if (anchor.Contains("href"))
            {
                anchor = anchor.Substring(anchor.IndexOf("href") + 6);
                foreach (char c in anchor)
                {
                    if (c != '"')
                    {
                        temp = temp + c;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            return temp;
        }

        public static string ParseImgLink(string anchor)
        {
            anchor = anchor.Replace(" ", "");
            string temp = "";
            if (anchor.Contains("src"))
            {
                anchor = anchor.Substring(anchor.IndexOf("src") + 5);
                foreach (char c in anchor)
                {
                    if (c != '"')
                    {
                        temp = temp + c;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            return temp;
        }

    }
}
