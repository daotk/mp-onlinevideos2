﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineVideos.Hoster.Base;
using OnlineVideos.Sites;
using System.Text.RegularExpressions;
using System.Web;

namespace OnlineVideos.Hoster
{
    public class Archiv : HosterBase
    {
        public override string getHosterUrl()
        {
            return "Archiv.to";
        }
        
        public override string getVideoUrls(string url)
        {
            string page = SiteUtilBase.GetWebData(url);
            if (!string.IsNullOrEmpty(page))
            {
                Match n = Regex.Match(page, @"embed src=""(?<url>[^""]+)""");
                if (n.Success)
                {
                    videoType = VideoType.flv;
                    return Regex.Match(HttpUtility.UrlDecode(n.Groups["url"].Value), @"file=(?<url>[^&]+)&").Groups["url"].Value;
                }
            }
            return "";
        }
    }
}
