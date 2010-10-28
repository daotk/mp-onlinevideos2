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
    public class Freeload : HosterBase
    {
        public override string getHosterUrl()
        {
            return "Freeload.to";
        }

        public override string getVideoUrls(string url)
        {
            string page = SiteUtilBase.GetWebData(url);
            if (!string.IsNullOrEmpty(page))
            {
                Match n = Regex.Match(page, @"type=""video/divx""\ssrc=""(?<url>[^""]+)""");
                if (n.Success)
                {
                    return n.Groups["url"].Value + "&&&&" + "Referer: " + url + "\\n";
                }
            }
            return String.Empty;
        }
    }
}