﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Windows.Forms;
using OnlineVideos.Sites;
using OnlineVideos;
using System.IO;

namespace SiteParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            generic = new MySiteUtil();
            generic.Initialize(new SiteSettings());
            generic.Settings.UtilName = "GenericSite";

            UtilToGui(generic);
        }

        System.ComponentModel.BindingList<SiteSettings> SiteSettingsList;
        MySiteUtil generic;

        private void UtilToGui(MySiteUtil util)
        {
            BaseUrlTextbox.Text = util.BaseUrl;

            CategoryRegexTextbox.Text = util.DynamicCategoriesRegEx;
            dynamicCategoryUrlFormatTextBox.Text = util.DynamicCategoryUrlFormatString;
            dynamicCategoryUrlDecodingCheckBox.Checked = util.DynamicCategoryUrlDecoding;

            SubcategorieRegexTextBox.Text = util.DynamicSubCategoriesRegEx;
            SubcategorieUrlFormatTextBox.Text = util.DynamicSubCategoryUrlFormatString;
            dynamicSubCategoryUrlDecodingCheckBox.Checked = util.DynamicSubCategoryUrlDecoding;

            videoListRegexTextBox.Text = util.VideoListRegEx;
            videoListRegexFormatTextBox.Text = util.VideoListRegExFormatString;

            videoThumbFormatStringTextBox.Text = util.VideoThumbFormatString;

            nextPageRegExTextBox.Text = util.NextPageRegEx;
            nextPageRegExUrlFormatStringTextBox.Text = util.NextPageRegExUrlFormatString;
            nextPageRegExUrlDecodingCheckBox.Checked = util.NextPageRegExUrlDecoding;

            prevPageRegExTextBox.Text = util.PrevPageRegEx;
            prevPageRegExUrlFormatStringTextBox.Text = util.PrevPageRegExUrlFormatString;
            prevPageRegExUrlDecodingCheckBox.Checked = util.PrevPageRegExUrlDecoding;

            videoUrlRegExTextBox.Text = util.VideoUrlRegEx;
            videoUrlFormatStringTextBox.Text = util.VideoUrlFormatString;
            videoListUrlDecodingCheckBox.Checked = util.VideoListUrlDecoding;
            videoUrlDecodingCheckBox.Checked = util.VideoUrlDecoding;

            playlistUrlRegexTextBox.Text = util.PlaylistUrlRegEx;
            playlistUrlFormatStringTextBox.Text = util.PlaylistUrlFormatString;

            fileUrlRegexTextBox.Text = util.FileUrlRegEx;
            fileUrlFormatStringTextBox.Text = util.FileUrlFormatString;
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
            generic.BaseUrl = ((TextBox)sender).Text;
        }
        #endregion

        #region Category
        private void CreateCategoryRegexButton_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            string webData = SiteUtilBase.GetWebData(BaseUrlTextbox.Text);
            CategoryRegexTextbox.Text = f2.Execute(CategoryRegexTextbox.Text, webData,
                new string[] { "url", "title", "thumb", "description" });
        }

        private void GetCategoriesButton_Click(object sender, EventArgs e)
        {
            //get categories
            generic.DynamicCategoriesRegEx = CategoryRegexTextbox.Text;
            generic.DynamicCategoryUrlFormatString = dynamicCategoryUrlFormatTextBox.Text;
            generic.DynamicCategoryUrlDecoding = dynamicCategoryUrlDecodingCheckBox.Checked;
            if (generic.DynamicCategoriesRegEx != null)
            {
                generic.Settings.Categories.Clear();
                generic.DiscoverDynamicCategories();
            }
            treeView1.Nodes.Clear();
            TreeNode root = treeView1.Nodes.Add("site");
            foreach (Category cat in generic.Settings.Categories)
            {
                root.Nodes.Add(cat.Name).Tag = cat;
                cat.HasSubCategories = true;
            }

        }
        #endregion

        #region SubCategories
        private void CreateSubcategoriesRegexButton_Click(object sender, EventArgs e)
        {
            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                Form2 f2 = new Form2();
                string webData = SiteUtilBase.GetWebData(((RssLink)parentCat).Url);
                SubcategorieRegexTextBox.Text = f2.Execute(SubcategorieRegexTextBox.Text, webData,
                    new string[] { "url", "title", "thumb", "description" });
            }
            else
                MessageBox.Show("no valid category selected");
        }

        private void GetSubCategoriesButton_Click(object sender, EventArgs e)
        {
            //subcategories
            generic.DynamicSubCategoriesRegEx = SubcategorieRegexTextBox.Text;
            generic.DynamicSubCategoryUrlFormatString = SubcategorieUrlFormatTextBox.Text;
            generic.DynamicSubCategoryUrlDecoding = dynamicSubCategoryUrlDecodingCheckBox.Checked;

            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null && parentCat.HasSubCategories)
            {
                TreeNode selected = treeView1.SelectedNode;
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
        #endregion

        #region VideoList
        private void CreateVideoListRegexButton_Click(object sender, EventArgs e)
        {
            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                Form2 f2 = new Form2();
                string webData = SiteUtilBase.GetWebData(((RssLink)parentCat).Url);
                videoListRegexTextBox.Text = f2.Execute(videoListRegexTextBox.Text, webData,
                    new string[] { "Title", "VideoUrl", "ImageUrl", "Description", "Duration", "Airdate" });
            }
            else
                MessageBox.Show("no valid category selected");
        }

        private void GetVideoListButton_Click(object sender, EventArgs e)
        {
            //videolist
            generic.VideoListRegEx = videoListRegexTextBox.Text;
            generic.VideoListRegExFormatString = videoListRegexFormatTextBox.Text;
            generic.VideoListUrlDecoding = videoListUrlDecodingCheckBox.Checked;
            generic.VideoThumbFormatString = videoThumbFormatStringTextBox.Text;

            generic.NextPageRegEx = nextPageRegExTextBox.Text;
            generic.NextPageRegExUrlFormatString = nextPageRegExUrlFormatStringTextBox.Text;
            generic.NextPageRegExUrlDecoding = nextPageRegExUrlDecodingCheckBox.Checked;

            generic.PrevPageRegEx = prevPageRegExTextBox.Text;
            generic.PrevPageRegExUrlFormatString = prevPageRegExUrlFormatStringTextBox.Text;
            generic.PrevPageRegExUrlDecoding = prevPageRegExUrlDecodingCheckBox.Checked;

            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                TreeNode selected = treeView1.SelectedNode;
                selected.Nodes.Clear();
                List<VideoInfo> videos = generic.getVideoList(parentCat);
                foreach (VideoInfo video in videos)
                    selected.Nodes.Add(video.Title).Tag = video;
                selected.Text += ' ' + selected.Nodes.Count.ToString();
                if (generic.HasNextPage)
                    nextPageLabel.Text = generic.NextPageUrl;
                else
                    nextPageLabel.Text = String.Empty;
                if (generic.HasPreviousPage)
                    prevPageLabel.Text = generic.PrevPageUrl;
                else
                    prevPageLabel.Text = String.Empty;
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
                string webData = SiteUtilBase.GetWebData(((RssLink)parentCat).Url);
                nextPageRegExTextBox.Text = f2.Execute(nextPageRegExTextBox.Text, webData,
                    new string[] { "url" });
            }
            else
                MessageBox.Show("no valid category selected");
        }

        private void CreatePrevPageRegexButton_Click(object sender, EventArgs e)
        {
            Category parentCat = GetTreeViewSelectedNode() as Category;
            if (parentCat != null)
            {
                Form2 f2 = new Form2();
                string webData = SiteUtilBase.GetWebData(((RssLink)parentCat).Url);
                prevPageRegExTextBox.Text = f2.Execute(prevPageRegExTextBox.Text, webData,
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
                string webData = SiteUtilBase.GetWebData(video.VideoUrl);
                videoUrlRegExTextBox.Text = f2.Execute(videoUrlRegExTextBox.Text, webData,
                    new string[] { "m0", "m1", "m2" });
            }
            else
                MessageBox.Show("no valid video selected");
        }

        private void GetVideoUrlButton_Click(object sender, EventArgs e)
        {
            //VideoUrl
            generic.VideoUrlRegEx = videoUrlRegExTextBox.Text;
            generic.VideoUrlDecoding = videoUrlDecodingCheckBox.Checked;
            generic.VideoUrlFormatString = videoUrlFormatStringTextBox.Text;
            generic.PlaylistUrlRegEx = playlistUrlRegexTextBox.Text;
            generic.PlaylistUrlFormatString = playlistUrlFormatStringTextBox.Text;
            generic.FileUrlRegEx = fileUrlRegexTextBox.Text;
            generic.FileUrlFormatString = fileUrlFormatStringTextBox.Text;
            VideoInfo video = GetTreeViewSelectedNode() as VideoInfo;
            if (video != null)
                ResultUrlTextBox.Text = generic.getUrl(video);
            else
                MessageBox.Show("no valid video selected");
        }

        private void CreatePlayListRegexButton_Click(object sender, EventArgs e)
        {
            //todo
        }

        private void CreateFileUrlRegexButton_Click(object sender, EventArgs e)
        {
            //todo
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
                        if (SiteSettingsList[i].UtilName != "GenericSite" || SiteSettingsList[i].Configuration == null) SiteSettingsList.RemoveAt(i);
                        else i++;
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
            generic.Initialize(siteSettings);
            UtilToGui(generic);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Windows Media Player\\wmplayer.exe"),
                ResultUrlTextBox.Text);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuration.AddConfigurationValues(generic, generic.Settings);

            XmlSerializer ser = new XmlSerializer(typeof(SiteSettings));
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            using (XmlWriter ww = XmlWriter.Create(sb, xmlSettings))
            {
                ser.Serialize(ww, generic.Settings);
            }

            Clipboard.SetText(sb.ToString());

        }

    }

    class MySiteUtil : GenericSiteUtil
    {
        public string BaseUrl { get { return baseUrl; } set { baseUrl = value; } }

        public string DynamicCategoriesRegEx { get { return GetRegex(regEx_dynamicCategories); } set { regEx_dynamicCategories = CreateRegex(value); dynamicCategoriesRegEx = value; } }
        public string DynamicCategoryUrlFormatString { get { return dynamicCategoryUrlFormatString; } set { dynamicCategoryUrlFormatString = value; } }
        public bool DynamicCategoryUrlDecoding { get { return dynamicCategoryUrlDecoding; } set { dynamicCategoryUrlDecoding = value; } }

        public string DynamicSubCategoriesRegEx { get { return GetRegex(regEx_dynamicSubCategories); } set { regEx_dynamicSubCategories = CreateRegex(value); dynamicSubCategoriesRegEx = value; } }
        public string DynamicSubCategoryUrlFormatString { get { return dynamicSubCategoryUrlFormatString; } set { dynamicSubCategoryUrlFormatString = value; } }
        public bool DynamicSubCategoryUrlDecoding { get { return dynamicSubCategoryUrlDecoding; } set { dynamicSubCategoryUrlDecoding = value; } }

        public string VideoListRegEx { get { return GetRegex(regEx_VideoList); } set { regEx_VideoList = CreateRegex(value); videoListRegEx = value; } }
        public string VideoListRegExFormatString { get { return videoListRegExFormatString; } set { videoListRegExFormatString = value; } }
        public bool VideoListUrlDecoding { get { return videoListUrlDecoding; } set { videoListUrlDecoding = value; } }
        public string VideoThumbFormatString { get { return videoThumbFormatString; } set { videoThumbFormatString = value; } }

        public string NextPageRegEx { get { return GetRegex(regEx_NextPage); } set { regEx_NextPage = CreateRegex(value); nextPageRegEx = value; } }
        public string NextPageRegExUrlFormatString { get { return nextPageRegExUrlFormatString; } set { nextPageRegExUrlFormatString = value; } }
        public bool NextPageRegExUrlDecoding { get { return nextPageRegExUrlDecoding; } set { nextPageRegExUrlDecoding = value; } }
        public string NextPageUrl { get { return nextPageUrl; } }

        public string PrevPageRegEx { get { return GetRegex(regEx_PrevPage); } set { regEx_PrevPage = CreateRegex(value); prevPageRegEx = value; } }
        public string PrevPageRegExUrlFormatString { get { return prevPageRegExUrlFormatString; } set { prevPageRegExUrlFormatString = value; } }
        public bool PrevPageRegExUrlDecoding { get { return prevPageRegExUrlDecoding; } set { prevPageRegExUrlDecoding = value; } }
        public string PrevPageUrl { get { return previousPageUrl; } }

        public string VideoUrlRegEx { get { return GetRegex(regEx_VideoUrl); } set { regEx_VideoUrl = CreateRegex(value); videoUrlRegEx = value; } }
        public string VideoUrlFormatString { get { return videoUrlFormatString; } set { videoUrlFormatString = value; } }
        public bool VideoUrlDecoding { get { return videoUrlDecoding; } set { videoUrlDecoding = value; } }

        public string PlaylistUrlRegEx { get { return GetRegex(regEx_PlaylistUrl); } set { regEx_PlaylistUrl = CreateRegex(value); playlistUrlRegEx = value; } }
        public string PlaylistUrlFormatString { get { return playlistUrlFormatString; } set { playlistUrlFormatString = value; } }
        public string FileUrlRegEx { get { return GetRegex(regEx_FileUrl); } set { regEx_FileUrl = CreateRegex(value); fileUrlRegEx = value; } }
        public string FileUrlFormatString { get { return fileUrlFormatString; } set { fileUrlFormatString = value; } }

        #region Regex_String
        private Regex CreateRegex(string s)
        {
            if (String.IsNullOrEmpty(s))
                return null;
            return new Regex(s, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
        }

        private string GetRegex(Regex r)
        {
            if (r == null) return String.Empty;
            return r.ToString().TrimStart('{').TrimEnd('}');
        }
        #endregion
    }

}
