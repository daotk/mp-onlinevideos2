﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Diagnostics;
using OnlineVideos;
using OnlineVideos.Sites;

namespace SiteParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            generic = new GenericSiteUtil();
            generic.Initialize(new SiteSettings());
            generic.Settings.Name = "please fill";
            generic.Settings.Description = "please fill";
            generic.Settings.Language = "please fill";
            generic.Settings.UtilName = "GenericSite";
            foreach (PlayerType pt in Enum.GetValues(typeof(PlayerType)))
                playerComboBox.Items.Add(pt);
            foreach (GenericSiteUtil.HosterResolving pt in Enum.GetValues(typeof(GenericSiteUtil.HosterResolving)))
                comboBoxResolving.Items.Add(pt);
            playerComboBox.SelectedIndex = 0;

            UtilToGui(generic);
#if !DEBUG
            debugToolStripMenuItem.Visible = false;
#endif
        }

        System.ComponentModel.BindingList<SiteSettings> SiteSettingsList;
        GenericSiteUtil generic;
        List<RssLink> staticList = new List<RssLink>();


        private void UtilToGui(GenericSiteUtil util)
        {
            nameTextBox.Text = util.Settings.Name;

            BaseUrlTextbox.Text = (string)GetProperty(util, "baseUrl");
            descriptionTextBox.Text = util.Settings.Description;
            playerComboBox.SelectedIndex = playerComboBox.Items.IndexOf(util.Settings.Player);
            ageCheckBox.Checked = util.Settings.ConfirmAge;
            languageTextBox.Text = util.Settings.Language;

            CategoryRegexTextbox.Text = GetRegex(util, "regEx_dynamicCategories");
            dynamicCategoryUrlFormatTextBox.Text = (string)GetProperty(util, "dynamicCategoryUrlFormatString");
            dynamicCategoryUrlDecodingCheckBox.Checked = (bool)GetProperty(util, "dynamicCategoryUrlDecoding");
            CategoryNextPageRegexTextBox.Text = (string)GetRegex(util, "regEx_dynamicCategoriesNextPage");

            SubcategoryRegexTextBox.Text = GetRegex(util, "regEx_dynamicSubCategories");
            SubcategoryUrlFormatTextBox.Text = (string)GetProperty(util, "dynamicSubCategoryUrlFormatString");
            dynamicSubCategoryUrlDecodingCheckBox.Checked = (bool)GetProperty(util, "dynamicSubCategoryUrlDecoding");
            SubcategoriesNextPageRegexTextBox.Text = (string)GetRegex(util, "regEx_dynamicSubCategoriesNextPage");

            videoListRegexTextBox.Text = GetRegex(util, "regEx_VideoList");
            videoListRegexFormatTextBox.Text = (string)GetProperty(util, "videoListRegExFormatString");

            videoThumbFormatStringTextBox.Text = (string)GetProperty(util, "videoThumbFormatString");

            nextPageRegExTextBox.Text = GetRegex(util, "regEx_NextPage");
            nextPageRegExUrlFormatStringTextBox.Text = (string)GetProperty(util, "nextPageRegExUrlFormatString");
            nextPageRegExUrlDecodingCheckBox.Checked = (bool)GetProperty(util, "nextPageRegExUrlDecoding");

            videoUrlRegExTextBox.Text = GetRegex(util, "regEx_VideoUrl");
            videoUrlFormatStringTextBox.Text = (string)GetProperty(util, "videoUrlFormatString");
            videoListUrlDecodingCheckBox.Checked = (bool)GetProperty(util, "videoListUrlDecoding");
            videoUrlDecodingCheckBox.Checked = (bool)GetProperty(util, "videoUrlDecoding");

            playlistUrlRegexTextBox.Text = GetRegex(util, "regEx_PlaylistUrl");
            playlistUrlFormatStringTextBox.Text = (string)GetProperty(util, "playlistUrlFormatString");

            fileUrlRegexTextBox.Text = GetRegex(util, "regEx_FileUrl");
            fileUrlFormatStringTextBox.Text = (string)GetProperty(util, "fileUrlFormatString");
            fileUrlPostStringTextBox.Text = (string)GetProperty(util, "fileUrlPostString");
            fileUrlNameFormatStringTextBox.Text = (string)GetProperty(util, "fileUrlNameFormatString");
            getRedirectedFileUrlCheckBox.Checked = (bool)GetProperty(util, "getRedirectedFileUrl");
            comboBoxResolving.SelectedItem = (GenericSiteUtil.HosterResolving)GetProperty(util, "resolveHoster");

            treeView1.Nodes.Clear();
            TreeNode root = treeView1.Nodes.Add("site");
            foreach (Category cat in staticList)
            {
                root.Nodes.Add(cat.Name).Tag = cat;
                cat.HasSubCategories = true;
            }

        }

        private void GuiToUtil(GenericSiteUtil util)
        {
            util.Settings.Name = nameTextBox.Text;
            SetProperty(util, "baseUrl", BaseUrlTextbox.Text);
            util.Settings.Description = descriptionTextBox.Text;
            util.Settings.Player = (PlayerType)playerComboBox.SelectedItem;
            util.Settings.ConfirmAge = ageCheckBox.Checked;
            util.Settings.Language = languageTextBox.Text;

            SetRegex(util, "regEx_dynamicCategories", "dynamicCategoriesRegEx", CategoryRegexTextbox.Text);
            SetProperty(util, "dynamicCategoryUrlFormatString", dynamicCategoryUrlFormatTextBox.Text);
            SetProperty(util, "dynamicCategoryUrlDecoding", dynamicCategoryUrlDecodingCheckBox.Checked);
            SetRegex(util, "regEx_dynamicCategoriesNextPage", "dynamicCategoriesNextPageRegEx", CategoryNextPageRegexTextBox.Text);

            SetRegex(util, "regEx_dynamicSubCategories", "dynamicSubCategoriesRegEx", SubcategoryRegexTextBox.Text);
            SetProperty(util, "dynamicSubCategoryUrlFormatString", SubcategoryUrlFormatTextBox.Text);
            SetProperty(util, "dynamicSubCategoryUrlDecoding", dynamicSubCategoryUrlDecodingCheckBox.Checked);
            SetRegex(util, "regEx_dynamicSubCategoriesNextPage", "dynamicSubCategoriesNextPageRegEx", SubcategoriesNextPageRegexTextBox.Text);

            SetRegex(util, "regEx_VideoList", "videoListRegEx", videoListRegexTextBox.Text);
            SetProperty(util, "videoListRegExFormatString", videoListRegexFormatTextBox.Text);

            SetProperty(util, "videoThumbFormatString", videoThumbFormatStringTextBox.Text);

            SetRegex(util, "regEx_NextPage", "nextPageRegEx", nextPageRegExTextBox.Text);
            SetProperty(util, "nextPageRegExUrlFormatString", nextPageRegExUrlFormatStringTextBox.Text);
            SetProperty(util, "nextPageRegExUrlDecoding", nextPageRegExUrlDecodingCheckBox.Checked);

            SetRegex(util, "regEx_VideoUrl", "videoUrlRegEx", videoUrlRegExTextBox.Text);
            SetProperty(util, "videoUrlFormatString", videoUrlFormatStringTextBox.Text);
            SetProperty(util, "videoListUrlDecoding", videoListUrlDecodingCheckBox.Checked);
            SetProperty(util, "videoUrlDecoding", videoUrlDecodingCheckBox.Checked);

            SetRegex(util, "regEx_PlaylistUrl", "playlistUrlRegEx", playlistUrlRegexTextBox.Text);
            SetProperty(util, "playlistUrlFormatString", playlistUrlFormatStringTextBox.Text);

            SetRegex(util, "regEx_FileUrl", "fileUrlRegEx", fileUrlRegexTextBox.Text);
            SetProperty(util, "fileUrlFormatString", fileUrlFormatStringTextBox.Text);
            SetProperty(util, "fileUrlPostString", fileUrlPostStringTextBox.Text);
            SetProperty(util, "fileUrlNameFormatString", fileUrlNameFormatStringTextBox.Text);
            SetProperty(util, "getRedirectedFileUrl", getRedirectedFileUrlCheckBox.Checked);
            SetProperty(util, "resolveHoster", comboBoxResolving.SelectedItem);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selected = treeView1.SelectedNode;
            if (selected.Tag is Category)
                ShowCategoryInfo((Category)selected.Tag);
            if (selected.Tag is VideoInfo)
                ShowVideoInfo((VideoInfo)selected.Tag);
        }

        object GetTreeViewSelectedNode()
        {
            TreeNode selected = treeView1.SelectedNode;
            if (selected == null)
            {
                MessageBox.Show("nothing selected");
                return null;
            }
            return selected.Tag;
        }

        private void ShowCategoryInfo(Category cat)
        {
            categoryInfoListView.Items.Clear();
            categoryInfoListView.Items.Add("Name").SubItems.Add(cat.Name);
            categoryInfoListView.Items.Add("Url").SubItems.Add(((RssLink)cat).Url);
            categoryInfoListView.Items.Add("Thumb").SubItems.Add(cat.Thumb);
            categoryInfoListView.Items.Add("Descr").SubItems.Add(cat.Description);
        }

        private void ShowVideoInfo(VideoInfo video)
        {
            categoryInfoListView.Items.Clear();
            categoryInfoListView.Items.Add("Title").SubItems.Add(video.Title);
            categoryInfoListView.Items.Add("VideoUrl").SubItems.Add(video.VideoUrl);
            categoryInfoListView.Items.Add("ImageUrl").SubItems.Add(video.ImageUrl);
            categoryInfoListView.Items.Add("Descr").SubItems.Add(video.Description);
            categoryInfoListView.Items.Add("Length").SubItems.Add(video.Length);
        }

        #region BaseUrl
        private void BaseUrlTextbox_TextChanged(object sender, EventArgs e)
        {
            SetProperty(generic, "baseUrl", ((TextBox)sender).Text);
        }
        #endregion

        #region Category
        private void CreateCategoryRegexButton_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            CategoryRegexTextbox.Text = f2.Execute(CategoryRegexTextbox.Text, BaseUrlTextbox.Text,
                new string[] { "url", "title", "thumb", "description" });
        }

        private void GetCategoriesButton_Click(object sender, EventArgs e)
        {
            //get categories
            GuiToUtil(generic);
            generic.Settings.Categories.Clear();
            foreach (Category cat in staticList)
                generic.Settings.Categories.Add(cat);

            if (GetRegex(generic, "regEx_dynamicCategories") != null)
                generic.DiscoverDynamicCategories();
            treeView1.Nodes.Clear();
            TreeNode root = treeView1.Nodes.Add("site");
            foreach (Category cat in generic.Settings.Categories)
            {
                root.Nodes.Add(cat.Name).Tag = cat;
                cat.HasSubCategories = true;
            }
        }

        private void CreateCategoryNextPageRegexButton_Click(object sender, EventArgs e)
        {
            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                Form2 f2 = new Form2();
                CategoryNextPageRegexTextBox.Text = f2.Execute(CategoryNextPageRegexTextBox.Text, ((RssLink)parentCat).Url,
                    new string[] { "url" });
            }
            else
                MessageBox.Show("no valid category selected");
        }

        private void manageStaticCategoriesButton_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            staticList = f3.Execute(staticList);
            GetCategoriesButton_Click(sender, e);
        }

        private void makeStaticButton_Click(object sender, EventArgs e)
        {
            foreach (Category cat in generic.Settings.Categories)
                staticList.Add(cat as RssLink);
        }

        #endregion

        #region SubCategories
        private void CreateSubcategoriesRegexButton_Click(object sender, EventArgs e)
        {
            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                Form2 f2 = new Form2();
                SubcategoryRegexTextBox.Text = f2.Execute(SubcategoryRegexTextBox.Text, ((RssLink)parentCat).Url,
                    new string[] { "url", "title", "thumb", "description" });
            }
            else
                MessageBox.Show("no valid category selected");
        }

        private void GetSubCategoriesButton_Click(object sender, EventArgs e)
        {
            //subcategories
            GuiToUtil(generic);

            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null && (parentCat.HasSubCategories || parentCat is NextPageCategory))
            {
                TreeNode selected = treeView1.SelectedNode;
                if (parentCat is NextPageCategory)
                {
                    selected = selected.Parent;
                    selected.Nodes.RemoveAt(selected.Nodes.Count - 1);
                }
                else
                    selected.Nodes.Clear();
                generic.DiscoverSubCategories(parentCat);
                foreach (Category cat in parentCat.SubCategories)
                {
                    selected.Nodes.Add(cat.Name).Tag = cat;
                    cat.HasSubCategories = false;
                }
            }
            else
                MessageBox.Show("no valid category selected");
        }

        private void CreateSubcategoriesNextPageRegexButton_Click(object sender, EventArgs e)
        {
            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                Form2 f2 = new Form2();
                SubcategoriesNextPageRegexTextBox.Text = f2.Execute(SubcategoriesNextPageRegexTextBox.Text, ((RssLink)parentCat).Url,
                    new string[] { "url" });
            }
            else
                MessageBox.Show("no valid category selected");
        }

        private void manageStaticSubCategoriesButton_Click(object sender, EventArgs e)
        {

            Form3 f3 = new Form3();
            RssLink parentCat = GetTreeViewSelectedNode() as RssLink;
            if (parentCat != null && staticList.Contains(parentCat))
            {
                List<RssLink> subcats = new List<RssLink>();
                foreach (RssLink tmp in parentCat.SubCategories)
                    subcats.Add(tmp);
                parentCat.SubCategories = new List<Category>(f3.Execute(subcats).ToArray());
                GetSubCategoriesButton_Click(sender, e);
            }
            else
                MessageBox.Show("no valid (static) category selected");
        }

        #endregion

        #region VideoList
        private void CreateVideoListRegexButton_Click(object sender, EventArgs e)
        {
            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                Form2 f2 = new Form2();
                videoListRegexTextBox.Text = f2.Execute(videoListRegexTextBox.Text, ((RssLink)parentCat).Url,
                    new string[] { "Title", "VideoUrl", "ImageUrl", "Description", "Duration", "Airdate" });
            }
            else
                MessageBox.Show("no valid category selected");
        }

        private void GetVideoListButton_Click(object sender, EventArgs e)
        {
            //videolist
            GuiToUtil(generic);

            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                TreeNode selected = treeView1.SelectedNode;
                if (parentCat is NextPageVideoCategory)
                {
                    selected = selected.Parent;
                    selected.Nodes.RemoveAt(selected.Nodes.Count - 1);
                }
                else
                    selected.Nodes.Clear();
                List<VideoInfo> videos = generic.getVideoList(parentCat);
                foreach (VideoInfo video in videos)
                    selected.Nodes.Add(video.Title).Tag = video;
                selected.Text += ' ' + selected.Nodes.Count.ToString();

                if (generic.HasNextPage)
                {
                    NextPageVideoCategory npCat = new NextPageVideoCategory();
                    npCat.Url = (string)GetProperty(generic, "nextPageUrl");
                    selected.Nodes.Add(npCat.Name).Tag = npCat;
                }
            }
            else
                MessageBox.Show("no valid category selected");
        }
        #endregion

        #region NextPrevPage
        private void CreateNextPageRegexButton_Click(object sender, EventArgs e)
        {
            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                Form2 f2 = new Form2();
                nextPageRegExTextBox.Text = f2.Execute(nextPageRegExTextBox.Text, ((RssLink)parentCat).Url,
                    new string[] { "url" });
            }
            else
                MessageBox.Show("no valid category selected");
        }

        #endregion

        #region VideoUrl
        private void CreateVideoUrlRegexButton_Click(object sender, EventArgs e)
        {
            VideoInfo video = GetTreeViewSelectedNode() as VideoInfo;
            if (video != null)
            {
                Form2 f2 = new Form2();
                videoUrlRegExTextBox.Text = f2.Execute(videoUrlRegExTextBox.Text, video.VideoUrl, null,
                    new string[] { "m0", "m1", "m2" });
            }
            else
                MessageBox.Show("no valid video selected");
        }

        private void GetVideoUrlButton_Click(object sender, EventArgs e)
        {
            //VideoUrl
            GuiToUtil(generic);

            VideoInfo video = GetTreeViewSelectedNode() as VideoInfo;
            if (video != null)
                videoUrlResultTextBox.Text = generic.getFormattedVideoUrl(video);
            else
                MessageBox.Show("no valid video selected");
        }

        private void CreatePlayListRegexButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(videoUrlResultTextBox.Text))
                MessageBox.Show("VideoUrlResult is empty");
            else
            {
                Form2 f2 = new Form2();
                playlistUrlRegexTextBox.Text = f2.Execute(playlistUrlRegexTextBox.Text, videoUrlResultTextBox.Text,
                    new string[] { "url" });
            }
        }

        private void GetPlayListUrlButton_Click(object sender, EventArgs e)
        {
            GuiToUtil(generic);
            if (String.IsNullOrEmpty(videoUrlResultTextBox.Text))
                MessageBox.Show("VideoUrlResult is empty");
            else
                playListUrlResultTextBox.Text = generic.getPlaylistUrl(videoUrlResultTextBox.Text);
        }

        private void CreateFileUrlRegexButton_Click(object sender, EventArgs e)
        {
            GuiToUtil(generic);
            if (String.IsNullOrEmpty(playListUrlResultTextBox.Text))
                MessageBox.Show("PlaylistUrlResult is empty");
            else
            {
                string webData;
                string post = (string)GetProperty(generic, "fileUrlPostString");
                if (String.IsNullOrEmpty(post))
                    webData = SiteUtilBase.GetWebData(playListUrlResultTextBox.Text);
                else
                    webData = SiteUtilBase.GetWebDataFromPost(playListUrlResultTextBox.Text, post);

                Form2 f2 = new Form2();
                fileUrlRegexTextBox.Text = f2.Execute(fileUrlRegexTextBox.Text, webData, playListUrlResultTextBox.Text,
                    new string[] { "m0", "m1", "m2", "n0", "n1", "n2" });
            }
        }

        private void getFileUrlButton_Click(object sender, EventArgs e)
        {
            GuiToUtil(generic);
            if (String.IsNullOrEmpty(playListUrlResultTextBox.Text))
                MessageBox.Show("PlaylistUrlResult is empty");
            else
            {
                Dictionary<string, string> playList;
                if (!String.IsNullOrEmpty(GetRegex(generic, "regEx_FileUrl")))
                    playList = generic.GetPlaybackOptions(playListUrlResultTextBox.Text);
                else
                {
                    playList = new Dictionary<string, string>();
                    playList.Add("url", playListUrlResultTextBox.Text);
                }
                ResultUrlComboBox.Items.Clear();

                if (playList != null)
                    foreach (KeyValuePair<string, string> entry in playList)
                    {
                        PlaybackOption po = new PlaybackOption(entry);
                        if ((bool)GetProperty(generic, "getRedirectedFileUrl"))
                            po.Url = SiteUtilBase.GetRedirectedUrl(po.Url);
                        ResultUrlComboBox.Items.Add(po);
                    }

                if (ResultUrlComboBox.Items.Count > 0)
                    ResultUrlComboBox.SelectedIndex = 0;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Windows Media Player\\wmplayer.exe"),
                (ResultUrlComboBox.SelectedItem as PlaybackOption).Url);
        }

        private void copyUrl_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((ResultUrlComboBox.SelectedItem as PlaybackOption).Url);
        }

        private void checkValid_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"""" + (ResultUrlComboBox.SelectedItem as PlaybackOption).Url + @""" is " +
                (!Utils.IsValidUri((ResultUrlComboBox.SelectedItem as PlaybackOption).Url) ? "NOT " : String.Empty) +
                "valid");
        }

        #endregion

        private void loadSitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(SerializableSettings));
                    SerializableSettings s = (SerializableSettings)ser.Deserialize(fs);
                    fs.Close();
                    SiteSettingsList = s.Sites;
                    int i = 0;
                    while (i < SiteSettingsList.Count)
                    {
                        if (SiteSettingsList[i].UtilName != "GenericSite" || SiteSettingsList[i].Configuration == null)
                            SiteSettingsList.RemoveAt(i);
                        else
                            i++;
                    }
                    comboBoxSites.ComboBox.DisplayMember = "Name";
                    comboBoxSites.ComboBox.DataSource = SiteSettingsList;
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Load settings from selected site into TextBoxes
            SiteSettings siteSettings = comboBoxSites.SelectedItem as SiteSettings;
            if (siteSettings != null)
            {
                generic = new GenericSiteUtil();
                generic.Initialize(siteSettings);
                staticList = new List<RssLink>();
                foreach (RssLink cat in generic.Settings.Categories)
                    staticList.Add(cat);

                UtilToGui(generic);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiToUtil(generic);
            generic.Settings.IsEnabled = true;
            generic.Settings.Categories.Clear();
            foreach (Category cat in staticList)
                generic.Settings.Categories.Add(cat);
            Utils.AddConfigurationValues(generic, generic.Settings);

            XmlSerializer serializer = new XmlSerializer(typeof(SiteSettings));
            XmlDocument doc = new XmlDocument();
            XPathNavigator nav = doc.CreateNavigator();
            XmlWriter writer = nav.AppendChild();
            writer.WriteStartDocument();
            serializer.Serialize(writer, generic.Settings);
            writer.Close();

            XmlNode final = doc.CreateNode(XmlNodeType.Element, "Site", String.Empty);
            foreach (XmlNode node in doc.SelectNodes("//item"))
            {
                if (String.IsNullOrEmpty(node.InnerText))
                    node.ParentNode.RemoveChild(node);
            }

            foreach (XmlNode node in doc.SelectNodes("//SubCategories"))
            {
                if (String.IsNullOrEmpty(node.InnerText))
                    node.ParentNode.RemoveChild(node);
            }

            XmlSerializer ser = new XmlSerializer(typeof(SiteSettings));
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.OmitXmlDeclaration = true;
            using (XmlWriter ww = XmlWriter.Create(sb, xmlSettings))
            {
                doc.WriteContentTo(ww);
            }
            string res = sb.ToString();
            res = res.Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", String.Empty);
            res = res.Replace(@" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", String.Empty);// damn namespaces
            res = res.Replace("SiteSettings", "Site");
            Clipboard.SetText(res);

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://code.google.com/p/mp-onlinevideos2/wiki/SiteParser");
        }

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (categoryInfoListView.SelectedItems.Count != 1) return;
            ListViewItem.ListViewSubItemCollection item = categoryInfoListView.SelectedItems[0].SubItems;
            Clipboard.SetText(item[1].Text);
        }

        #region PlaybackOption
        private class PlaybackOption
        {
            public PlaybackOption(KeyValuePair<string, string> val)
            {
                Name = val.Key;
                Url = val.Value;
            }
            public string Name;
            public string Url;
            public override string ToString()
            {
                return (String.IsNullOrEmpty(Name) ? String.Empty : Name + " | ") + Url;
            }
        }

        #endregion

        #region debug
        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Execute(String.Empty, BaseUrlTextbox.Text,
                new string[] { "url", "title", "thumb", "description" });
        }

        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Execute(String.Empty, BaseUrlTextbox.Text,
                new string[] { "Title", "VideoUrl", "ImageUrl", "Description", "Duration", "Airdate" });
        }
        #endregion

        #region GenericProperties
        private void SetProperty(GenericSiteUtil site, string propertyName, object value)
        {
            typeof(GenericSiteUtil).InvokeMember(propertyName, BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.SetField, null, site, new[] { value });
        }

        private object GetProperty(GenericSiteUtil site, string propertyName)
        {
            return typeof(GenericSiteUtil).InvokeMember(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, site, null);
        }

        private string GetRegex(GenericSiteUtil site, string regexName)
        {
            Regex r = (Regex)GetProperty(site, regexName);
            if (r == null) return String.Empty;
            return r.ToString().TrimStart('{').TrimEnd('}');
        }

        private void SetRegex(GenericSiteUtil site, string regexName, string propertyName, string value)
        {
            Regex r;
            if (String.IsNullOrEmpty(value))
                r = null;
            else
                r = new Regex(value, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
            SetProperty(site, regexName, r);
            SetProperty(site, propertyName, value);
        }
        #endregion

        private class NextPageVideoCategory : RssLink
        {
            public NextPageVideoCategory()
            {
                Name = Translation.NextPage;
            }
        }

    }
}
