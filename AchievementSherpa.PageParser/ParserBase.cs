using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Diagnostics;

namespace AchievementSherpa.PageParser
{
    public class ParserBase
    {
        protected DateTime GetValueAsDateTime(HtmlNode rootNode, string xpath)
        {
            HtmlNode node = rootNode.SelectSingleNode(xpath);
            if (node != null)
            {
                DateTime value = DateTime.MinValue;
                if (DateTime.TryParse(node.InnerText, out value))
                {
                    return value;
                }
            }

            return DateTime.MinValue;
        }

        protected string GetValueAsString(HtmlNode rootNode, string xpath)
        {
            HtmlNode node = rootNode.SelectSingleNode(xpath);
            if (node != null)
            {
                return node.InnerText.Trim();
            }

            return string.Empty;
        }

        protected int GetValueAsInt32(HtmlNode rootNode, string xpath)
        {
            HtmlNode node = rootNode.SelectSingleNode(xpath);
            if (node != null)
            {
                int value = 0;
                int.TryParse(node.InnerText, out value);
                return value;
            }

            return 0;
        }

        protected Stopwatch stopwatch = new Stopwatch();

        protected HtmlDocument DownloadPage(string url, bool ignoreNotFound)
        {
            stopwatch.Start();
            string html = DownloadHtml.GetHtmlFromUrl(new Uri(url), ignoreNotFound);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            stopwatch.Stop();

            return doc;
        }
        protected HtmlDocument DownloadPage(string url)
        {
            return DownloadPage(url, false);
           
        }
    }
}
