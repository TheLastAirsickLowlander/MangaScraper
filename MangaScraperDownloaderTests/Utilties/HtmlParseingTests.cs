using Microsoft.VisualStudio.TestTools.UnitTesting;
using MangaScraper.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaScraper.Utilties.Tests
{
    [TestClass()]
    public class HtmlParseingTests
    {
        [TestMethod()]
        public void ParseHrefLinkTest()
        {
            string body = "<a href=\"/Manga/Dungeon-ni-Deai-o-Motomeru-no-wa-Machigatte-Iru-Darou-ka/Ch-025--Godly-Liquor?id=221924\" title = \"Read Dungeon ni Deai o Motomeru no wa Machigatte Iru Darou ka Ch.025: Godly Liquor online\" > Dungeon ni Deai o Motomeru no wa Machigatte Iru Darou ka Ch.025: Godly Liquor</ a >";

            string value = HtmlParseing.ParseHrefLink(body);

            Assert.AreEqual("/Manga/Dungeon-ni-Deai-o-Motomeru-no-wa-Machigatte-Iru-Darou-ka/Ch-025--Godly-Liquor?id=221924", value);
        }

        [TestMethod()]
        public void ParseImgLinkTest()
        {
            string body = "<img src=\"http://2.bp.blogspot.com/-nIClzDvEdRw/VYRdmoyMLmI/AAAAAAACV0Q/MIt12iIV7Wk/000.png?imgmax=30000\"></src>";
            string value = HtmlParseing.ParseImgLink(body);
            Assert.AreEqual("http://2.bp.blogspot.com/-nIClzDvEdRw/VYRdmoyMLmI/AAAAAAACV0Q/MIt12iIV7Wk/000.png?imgmax=30000", value);
        }
    }
}