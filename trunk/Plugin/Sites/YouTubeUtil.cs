using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using MediaPortal.GUI.Library;

namespace OnlineVideos.Sites
{

    public class YouTubeUtil : SiteUtilBase, IFilter, ISearch, IFavorite
    {
        [Category("OnlineVideosConfiguration"), Description("Add some dynamic categories found at startup to the list of configured ones.")]
        bool useDynamicCategories = true;

        static Regex PageStartIndex = new Regex(@"start-index=(?<item>[\d]+)", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        static Regex swfJsonArgs = new Regex(@"var\sswfArgs\s=\s(?<json>\{.+\})", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private List<int> steps;
        private Dictionary<String, String> orderByList;
        private Dictionary<String, String> timeFrameList;
        private YouTubeQuery _LastPerformedQuery;
        private const String CLIENT_ID = "ytapi-GregZ-OnlineVideos-s2skvsf5-0";
        private const String DEVELOPER_KEY = "AI39si5x-6x0Nybb_MvpC3vpiF8xBjpGgfq-HTbyxWP26hdlnZ3bTYyERHys8wyYsbx3zc5f9bGYj0_qfybCp-wyBF-9R5-5kA";
        private const String FAVORITE_FEED = "http://gdata.youtube.com/feeds/api/users/{0}/favorites";
        private const String RELATED_VIDEO_FEED = "http://gdata.youtube.com/feeds/api/videos/{0}/related";
        private const String VIDEO_URL = "http://www.youtube.com/get_video?video_id={0}&t={1}";
        private const String CATEGORY_FEED = "http://gdata.youtube.com/feeds/api/videos/-/{0}";
        private const String ALL_CATEGORIES_FEED = "http://gdata.youtube.com/schemas/2007/categories.cat?hl=en-US";
        private YouTubeService service;

        public YouTubeUtil()
        {
            steps = new List<int>();
            steps.Add(10);
            steps.Add(20);
            steps.Add(30);
            steps.Add(40);
            steps.Add(50);
            orderByList = new Dictionary<String, String>();
            orderByList.Add("Relevance", "relevance");
            orderByList.Add("Published", "published");
            orderByList.Add("View Count", "viewCount");
            orderByList.Add("Rating", "rating");

            timeFrameList = new Dictionary<string, string>();
            foreach(String name in Enum.GetNames(typeof(YouTubeQuery.UploadTime))){
                if(name.Equals("ThisWeek",StringComparison.InvariantCultureIgnoreCase)){
                    timeFrameList.Add("This Week",name);
                }else if(name.Equals("ThisMonth",StringComparison.InvariantCultureIgnoreCase)){
                    timeFrameList.Add("This Month",name);
                }else if(name.Equals("Today",StringComparison.InvariantCultureIgnoreCase)){
                    timeFrameList.Add("Today",name);
                }else if(name.Equals("AllTime",StringComparison.InvariantCultureIgnoreCase)){
                    timeFrameList.Add("All Time",name);
                }else{
                    timeFrameList.Add(name,name);
                }
            }            

            service = new YouTubeService("OnlineVideos", CLIENT_ID, DEVELOPER_KEY);
            

            //orderByList.Add("")
        }

        public enum YoutubeVideoQuality : int
        {
            Normal = 0,
            High = 1,
            HD = 2,
            Unknow = 3,
        }

        string nextPageUrl = "";
        string previousPageUrl = "";
        bool nextPageAvailable = false;
        bool previousPageAvailable = false;

        private CookieCollection moCookies;
        private Regex regexId = new Regex("/videos/(.+)");

        public override bool HasLoginSupport
        {
            get { return true; }
        }

        public override List<OnlineVideos.VideoInfo> getRelatedVideos(string fsId)
        {
            YouTubeQuery query = new YouTubeQuery(String.Format(RELATED_VIDEO_FEED, fsId));
            return parseGData(query);
        }

        public override List<VideoInfo> getSiteFavorites(String fsUser)
        {
            //http://www.youtube.com/api2_rest?method=%s&dev_id=7WqJuRKeRtc&%s"   # usage   base_api %( method, extra)   eg base_api %( youtube.videos.get_detail, video_id=yyPHkJMlD0Q)
            //String lsUrl = "http://www.youtube.com/api2_rest?method=youtube.users.list_favorite_videos&dev_id=7WqJuRKeRtc&user="+fsUser;
            YouTubeQuery query = new YouTubeQuery(String.Format(FAVORITE_FEED, fsUser));
            //return parseRestXML(lsUrl);
            return parseGData(query);
            //String lsXMLResponse = getHTMLData(lsUrl);
            //Log.Info(lsXMLResponse);
        }

        public List<VideoInfo> parseGData(YouTubeQuery query)
        {           

            YouTubeFeed feed = service.Query(query);

            List<VideoInfo> loRssItems = new List<VideoInfo>();

            // check for previous page link
            if (feed.PrevChunk != null)
            {
                previousPageAvailable = true;
                previousPageUrl = feed.PrevChunk;
            }
            else
            {
                previousPageAvailable = false;
                previousPageUrl = "";
            }

            // check for next page link
            if (feed.NextChunk != null)
            {
                nextPageAvailable = true;
                nextPageUrl = feed.NextChunk;
            }
            else
            {
                nextPageAvailable = false;
                nextPageUrl = "";
            }

            foreach (YouTubeEntry entry in feed.Entries)
            {
                loRssItems.Add(getVideoInfo(entry));
            }
            _LastPerformedQuery = query;

            return loRssItems;
        }

        public VideoInfo getVideoInfo(YouTubeEntry entry)
        {
            VideoInfo video = new VideoInfo();
            video.Other = entry;

            video.Description = entry.Media.Description != null ? entry.Media.Description.Value : "";
            int maxHeight = 0;
            foreach (MediaThumbnail thumbnail in entry.Media.Thumbnails)
            {
                if (Int32.Parse(thumbnail.Height) > maxHeight)
                {
                    video.ImageUrl = thumbnail.Url;
                }
            }
            video.Length = entry.Media.Duration != null ? entry.Media.Duration.Seconds : "";
            video.Title = entry.Title.Text;            
            video.VideoUrl = entry.Media.VideoId.Value;
            return video;
        }

        public override List<VideoInfo> getVideoList(Category category)
        {
            string fsUrl = ((RssLink)category).Url;

            if (fsUrl.StartsWith("fav:"))
            {
                return getSiteFavorites(fsUrl.Substring(4));
            }
            YouTubeQuery query = new YouTubeQuery(fsUrl);
            List<VideoInfo> loRssItemList = parseGData(query);
            //List<VideoInfo> loVideoList = new List<VideoInfo>();
            //VideoInfo video;			
            return loRssItemList;
        }

        public override String getUrl(VideoInfo foVideo)
        {
            OnlineVideoSettings settings = OnlineVideoSettings.getInstance();
            YoutubeVideoQuality qa = settings.YouTubeQuality;

            Dictionary<string, string> Items = new Dictionary<string, string>();
            GetVideInfo(foVideo.VideoUrl, Items);

            string Token = "";
            string FmtMap = "";

            if (Items.ContainsKey("token"))
                Token = Items["token"];
            if (Token == "" && Items.ContainsKey("t"))
                Token = Items["t"];
            if (Items.ContainsKey("fmt_map"))
                FmtMap = System.Web.HttpUtility.UrlDecode(Items["fmt_map"]);
            
            if (qa == YoutubeVideoQuality.HD && !FmtMap.Contains("22/"))
            {
                qa = YoutubeVideoQuality.High;
            }

            string lsUrl = string.Format("http://youtube.com/get_video?video_id={0}&t={1}&ext=.flv", foVideo.VideoUrl, Token);
            switch (qa)
            {
                case YoutubeVideoQuality.Normal:
                    lsUrl = string.Format("http://youtube.com/get_video?video_id={0}&t={1}&ext=.flv", foVideo.VideoUrl, Token);
                    break;
                case YoutubeVideoQuality.High:
                    lsUrl = string.Format("http://youtube.com/get_video?video_id={0}&t={1}&fmt=18&ext=.mp4", foVideo.VideoUrl, Token);
                    break;
                case YoutubeVideoQuality.HD:
                    lsUrl = string.Format("http://youtube.com/get_video?video_id={0}&t={1}&fmt=22&ext=.mp4", foVideo.VideoUrl, Token);
                    break;
            }
            Log.Info("youtube video url={0}", lsUrl);
            return lsUrl;
        }
        
        public void GetVideInfo(string videoId,Dictionary<string, string> Items )
        {
            WebClient client = new WebClient();
            client.CachePolicy = new System.Net.Cache.RequestCachePolicy();
            client.UseDefaultCredentials = true;
            client.Proxy.Credentials = CredentialCache.DefaultCredentials;
            try
            {                
                string contents = client.DownloadString(string.Format("http://youtube.com/get_video_info?video_id={0}", videoId));
                string[] elemest = (contents).Split('&');

                foreach (string s in elemest)
                {
                    Items.Add(s.Split('=')[0], s.Split('=')[1]);
                }

                if (Items["status"] == "fail")
                {
                    contents = client.DownloadString(string.Format("http://www.youtube.com/watch?v={0}", videoId));
                    Match m = swfJsonArgs.Match(contents);
                    if (m.Success)
                    {
                        Items.Clear();
                        object data = Jayrock.Json.Conversion.JsonConvert.Import(m.Groups["json"].Value);
                        foreach (string z in (data as Jayrock.Json.JsonObject).Names)
                        {
                            Items.Add(z,(data as Jayrock.Json.JsonObject)[z].ToString());
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public bool login(String fsUser, String fsPassword)
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://www.youtube.com/login?next=/");
            //HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://www.google.com/youtube/accounts/ClientLogin");
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.CookieContainer = new CookieContainer();


            Stream RequestStream = Request.GetRequestStream();
            ASCIIEncoding ASCIIEncoding = new ASCIIEncoding();
            //Byte [] PostData = ASCIIEncoding.GetBytes("username=" + fsUser +"&password="+ fsPassword);

            Byte[] PostData = ASCIIEncoding.GetBytes("current_form=loginForm&next=%%2F&username=" + fsUser + "&password=" + fsPassword + "&action_login=Log+In");
            //Byte [] PostData = ASCIIEncoding.GetBytes("Email="+fsUser+"&Passwd="+fsPassword+"&service=youtube&source=MP-OnlineVideos");
            RequestStream.Write(PostData, 0, PostData.Length);
            RequestStream.Close();
            HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
            //StreamReader Reader  = new StreamReader(Request.GetResponse().GetResponseStream());
            //String ResultHTML = Reader.ReadToEnd();
            response.Cookies = Request.CookieContainer.GetCookies(Request.RequestUri);
            moCookies = response.Cookies;
            //	Log.Info("Found {0} cookies after login ",response.Cookies.Count);

            //foreach(Cookie cky in response.Cookies)
            //{
            //	Log.Info(cky.Name + " = " + cky.Value +" expires on "+cky.Expires);
            //}
            response.Close();
            return isLoggedIn();
        }

        private bool isLoggedIn()
        {
            return moCookies != null && moCookies["LOGIN_INFO"] != null;
        }
        
        public override int DiscoverDynamicCategories()
        {
            if (!useDynamicCategories) return base.DiscoverDynamicCategories();

            Dictionary<String, String> categories = getYoutubeCategories();
            foreach (KeyValuePair<String, String> cat in categories)
            {
                RssLink item = new RssLink();
                item.Name = cat.Key;
                item.Url = String.Format(CATEGORY_FEED, cat.Value);
                Settings.Categories.Add(item);
            }
            Settings.DynamicCategoriesDiscovered = true;
            return categories.Count;
        }

        //private Dictionary<String, String> getYoutubeCategories()
        //{
        //    YouTubeQuery query = new YouTubeQuery(ALL_CATEGORIES_FEED);
        //    YouTubeFeed feed = service.Query(query);
        //    Dictionary<String, String> categories = new Dictionary<string, string>();
        //    foreach (YouTubeCategory category in feed.Categories)
        //    {
        //        categories.Add(category.Label,category.Term);   
        //    }
        //    return categories;
        //}

        private Dictionary<String, String> getYoutubeCategories()
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(XmlReader.Create("http://gdata.youtube.com/schemas/2007/categories.cat?hl=en-US"));
            }
            catch
            {

                return null;
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            XmlNodeList nodeList = doc.SelectNodes("/*/atom:category", nsMgr);
            Log.Info("Youtube - dynamic Categories: " + nodeList.Count);
            Dictionary<String, String> categories = new Dictionary<string, string>();
            foreach (XmlNode node in nodeList)
            {
                categories.Add(node.Attributes["label"].Value, node.Attributes["term"].Value);
            }
            return categories;
        }

        public override bool HasNextPage
        {
            get { return nextPageAvailable; }
        }

        public override List<VideoInfo> getNextPageVideos()
        {
            YouTubeQuery query = _LastPerformedQuery;

            Match mIndex = PageStartIndex.Match(nextPageUrl);
            if (mIndex.Success)
            {
                query.StartIndex = Convert.ToInt16(mIndex.Groups["item"].Value);
            }

            return parseGData(query);
        }

        public override bool HasPreviousPage
        {
            get { return previousPageAvailable; }
        }

        public override List<VideoInfo> getPreviousPageVideos()
        {
            YouTubeQuery query = _LastPerformedQuery;

            Match mIndex = PageStartIndex.Match(previousPageUrl);
            if (mIndex.Success)
            {
                query.StartIndex = Convert.ToInt16(mIndex.Groups["item"].Value);
            }

            return parseGData(query);
        }

        #region IFilter Members

        //private String buildFilterUrl(string catUrl, int maxResult, string orderBy, string timeFrame)
        //{
        //    if (catUrl == null)
        //    {
        //        catUrl = "";
        //    }
        //    String newCatUrl = catUrl;
        //    if (catUrl.IndexOf("time=", StringComparison.CurrentCultureIgnoreCase) > 0)
        //    {
        //        Regex timeRgx = new Regex(@"\?.*time=([^&]*)");
        //        newCatUrl = timeRgx.Replace(catUrl, new MatchEvaluator(delegate(Match match) { return timeFrame; }));
        //    }
        //    else
        //    {
        //        if (catUrl.Contains("?"))
        //        {
        //            newCatUrl += "&";
        //        }
        //        else
        //        {
        //            newCatUrl += "?";
        //        }
        //        newCatUrl += "time=" + timeFrame;
        //    }
        //    if (catUrl.IndexOf("orderby=", StringComparison.CurrentCultureIgnoreCase) > 0)
        //    {
        //        Regex timeRgx = new Regex(@"\?.*orderby=([^&]*)");
        //        newCatUrl = timeRgx.Replace(catUrl, new MatchEvaluator(delegate(Match match) { return orderBy; }));
        //    }
        //    else
        //    {
        //        newCatUrl += "&orderby=" + orderBy;
        //    }
        //    if (catUrl.IndexOf("max-results=", StringComparison.CurrentCultureIgnoreCase) > 0)
        //    {
        //        Regex timeRgx = new Regex(@"\?.*max-results=([^&]*)");
        //        newCatUrl = timeRgx.Replace(catUrl, new MatchEvaluator(delegate(Match match) { return maxResult + ""; }));
        //    }
        //    else
        //    {
        //        newCatUrl += "&max-results=" + maxResult;
        //    }
        //    return newCatUrl;
        //}

        public List<VideoInfo> filterVideoList(Category category, int maxResult, string orderBy, string timeFrame)
        {
            YouTubeQuery query = _LastPerformedQuery;
            query.StartIndex = 1;
            query.NumberToRetrieve = maxResult;
            query.OrderBy = orderBy;

            ///-------------------------------------------------------------------------------------------------
            /// 2009-06-09 MichelC
            /// Youtube doesn't allow the following parameter for Recently Featured clips and return and error.
            ///-------------------------------------------------------------------------------------------------
            if (category.Name != "Recently Featured")
            {
                if (Enum.IsDefined(typeof(YouTubeQuery.UploadTime), timeFrame))
                {
                    query.Time = (YouTubeQuery.UploadTime)Enum.Parse(typeof(YouTubeQuery.UploadTime), timeFrame, true);
                }
            }

            return parseGData(query);
            //String filteredUrl = buildFilterUrl(catUrl, maxResult, orderBy, timeFrame); 
            //Log.Info("Youtube Filtered url:" + filteredUrl);
            //return getVideoList(filteredUrl);
        }

        public List<VideoInfo> filterSearchResultList(string queryStr, int maxResult, string orderBy, string timeFrame)
        {
            //String filteredUrl = buildFilterUrl(buildSearchUrl(query,String.Empty), maxResult, orderBy, timeFrame);
            //Log.Info("Youtube Filtered url:" + filteredUrl);
            //return getVideoList(filteredUrl);
            YouTubeQuery query = _LastPerformedQuery;
            query.StartIndex = 1;
            query.NumberToRetrieve = maxResult;
            query.OrderBy = orderBy;
            if (Enum.IsDefined(typeof(YouTubeQuery.UploadTime), timeFrame))
            {
                query.Time = (YouTubeQuery.UploadTime)Enum.Parse(typeof(YouTubeQuery.UploadTime), timeFrame, true);
            }
            return parseGData(query);
        }

        public List<VideoInfo> filterSearchResultList(string queryStr, string category, int maxResult, string orderBy, string timeFrame)
        {
            //String filteredUrl = buildFilterUrl(buildSearchUrl(query, category), maxResult, orderBy, timeFrame);
            //Log.Info("Youtube Filtered url:" + filteredUrl);
            //return getVideoList(filteredUrl);
            YouTubeQuery query = _LastPerformedQuery;
            query.StartIndex = 1;
            query.NumberToRetrieve = maxResult;
            query.OrderBy = orderBy;
            if (Enum.IsDefined(typeof(YouTubeQuery.UploadTime), timeFrame))
            {
                query.Time = (YouTubeQuery.UploadTime)Enum.Parse(typeof(YouTubeQuery.UploadTime), timeFrame, true);
            }
            return parseGData(query);
        } 

        public List<int> getResultSteps()
        {
            return steps;
        }

        public Dictionary<string, String> getOrderbyList()
        {
            return orderByList;
        }

        public Dictionary<string, String> getTimeFrameList()
        {
            return timeFrameList;
        }

        #endregion

        #region ISearch Members

        public Dictionary<string, string> GetSearchableCategories()
        {
            return getYoutubeCategories();            
        }

        public List<VideoInfo> Search(string queryStr)
        {
            YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
            query.Query = queryStr;           
            List<VideoInfo> loRssItemList = parseGData(query);            
            return loRssItemList;
            
        }
        
        //private String buildSearchUrl(string query, string category)
        //{
        //    String searchUrl;
        //    if (!String.IsNullOrEmpty(category))
        //    {
        //        searchUrl = String.Format("http://gdata.youtube.com/feeds/api/videos?vq={0}&category={1}", query, category);
        //    }
        //    else
        //    {
        //        searchUrl = String.Format("http://gdata.youtube.com/feeds/api/videos?vq={0}", query);
        //    }
        //    return searchUrl;

        //}

        public List<VideoInfo> Search(string queryStr, string category)
        {
            YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
            query.Query = queryStr;  
            AtomCategory category1 = new AtomCategory(category, YouTubeNameTable.CategorySchema);
            query.Categories.Add(new QueryCategory(category1));            
            
            List<VideoInfo> loRssItemList = parseGData(query);
            return loRssItemList;
        }

        #endregion

        #region IFavorite Members

        public List<VideoInfo> getFavorites()
        {
            if (string.IsNullOrEmpty(Settings.Username)) return new List<VideoInfo>();

            //service.setUserCredentials(fsUsername,fsPassword);
            
            YouTubeQuery query =new YouTubeQuery(String.Format(FAVORITE_FEED,Settings.Username));           
        
            return parseGData(query);
        }               

        public void addFavorite(VideoInfo video)
        {
            service.setUserCredentials(Settings.Username, Settings.Password);
            YouTubeEntry entry = (YouTubeEntry)video.Other;
            service.Insert(new Uri(String.Format(FAVORITE_FEED, Settings.Username)), entry);
        //    String lsPostUrl = "http://gdata.youtube.com/feeds/api/users/default/favorites";
        //    String authToken = getAuthToken(fsUsername, fsPassword);
        //    HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(lsPostUrl);
        //    Request.Method = "POST";
        //    Request.ContentType = "application/atom+xml";
        //    Request.Headers.Add(
        //        HttpRequestHeader.Authorization, "GoogleLogin auth=" + authToken);
        //    Request.Headers.Add("X-GData-Client: " + CLIENT_ID);
        //    Request.Headers.Add("X-GData-Key: key=" + DEVELOPER_KEY);
        //    Request.Headers.Add("GData-Version","2");
        //    ASCIIEncoding ASCIIEncoding = new ASCIIEncoding();
        //    Byte [] PostData = ASCIIEncoding.GetBytes(String.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><id>{0}</id></entry>","J2N0t4OEUbc"));
        //     Stream RequestStream = Request.GetRequestStream();
        //    RequestStream.Write(PostData, 0, PostData.Length);
        //    RequestStream.Close();
            
        
        //HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
        //StreamReader Reader  = new StreamReader(response.GetResponseStream());
        //String lsResponse = Reader.ReadToEnd();
        //Log.Info("Youtube authorization token:"+lsResponse);
        //response.Close();
            //throw new Exception("");
        }

        public void removeFavorite(VideoInfo video)
        {
            ((YouTubeEntry)video.Other).Delete();
            //String lsPostUrl = String.Format("http://gdata.youtube.com/feeds/api/users/{0}/favorites/{1}", fsUsername, "vjVQa1PpcFOKheU6YrMZmZ6GRqLUdhAz8qZtu8cCzBs");
            //String authToken = getAuthToken(fsUsername, fsPassword);
            //HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(lsPostUrl);
            //Request.Method = "DELETE";
            //Request.ContentType = "application/atom+xml";
            //Request.Headers.Add(
            //    HttpRequestHeader.Authorization, "GoogleLogin auth=" + authToken);
            //Request.Headers.Add("X-GData-Client: " + CLIENT_ID);
            //Request.Headers.Add("X-GData-Key: key=" + DEVELOPER_KEY);
            //Request.Headers.Add("GData-Version", "2");
         
            //HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
            //StreamReader Reader = new StreamReader(response.GetResponseStream());
            //String lsResponse = Reader.ReadToEnd();
            //Log.Info("Youtube authorization token:" + lsResponse);
            //response.Close();
        }

        #endregion

        //private String getAuthToken(String fsUsername, String fsPassword)
        //{
        //    String lsClientLoginUrl = "https://www.google.com/youtube/accounts/ClientLogin";
        //    HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(lsClientLoginUrl);
        //    Request.Method = "POST";
        //    Request.ContentType = "application/x-www-form-urlencoded";
        //    //Request.CookieContainer = new CookieContainer();
			
        //    //Request.CookieContainer.Add(moCookies);
			
        //    //Stream RequestStream  = Request.GetRequestStream();
        //    ASCIIEncoding ASCIIEncoding  =  new ASCIIEncoding();
            
        //    Byte [] PostData = ASCIIEncoding.GetBytes(
        //        "Email="+fsUsername+
        //        "&Passwd="+fsPassword+
        //        "&service=youtube"+
        //        "&source=OnlineVideos");
        //    /*
                //    Stream RequestStream = Request.GetRequestStream();
        //    RequestStream.Write(PostData, 0, PostData.Length);
        //    RequestStream.Close();
            
        
        //HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
        //StreamReader Reader  = new StreamReader(response.GetResponseStream());
        //String lsResponse = Reader.ReadToEnd();
        //Log.Info("Youtube authorization token:"+lsResponse);
        //response.Close();
        //Regex authRegex = new Regex("Auth=([^\n]*)");
        //return authRegex.Match(lsResponse).Groups[1].Value;

        ////return lsResponse;
        //}

    }

}
