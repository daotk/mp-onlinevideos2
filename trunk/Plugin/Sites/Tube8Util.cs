using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;
using MediaPortal.GUI.Library;

namespace OnlineVideos.Sites
{
    public class Tube8Util : SiteUtilBase    
    {
        public override List<VideoInfo> getVideoList(Category category)
        {
            return Parse(((RssLink)category).Url);            
        }
        
        private List<VideoInfo> Parse(String fsUrl)
        {
            List<VideoInfo> loRssItems = new List<VideoInfo>(); 

            try
            {
                // receive main page
                string dataPage = GetWebData(fsUrl);
                Log.Debug("Tube8 - Received " + dataPage.Length + " bytes");

                // is there any data ?
                if (dataPage.Length > 0)
                {
                    ParseLinks(dataPage, loRssItems);
                    if (loRssItems.Count > 0)
                    {
                        Log.Debug("Tube8 - finish to receive " + fsUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return loRssItems;
        }
        
        private void ParseLinks(string Page, List<VideoInfo> loRssItems)
        {
            int cnt = 0;

            GetValues g = new GetValues();

            while (g.Pointer != -1)
            {
                g.Html = Page;
                g.Search = ">videosArray";
                g.Start = "'";
                g.Stop = "'";

                g = GetDataPage(g);

                if (g.Pointer != -1)
                {
                    cnt++;
                    
                    // add new entry
                    VideoInfo loRssItem = new VideoInfo();
                    
                    loRssItem.VideoID = cnt;
                    loRssItem.SiteID = cnt.ToString();                    

                    //tmpClip.Url = g.Data;   // link
                    string h = g.Data;

                    string[] thb = new string[6];
                    g.Search = "[0]";                    
                    g = GetDataPage(g);
                    thb[0] = h + g.Data;    //ima 0     
                    /*
                    g.Search = "[1]";
                    g = GetDataPage(g);
                    thb[1] = h + g.Data;    //img 1
                    g.Search = "[2]";
                    g = GetDataPage(g);
                    thb[2] = h + g.Data;    //img 2
                    g.Search = "[3]";
                    g = GetDataPage(g);
                    thb[3] = h + g.Data;    //img 3
                    g.Search = "[4]";
                    g = GetDataPage(g);
                    thb[4] = h + g.Data;    //img 4
                    g.Search = "[5]";
                    g = GetDataPage(g);
                    thb[5] = h + g.Data;    //img 5
                    */
                    loRssItem.ImageUrl = thb[0];

                    g.Search = "href";
                    g.Start = "\"";
                    g.Stop = "\"";

                    g = GetDataPage(g);
                    loRssItem.VideoUrl = g.Data;    //video link

                    g.Search = "alt";
                    g = GetDataPage(g);
                    loRssItem.Title = g.Data; // title

                    loRssItems.Add(loRssItem);
                }
            }
        }        

        public struct GetValues
        {
            public string Html;
            public int Pointer;
            public string Search;
            public string Start;
            public string Stop;
            public string Data;
        }

        private static GetValues GetDataPage(GetValues inVal)
        {
            inVal.Data = "";
            string page = inVal.Html;

            int x = page.IndexOf(inVal.Search, inVal.Pointer);
            if (x > 0)
            {
                x = page.IndexOf(inVal.Start, x + inVal.Search.Length + 1);
                if (x > 0)
                {
                    int y = page.IndexOf(inVal.Stop, x + 1);
                    if (y > 0)
                    {
                        inVal.Data = page.Substring(x + 1, y - x - 1);
                        inVal.Pointer = y + 1;
                    }
                    else
                        inVal.Pointer = x + 1;
                }
                else
                {
                    inVal.Pointer = x + 1;
                }
            }
            else
                inVal.Pointer = x;

            return inVal;
        }

        // resolve url for video
        public override String getUrl(VideoInfo video, SiteSettings foSite)
        {
            string ret = video.VideoUrl;
            string data;

            data = GetWebData(video.VideoUrl);

            //so.addVariable('videoUrl','http://mediat03.tube8.com/flv/3c9947fb83c1a254453f79c67157576d/497f5b7c/0901/23/497a9788734a0/497a9788734a0.flv');

            GetValues g = new GetValues();
            g.Html = data;
            g.Search = "so.addVariable('videoUrl'";
            g.Start = "'";
            g.Stop = "'";

            g = GetDataPage(g);

            if (g.Pointer != -1)
            {
                ret = g.Data;
                Log.Debug("Tube8 - Found flv " + ret);
            }
            return ret;
        }
        
    }
}
