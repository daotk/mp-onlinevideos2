using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using HtmlAgilityPack;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace OnlineVideos.Sites
{
    public class SVTPlayUtil : LatestVideosSiteUtilBase
    {
        public enum JaNej { Ja, Nej };
        protected const string _oppetArkiv = "�ppet arkiv";
        protected const string _programA_O = "Program A-�";
        protected const string _latestVideos = "Senaste program";

        protected string nextPageUrl = "";

        [Category("OnlineVideosConfiguration"), Description("Url used for prepending relative links.")]
        protected string baseUrl;
        [Category("OnlineVideosConfiguration"), Description("Url used for prepending relative links.")]
        protected string oppetArkivListUrl;
        [Category("OnlineVideosConfiguration"), Description("hdcore value for manifest-urls")]
        protected string hdcore;
        [Category("OnlineVideosConfiguration"), Description("swfUrl")]
        protected string swfUrl;

        [Category("OnlineVideosUserConfiguration"), LocalizableDisplayName("H�mta undertexter"), Description("V�lj om du vill h�mta eventuella undertexter")]
        protected JaNej retrieveSubtitles = JaNej.Ja;
        [Category("OnlineVideosUserConfiguration"), LocalizableDisplayName("Gruppera efter begynnelsebokstav"), Description("V�lj om du vill gruppera programlistningar efter begynnelsebokstav eller inte.")]
        protected JaNej splitByLetter = JaNej.Ja;

        protected bool RetrieveSubtitles
        {
            get { return retrieveSubtitles == JaNej.Ja; }
        }

        protected bool SplitByLetter
        {
            get { return splitByLetter == JaNej.Ja; }
        }

        #region category

        private RssLink DiscoverCategoryFromArticle(HtmlNode article)
        {
            RssLink cat = new RssLink();
            cat.Description = HttpUtility.HtmlDecode(article.GetAttributeValue("data-description", ""));
            HtmlNode a = article.Descendants("a").First();
            Uri uri = new Uri(new Uri(baseUrl), a.GetAttributeValue("href", ""));
            cat.Url = uri.ToString();
            IEnumerable<HtmlNode> imgs = a.Descendants("img");
            if (imgs != null && imgs.Count() > 0)
            {
                uri = new Uri(new Uri(baseUrl), a.Descendants("img").First().GetAttributeValue("src", ""));
                cat.Thumb = uri.ToString();
            }
            HtmlNode fcap = a.SelectSingleNode(".//figcaption");
            if (fcap != null)
                cat.Name = HttpUtility.HtmlDecode(fcap.InnerText);
            else
                cat.Name = HttpUtility.HtmlDecode(article.GetAttributeValue("data-title", ""));
            cat.Name = (cat.Name ?? "").Trim();
            if (cat.Name.ToLower().Contains("oppetarkiv"))
            {
                cat.Name = _oppetArkiv;
                cat.Url = oppetArkivListUrl;
            }
            return cat;
        }

        private List<Category> DiscoverProgramAOCategories(HtmlNode htmlNode, Category parentCategory)
        {
            List<Category> categories = new List<Category>();
            IEnumerable<HtmlNode> alphabetList = htmlNode.Descendants("li").Where(d => d.GetAttributeValue("class", "").StartsWith("play_alphabetic-list"));
            foreach (HtmlNode alphaLi in alphabetList)
            {
                Category alphaCat = new Category() { Name = HttpUtility.HtmlDecode(alphaLi.SelectSingleNode("h3").InnerText), SubCategories = new List<Category>(), HasSubCategories = true, ParentCategory = parentCategory };
                HtmlNodeCollection programs = alphaLi.SelectNodes("ul/li");
                if (programs != null)
                {
                    foreach (HtmlNode program in programs)
                    {
                        HtmlNode a = program.SelectSingleNode("a");
                        Uri uri = new Uri(new Uri(baseUrl), a.GetAttributeValue("href", ""));
                        RssLink programCat = new RssLink() { Name = HttpUtility.HtmlDecode(a.InnerText), Url = uri.ToString(), HasSubCategories = true, ParentCategory = SplitByLetter ? alphaCat : parentCategory };
                        if (SplitByLetter)
                            alphaCat.SubCategories.Add(programCat);
                        else
                            categories.Add(programCat);
                    }
                }
                if (SplitByLetter && alphaCat.SubCategories.Count > 0)
                {
                    alphaCat.SubCategoriesDiscovered = true;
                    categories.Add(alphaCat);
                }
            }
            return categories;
        }

        private List<Category> DiscoverOppetArkivCategories(HtmlNode htmlNode, Category parentCategory)
        {
            List<Category> categories = new List<Category>();
            HtmlNode div = htmlNode.Descendants("div").First(d => d.GetAttributeValue("role", "") == "main");
            foreach (HtmlNode alphaSec in div.SelectNodes("section"))
            {
                Category alphaCat = new Category() { Name = HttpUtility.HtmlDecode(alphaSec.SelectSingleNode("h2/a").InnerText), SubCategories = new List<Category>(), HasSubCategories = true, ParentCategory = parentCategory };
                HtmlNodeCollection programs = alphaSec.SelectNodes("ul/li");
                if (programs != null)
                {
                    foreach (HtmlNode program in programs)
                    {
                        HtmlNode a = program.SelectSingleNode("a");
                        Uri uri = new Uri(new Uri(baseUrl), a.GetAttributeValue("href", ""));
                        RssLink programCat = new RssLink() { Name = HttpUtility.HtmlDecode(a.InnerText), Url = uri.ToString(), HasSubCategories = false, ParentCategory = SplitByLetter ? alphaCat : parentCategory };
                        if (SplitByLetter)
                            alphaCat.SubCategories.Add(programCat);
                        else
                            categories.Add(programCat);
                    }
                }
                if (SplitByLetter && alphaCat.SubCategories.Count > 0)
                {
                    alphaCat.SubCategoriesDiscovered = true;
                    categories.Add(alphaCat);
                }
            }
            return categories;
        }

        public override int DiscoverDynamicCategories()
        {
            Settings.Categories.First(c => c.Name == _programA_O).HasSubCategories = true;
            HtmlNode htmlNode = GetWebData<HtmlDocument>(baseUrl).DocumentNode;
            foreach (HtmlNode section in htmlNode.Descendants("section").Where(n => n.GetAttributeValue("class", "").Contains("play_js-hovered-list")))
            {
                HtmlNode div = section.SelectSingleNode("div");
                RssLink cat = new RssLink();
                cat.Name = HttpUtility.HtmlDecode(div.SelectSingleNode("div/h1").InnerText).Trim();
                cat.HasSubCategories = div.GetAttributeValue("id", "") == "categories";
                if (cat.HasSubCategories)
                {
                    cat.SubCategories = new List<Category>();
                    foreach (HtmlNode article in section.Descendants("article"))
                    {
                        Category subCat = DiscoverCategoryFromArticle(article);
                        subCat.HasSubCategories = true;
                        subCat.ParentCategory = cat;
                        cat.SubCategories.Add(subCat);
                    }
                    cat.SubCategoriesDiscovered = cat.SubCategories.Count > 0;
                }
                else
                {
                    List<VideoInfo> videos = new List<VideoInfo>();
                    foreach (HtmlNode article in section.Descendants("article"))
                    {
                        videos.Add(getVideoFromArticle(article));
                    }
                    cat.Other = videos;
                    cat.EstimatedVideoCount = (uint)videos.Count;
                }
                Settings.Categories.Add(cat);
            }
            Settings.Categories.Add(new Category()
            {
                Name = "Kanaler",
                HasSubCategories = false
            });
            Settings.DynamicCategoriesDiscovered = Settings.Categories.Count > 0;
            return Settings.Categories.Count;
        }

        public override int DiscoverSubCategories(Category parentCategory)
        {
            List<Category> categories = new List<Category>();
            HtmlNode htmlNode = GetWebData<HtmlDocument>((parentCategory as RssLink).Url).DocumentNode;
            if (parentCategory.Name == _programA_O)
            {
                categories = DiscoverProgramAOCategories(htmlNode, parentCategory);
            }
            else if (parentCategory.Name == _oppetArkiv)
            {
                categories = DiscoverOppetArkivCategories(htmlNode, parentCategory);
            }
            else
            {
                IEnumerable<HtmlNode> categoryNodes = htmlNode.Descendants("li").Where(li => li.GetAttributeValue("class", "").Contains("play_category__tab-list-item"));
                if (categoryNodes == null || categoryNodes.Count() == 0)
                    categoryNodes = htmlNode.Descendants("li").Where(li => li.GetAttributeValue("class", "").Contains("play_list__item"));
                if (categoryNodes == null || categoryNodes.Count() == 0)
                    categoryNodes = htmlNode.Descendants("li").Where(li => li.GetAttributeValue("class", "").Contains("play_tab-list__item"));

                foreach (HtmlNode categoryNode in categoryNodes)
                {
                    string ariaControls = categoryNode.SelectSingleNode("a").GetAttributeValue("aria-controls", "");
                    HtmlNode div = htmlNode.SelectSingleNode("//div[@id = '" + ariaControls + "']");

                    RssLink category = new RssLink() { Name = HttpUtility.HtmlDecode(categoryNode.SelectSingleNode("a").InnerText.Trim()), ParentCategory = parentCategory };
                    if (category.Name == _programA_O)
                    {
                        IEnumerable<HtmlNode> alphaList = div.Descendants("li").Where(d => d.GetAttributeValue("class", "").Contains("alphabetic-list-item") && !d.GetAttributeValue("class", "").Contains("play_is-hidden"));
                        bool split = SplitByLetter && alphaList != null && alphaList.Count() > 0;
                        category.HasSubCategories = true;
                        category.SubCategories = new List<Category>();
                        if (split)
                        {
                            foreach (HtmlNode alphaDiv in alphaList)
                            {
                                Category alphaCat = new Category() { Name = HttpUtility.HtmlDecode(alphaDiv.SelectSingleNode("h3").InnerText), ParentCategory = category, HasSubCategories = true, SubCategories = new List<Category>() };
                                foreach (HtmlNode article in alphaDiv.Descendants("article"))
                                {
                                    RssLink subCat = DiscoverCategoryFromArticle(article);
                                    subCat.HasSubCategories = true;
                                    subCat.ParentCategory = alphaCat;
                                    alphaCat.SubCategories.Add(subCat);
                                }
                                alphaCat.SubCategoriesDiscovered = alphaCat.SubCategories.Count > 0;
                                category.SubCategories.Add(alphaCat);
                            }

                        }
                        else
                        {
                            foreach (HtmlNode article in div.Descendants("article"))
                            {
                                RssLink subCat = DiscoverCategoryFromArticle(article);
                                subCat.HasSubCategories = true;
                                subCat.ParentCategory = category;
                                category.SubCategories.Add(subCat);
                            }
                        }
                        category.SubCategoriesDiscovered = category.SubCategories.Count > 0;
                    }
                    else
                    {
                        category.HasSubCategories = false;
                        List<VideoInfo> videos = new List<VideoInfo>();
                        foreach (HtmlNode article in div.Descendants("article"))
                        {
                            videos.Add(getVideoFromArticle(article));
                        }
                        category.Other = videos;
                        category.EstimatedVideoCount = (uint)videos.Count;
                    }
                    categories.Add(category);
                }
            }
            parentCategory.SubCategories = categories;
            parentCategory.SubCategoriesDiscovered = categories.Count > 0;
            return categories.Count;
        }

        #endregion

        #region video

        private VideoInfo getVideoFromArticle(HtmlNode article)
        {
            VideoInfo video = new VideoInfo();
            string title = article.GetAttributeValue("data-title", "");
            if (string.IsNullOrWhiteSpace(title))
            {
                //A-� listings
                HtmlNode a = article.SelectSingleNode(".//a[contains(@class,'header-link')]");
                if (a != null)
                {
                    video.Title = a.InnerText;
                    video.VideoUrl = a.GetAttributeValue("href", "");
                    HtmlNode p = article.SelectSingleNode(".//p[contains(@class,'description-text')]");
                    video.Description = ""; 
                    if (p != null)
                        video.Description = HttpUtility.HtmlDecode(p.InnerText);
                    p = article.SelectSingleNode(".//p[contains(@class,'expire-date')]");
                    if (p != null)
                        video.Description += "\r\n" + HttpUtility.HtmlDecode(p.InnerText.Trim());
                    p = article.SelectSingleNode(".//p[contains(@class,'meta-info')]");
                    if (p != null)
                        video.Airdate = HttpUtility.HtmlDecode(p.InnerText.Trim());
                    HtmlNode time = article.SelectSingleNode(".//time");
                    if (time != null)
                        video.Length = time.InnerText;
                    HtmlNode img = article.SelectSingleNode(".//img");
                    if (img != null)
                    {
                        video.Thumb = img.GetAttributeValue("src", "");
                        string alt = HttpUtility.HtmlDecode(img.GetAttributeValue("alt", ""));
                        video.Title = string.IsNullOrWhiteSpace(alt) ? video.Title : alt + " - " + video.Title;
                    }
                    if (video.Title.ToLower().StartsWith("spela"))
                        video.Title = video.Title.Substring(5).Trim();
                }
            }
            else
            {
                HtmlNode playLinkSubNode = article.Descendants("span").FirstOrDefault(s => s.GetAttributeValue("class", "") == "play-link-sub");
                if (playLinkSubNode != null)
                {
                    string playLinkSub = playLinkSubNode.InnerText.Trim().Replace('\n', ' ');
                    if (playLinkSub != "" && !title.Contains(playLinkSub))
                    {
                        title = playLinkSub + " - " + title;
                    }
                }
                video.Title = title;
                video.Description = article.GetAttributeValue("data-description", "");
                video.Airdate = HttpUtility.HtmlDecode(article.GetAttributeValue("data-broadcasted", ""));
                video.Length = article.GetAttributeValue("data-length", "");
                HtmlNode a = article.SelectSingleNode("a");
                Uri uri = new Uri(new Uri(baseUrl), a.GetAttributeValue("href", ""));
                video.VideoUrl = uri.ToString();
                HtmlNode img = a.Descendants("img").FirstOrDefault(i => !string.IsNullOrEmpty(i.GetAttributeValue("data-imagename", "")));
                if (img == null)
                {
                    img = a.Descendants("img").FirstOrDefault(i => !string.IsNullOrEmpty(i.GetAttributeValue("src", "")));
                    if (img != null)
                        video.Thumb = img.GetAttributeValue("src", "");
                }
                else
                {
                    video.Thumb = img.GetAttributeValue("data-imagename", "");
                }
            }
            video.CleanDescriptionAndTitle();
            return video;
        }

        private List<VideoInfo> getOppetArkivVideoList(HtmlAgilityPack.HtmlNode node)
        {
            List<VideoInfo> videoList = new List<VideoInfo>();
            var div = node.SelectSingleNode("//div[contains(@class,'svtGridBlock')]");
            foreach (var article in div.Elements("article"))
            {
                VideoInfo video = new VideoInfo();
                video.VideoUrl = article.Descendants("a").Select(a => a.GetAttributeValue("href", "")).FirstOrDefault();
                if (!string.IsNullOrEmpty(video.VideoUrl))
                {
                    video.Title = HttpUtility.HtmlDecode((article.Descendants("a").Select(a => a.GetAttributeValue("title", "")).FirstOrDefault() ?? "").Trim().Replace('\n', ' '));
                    video.Thumb = article.Descendants("img").Select(i => i.GetAttributeValue("src", "")).FirstOrDefault();
                    video.Airdate = article.Descendants("time").Select(t => t.GetAttributeValue("datetime", "")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(video.Airdate)) video.Airdate = DateTime.Parse(video.Airdate).ToString("d", OnlineVideoSettings.Instance.Locale);
                    videoList.Add(video);
                }
            }
            return videoList;
        }

        private void getNextPageVideosUrl(HtmlAgilityPack.HtmlNode node)
        {
            HasNextPage = false;
            nextPageUrl = "";
            var a_o_buttons = node.SelectNodes("//a[contains(@class, 'svtoa-button')]");
            if (a_o_buttons != null)
            {
                var a_o_next_button = a_o_buttons.Where(a => (a.InnerText ?? "").Contains("Visa fler")).FirstOrDefault();
                if (a_o_next_button != null)
                {
                    nextPageUrl = a_o_next_button.GetAttributeValue("href", "");
                    nextPageUrl = HttpUtility.UrlDecode(nextPageUrl);
                    nextPageUrl = HttpUtility.HtmlDecode(nextPageUrl); //Some urls come html encoded
                    HasNextPage = true;
                }
            }
        }

        public override List<VideoInfo> GetNextPageVideos()
        {
            HasNextPage = false;
            if (!string.IsNullOrEmpty(nextPageUrl))
            {
                HtmlNode htmlNode = GetWebData<HtmlDocument>(nextPageUrl).DocumentNode;
                getNextPageVideosUrl(htmlNode);
                return getOppetArkivVideoList(htmlNode);
            }
            return new List<VideoInfo>();
        }

        public override List<VideoInfo> GetVideos(Category category)
        {
            if (category.Other is List<VideoInfo>)
            {
                return category.Other as List<VideoInfo>;
            }
            else if (category.ParentCategory == null && category.Name == "Kanaler")
            {
                List < VideoInfo >  videos = new List<VideoInfo>()
                {
                    new VideoInfo() {Title = "SVT1"},
                    new VideoInfo() {Title = "SVT2"},
                    new VideoInfo() {Title = "Barnkanalen"},
                    new VideoInfo() {Title = "SVT24"},
                    new VideoInfo() {Title = "Kunskapskanalen"}
                };
                videos.ForEach(v => 
                {
                    v.VideoUrl = string.Format("http://www.svtplay.se/kanaler/{0}", v.Title.ToLower());
                });
                return videos;
            }
            else
            {
                var htmlNode = GetWebData<HtmlDocument>((category as RssLink).Url).DocumentNode;
                getNextPageVideosUrl(htmlNode);
                return getOppetArkivVideoList(htmlNode);
            }
        }

        public override string GetVideoUrl(VideoInfo video)
        {
            string url = "";
            Uri result;
            if (!Uri.TryCreate(video.VideoUrl, UriKind.Absolute, out result))
                Uri.TryCreate(new Uri(baseUrl), video.VideoUrl, out result);
            video.VideoUrl = result.ToString();
            JToken videoToken = GetWebData<JObject>(video.VideoUrl + "?output=json")["video"];
            if (RetrieveSubtitles)
            {
                try
                {
                    var subtitleReferences = videoToken["subtitleReferences"].Where(sr => ((string)sr["url"] ?? "").EndsWith("srt"));
                    if (subtitleReferences != null && subtitleReferences.Count() > 0)
                    {
                        url = (string)subtitleReferences.First()["url"];
                        if (!string.IsNullOrEmpty(url))
                        {
                            video.SubtitleText = CleanSubtitle(GetWebData(url));
                        }
                    }
                }
                catch { }
            }
            JToken videoReference = videoToken["videoReferences"].FirstOrDefault(vr => (string)vr["playerType"] == "flash" && !string.IsNullOrEmpty((string)vr["url"]));
            if (videoReference == null)
            {
                url = "";
            }
            else
            {
                Boolean live = false;
                JValue liveVal = (JValue)videoToken["live"];
                if (liveVal != null)
                    live = liveVal.Value<bool>();
                url = (string)videoReference["url"] + "?hdcore=" + hdcore + "&g=" + OnlineVideos.Sites.Utils.HelperUtils.GetRandomChars(12);
                url = new MPUrlSourceFilter.AfhsManifestUrl(url)
                {
                    LiveStream = live,
                    Referer = swfUrl
                }.ToString();
            }
            return url;
        }

        public override ITrackingInfo GetTrackingInfo(VideoInfo video)
        {
            Regex rgx = new Regex(@"(?<VideoKind>TvSeries)(?<Title>[^-]*).*?[Ss]�song.*?(?<Season>\d+).*?[Aa]vsnitt.*?(?<Episode>\d+)");
            Match m = rgx.Match("TvSeries" + video.Title);
            ITrackingInfo ti = new TrackingInfo() { Regex = m };
            return ti;
        }

        #endregion

        #region search

        public override bool CanSearch
        {
            get
            {
                return true;
            }
        }

        public override List<SearchResultItem> Search(string query, string category = null)
        {
            List<SearchResultItem> results = new List<SearchResultItem>();
            string[] subcats = { "search-categories", "search-titles", "" };
            HtmlNode htmlNode = GetWebData<HtmlDocument>(baseUrl + "sok?q=" + HttpUtility.UrlEncode(query)).DocumentNode;
            foreach (HtmlNode section in htmlNode.Descendants("section").Where(n => n.GetAttributeValue("class", "").Contains("play_js-hovered-list")))
            {
                HtmlNode div = section.SelectSingleNode("div");
                RssLink cat = new RssLink();
                cat.Name = div.SelectSingleNode("div/h1").InnerText.Trim();
                if (cat.Name.ToLower().Contains("oppetarkiv"))
                    cat.Name = _oppetArkiv;
                cat.HasSubCategories = subcats.Any(c => c == div.GetAttributeValue("id", ""));
                if (cat.HasSubCategories)
                {
                    cat.SubCategories = new List<Category>();
                    foreach (HtmlNode article in section.Descendants("article"))
                    {
                        Category subCat = DiscoverCategoryFromArticle(article);
                        subCat.HasSubCategories = true;
                        subCat.ParentCategory = cat;
                        cat.SubCategories.Add(subCat);
                    }
                    cat.SubCategoriesDiscovered = cat.SubCategories.Count > 0;
                }
                else
                {
                    List<VideoInfo> videos = new List<VideoInfo>();
                    foreach (HtmlNode article in section.Descendants("article"))
                    {
                        videos.Add(getVideoFromArticle(article));
                    }
                    cat.Other = videos;
                    cat.EstimatedVideoCount = (uint)videos.Count;
                }
                results.Add(cat);
            }
            return results;
        }

        public override string GetFileNameForDownload(VideoInfo video, Category category, string url)
        {
            //Extension always .f4m
            return Helpers.FileUtils.GetSaveFilename(video.Title) + ".f4m";
        }

        #endregion

        #region subtitle

        string CleanSubtitle(string subtitle)
        {
            // For some reason the time codes in the subtitles from �ppet arkiv starts @ 10 hours. replacing first number in the
            // hour position with 0. Hope and pray there will not be any shows with 10+ h playtime...
            // Remove all trailing stuff, ie in 00:45:21.960 --> 00:45:25.400 A:end L:82%
            Regex rgx = new Regex(@"\d(\d:\d\d:\d\d\.\d\d\d)\s*-->\s*\d(\d:\d\d:\d\d\.\d\d\d).*$", RegexOptions.Multiline);
            subtitle = rgx.Replace(subtitle, new MatchEvaluator((Match m) =>
            {
                return "0" + m.Groups[1].Value + " --> 0" + m.Groups[2].Value + "\r";
            }));

            // Removes color codes, ie <36>....</36>. Can't use XDocument to remove all tags since <36> is invalid xml (parse throws)
            // I haven't noticed any other tags or strange WebSrt stuff, so keeping it simple. 
            rgx = new Regex(@"</{0,1}\d\d>");
            return rgx.Replace(subtitle, string.Empty);
        }

        #endregion

        #region LatestVideos

        public override List<VideoInfo> GetLatestVideos()
        {
            HtmlNode htmlNode = GetWebData<HtmlDocument>(baseUrl).DocumentNode;
            HtmlNode div = htmlNode.Descendants("div").FirstOrDefault(d => d.GetAttributeValue("id", "") == "latest-videos");
            List<VideoInfo> videos = new List<VideoInfo>();
            if (div != null)
            {
                foreach (HtmlNode article in div.Descendants("article"))
                {
                    videos.Add(getVideoFromArticle(article));
                }
            }
            return videos.Count >= LatestVideosCount ? videos.GetRange(0, (int)LatestVideosCount) : new List<VideoInfo>();
        }

        #endregion
    }
}
