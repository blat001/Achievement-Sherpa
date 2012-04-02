using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Xml.Linq;
using System.Net;
using System.Text;
using System.IO;






    /// 
    /// Summary description for DownloadHtml
    /// 
    internal static class DownloadHtml
    {

        public static string GetHtmlFromUrl(Uri url)
        {
            return GetHtmlFromUrl(url, false);
        }
        public static string GetHtmlFromUrl(Uri url, bool ignoreNotFound)
        {
            string html = string.Empty;
            bool success = false;

            int numberOfDownloads = 0;
            while (!success)
            {
                try
                {
                    HttpWebRequest request = GenerateHttpWebRequest(url);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (CategorizeResponse(response) == ResponseCategories.Success)
                        {
                            Stream responseStream = response.GetResponseStream();
                            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                html = reader.ReadToEnd();
                            }
                        }
                    }

                    success = true;
                }
                catch (WebException web)
                {
                    if (web.Response != null)
                    {
                        HttpWebResponse response = web.Response as HttpWebResponse;
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            success = true;
                        }

                        if (ignoreNotFound)
                        {
                            Stream responseStream = response.GetResponseStream();
                            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                html = reader.ReadToEnd();
                            }

                            //Console.WriteLine("NOT FOUND ERROR");
                        }
                    }

                    //if ( web.Status == WebExceptionStatus.
                    //Console.WriteLine("Web Exception for URL: {0}", url);
                }
                finally
                {
                    numberOfDownloads++;

                    if (numberOfDownloads > 10)
                    {
                        success = true;
                    }
                }

                
            }
            return html;
        }

        public static HttpWebRequest GenerateHttpWebRequest(Uri uri)
        {

            //all this mess below is my attempt to resolve some of the issues in taking on various conflicts in httpreqeust.
            //code is left in
            //if infact requests vary may need to switch(key) on differnet sites?

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.672.2 Safari/534.2";

            CookieContainer cc = new CookieContainer();
            httpRequest.CookieContainer = cc;//must assing a cookie container for the request to pull the cookies

            httpRequest.AllowAutoRedirect = true;   //example, Hanes.com

            httpRequest.Credentials = CredentialCache.DefaultCredentials;

            //httpRequest.Headers.Add("HTTP_USER_AGENT", @"Mozilla/5.0(PC) (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4");


            //   httpRequest.Headers.Add("Agent", "Mozilla/5.0(PC) (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4");
            //   httpRequest.Headers.Add("Accept-Charset", "ISO-8859-1");
            /*

            httpRequest.Headers.Add("Accept-Language", "en-us,en;q=0.5");
            httpRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
            httpRequest.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");


          //  httpRequest.Headers.Add("Set-Cookie", response.Headers("Set-Cookie"));
            httpRequest.Headers.Add("Agent", "Mozilla//5.0 (X11; U; Linux i686; en-US; ry; 1.8.0.7) Geck//20060925 Firefox//1.5.0.7");
            */


            return httpRequest;
        }

        public static HttpWebRequest GenerateHttpWebRequest(Uri uri, string postData, string contentType)
        {
            HttpWebRequest httpRequest = GenerateHttpWebRequest(uri);

            byte[] bytes = Encoding.UTF8.GetBytes(postData);

            httpRequest.ContentLength = postData.Length;

            using (Stream requestStream = httpRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            return httpRequest;
        }

        public static HttpWebRequest AddProxyInfoToRequest(HttpWebRequest httpRequest, Uri proxyUri, string proxyId, string proxyPassword, string proxyDomain)
        {
            if (httpRequest != null)
            {

                WebProxy proxyInfo = new WebProxy();
                proxyInfo.Address = proxyUri;
                proxyInfo.BypassProxyOnLocal = true;
                proxyInfo.Credentials = new NetworkCredential(proxyId, proxyPassword, proxyDomain);
                httpRequest.Proxy = proxyInfo;

            }
            return httpRequest;
        }

        public static ResponseCategories CategorizeResponse(HttpWebResponse httpResponse)
        {
            //Just incase there are more success codes defined in the future by
            // HttpStatusCode, We will checkf or the "success" ranges
            // instead of using teh HttpStatusCode enum as it overloads some values.

            int statusCode = (int)httpResponse.StatusCode;

            if ((statusCode >= 100) && (statusCode <= 199))
            {
                return ResponseCategories.Informational;
            }
            else if ((statusCode >= 200) && (statusCode <= 299))
            {
                return ResponseCategories.Success;
            }
            else if ((statusCode >= 300) && (statusCode <= 399))
            {
                return ResponseCategories.Redirected;
            }
            else if ((statusCode >= 400) && (statusCode <= 499))
            {
                return ResponseCategories.ClientError;
            }
            else if ((statusCode >= 500) && (statusCode <= 599))
            {
                return ResponseCategories.ServerError;
            }
            return ResponseCategories.Unknown;
        }


        public enum ResponseCategories
        {
            Unknown, // Unknown code ( < 100 or > 599 )
            Informational, //Informational codes (100 >=199)
            Success, // success codes (200 >= 299)
            Redirected, //Redirection code (300, 399)
            ClientError, // Client error code (400 >= 499)
            ServerError//Server Error Code (500,599 )

        }


    }