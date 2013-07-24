using System;
using System.Net;
using System.Text.RegularExpressions;

using OnlineVideos.Hoster.Base;
using OnlineVideos.Sites;

namespace OnlineVideos.Hoster
{
    public class PrimeShare : HosterBase
    {
        public override string getHosterUrl()
        {
            return "primeshare.tv";
        }

        private string requestFileInformation(string url, CookieContainer cc)
        {
            string webData = SiteUtilBase.GetWebData(url, cc);
            if (!string.IsNullOrEmpty(webData))
            {
                if (!string.IsNullOrEmpty(getRegExData(@"(?<exists>This\sfile\sdoesn\'t\sexist,\sor\shas\sbeen\sremoved\s?\.)", webData, "exists")))
                    webData = string.Empty;
            }
            return webData;
        }

        public override string getVideoUrls(string url)
        {
            CookieContainer cc = new CookieContainer();
            string webData = requestFileInformation(url, cc);
            if (string.IsNullOrEmpty(webData)) return string.Empty;
            

             //Grab hidden value: op, usr_login, id, fname, referer, method_free
            string hash = GetSubString(webData, @"name=""hash"" value=""", @"""");

            //Wait for 10 seconds
             System.Threading.Thread.Sleep(11000);

            //Send Postdata (simulates a button click)
             string postData = @"hash=" + hash;
             string page = GenericSiteUtil.GetWebDataFromPost(url, postData, cc);

            //Grab file url from html
            Match n = Regex.Match(page, @"provider:\s'stream',\s*url:\s'(?<url>[^']*)'");

            if (n.Success)
            {
                videoType = VideoType.flv;
                return n.Groups["url"].Value;
            }
			return String.Empty;
        }
    }
}