﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OnlineVideos.Sites
{
    public class TV3NZOnDemandUtil : GenericSiteUtil
    {
        public override string getUrl(VideoInfo video)
        {
            string res = base.getUrl(video);
            string[] urlParts = video.VideoUrl.Split('.');

            if (urlParts[1] == "four")
                res = res.Replace("@@", "c4");
            else
                res = res.Replace("@@", "tv3");

            video.PlaybackOptions = new Dictionary<string, string>();
            string[] bitRates = { "330K", "700K" };
            foreach (string bitRate in bitRates)
            {
                string url = ReverseProxy.Instance.GetProxyUri(RTMP_LIB.RTMPRequestHandler.Instance,
                string.Format("http://127.0.0.1/stream.flv?rtmpurl={0}&swfVfy={1}",
                HttpUtility.UrlEncode(res + bitRate),
                HttpUtility.UrlEncode(@"http://static.mediaworks.co.nz/video/3.9/videoPlayer3.9.swf")));
                video.PlaybackOptions.Add(bitRate, url);
                //rtmpe://nzcontent.mediaworks.co.nz/tv3/_definst_/mp4:/transfer/07022011/HW031459_700K -W http://static.mediaworks.co.nz/video/3.9/videoPlayer3.9.swf
                //rtmpe://nzcontent.mediaworks.co.nz/c4/_definst_/mp4:/transfer/07022011/HW031459_700K -W http://static.mediaworks.co.nz/video/3.9/videoPlayer3.9.swf
            }
            return video.PlaybackOptions[bitRates[0]];
        }
    }
}