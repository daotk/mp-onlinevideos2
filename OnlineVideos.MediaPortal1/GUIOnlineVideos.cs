using System;
using System.Collections.Generic;
using System.Linq;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MediaPortal.Profile;
using OnlineVideos.MediaPortal1.Player;
using Action = MediaPortal.GUI.Library.Action;

namespace OnlineVideos.MediaPortal1
{
    [PluginIcons("OnlineVideos.MediaPortal1.OnlineVideos.png", "OnlineVideos.MediaPortal1.OnlineVideosDisabled.png")]
    public partial class GUIOnlineVideos : GUIWindow, ISetupForm, IShowPlugin
    {
        public const int WindowId = 4755;

        public enum State { sites = 0, categories = 1, videos = 2, details = 3, groups = 4 }

        public enum VideosMode { Category = 0, Search = 1 }

        #region IShowPlugin Implementation

        bool IShowPlugin.ShowDefaultHome()
        {
            return true;
        }

        #endregion

        #region ISetupForm Implementation

        string ISetupForm.Author()
        {
            return "offbyone";
        }

        bool ISetupForm.CanEnable()
        {
            return true;
        }

        bool ISetupForm.DefaultEnabled()
        {
            return true;
        }

        string ISetupForm.Description()
        {
            return "Browse videos from various online sites.";
        }

        bool ISetupForm.GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
        {
            // don't use PluginConfiguration.Instance here -> GetHome is already called when MediaPortal starts up into HomeScreen
            // and we don't want to load all sites and config at that moment already
            using (Settings settings = new MPSettings())
            {
                strButtonText = settings.GetValueAsString(PluginConfiguration.CFG_SECTION, PluginConfiguration.CFG_BASICHOMESCREEN_NAME, "Online Videos");
            }
            strButtonImage = String.Empty;
            strButtonImageFocus = String.Empty;
            strPictureImage = @"hover_OnlineVideos.png";
            return true;
        }

        int ISetupForm.GetWindowId()
        {
            return GetID;
        }

        bool ISetupForm.HasSetup()
        {
            return true;
        }

        string ISetupForm.PluginName()
        {
            return PluginConfiguration.PLUGIN_NAME;
        }

        /// <summary>
        /// Show Plugin Configuration Dialog.
        /// </summary>
        void ISetupForm.ShowPlugin()
        {
            System.Windows.Forms.Form setup = new Configuration();
            setup.ShowDialog();
        }

        #endregion

        #region Skin Controls
        [SkinControlAttribute(2)]
        protected GUIButtonControl GUI_btnViewAs = null;
        [SkinControlAttribute(5)]
        protected GUISelectButtonControl GUI_btnMaxResult = null;
        [SkinControlAttribute(6)]
        protected GUISelectButtonControl GUI_btnOrderBy = null;
        [SkinControlAttribute(7)]
        protected GUISelectButtonControl GUI_btnTimeFrame = null;
        [SkinControlAttribute(8)]
        protected GUIButtonControl GUI_btnUpdate = null;
        [SkinControlAttribute(9)]
        protected GUISelectButtonControl GUI_btnSearchCategories = null;
        [SkinControlAttribute(10)]
        protected GUIButtonControl GUI_btnSearch = null;
        [SkinControlAttribute(12)]
        protected GUIButtonControl GUI_btnEnterPin = null;
        [SkinControlAttribute(50)]
        protected GUIFacadeControl GUI_facadeView = null;
        [SkinControlAttribute(51)]
        protected GUIListControl GUI_infoList = null;
        [SkinControlAttribute(47016)]
        protected GUIButtonControl GUI_btnCurrentDownloads = null;
        #endregion

        #region state variables

        #region Facade ViewModes
#if MP11
        protected GUIFacadeControl.ViewMode currentView = GUIFacadeControl.ViewMode.List;
        protected GUIFacadeControl.ViewMode? suggestedView;
#else
        protected GUIFacadeControl.Layout currentView = GUIFacadeControl.Layout.List;
        protected GUIFacadeControl.Layout? suggestedView;
#endif
        #endregion
        #region CurrentState
        State currentState = State.sites;
        public State CurrentState
        {
            get { return currentState; }
            set { currentState = value; GUIPropertyManager.SetProperty("#OnlineVideos.state", value.ToString()); }
        }
        #endregion
        #region SelectedSite
        Sites.SiteUtilBase selectedSite;
        Sites.SiteUtilBase SelectedSite
        {
            get { return selectedSite; }
            set
            {
                selectedSite = value;
                if (selectedSite == null)
                    ResetSelectedSite();
                else
                {
                    GUIPropertyManager.SetProperty("#OnlineVideos.selectedSite", selectedSite.Settings.Name);
                    GUIPropertyManager.SetProperty("#OnlineVideos.selectedSiteUtil", selectedSite.Settings.UtilName);
                }
            }
        }
        #endregion
        #region Buffering
        OnlineVideos.MediaPortal1.Player.PlayerFactory _bufferingPlayerFactory = null;
        OnlineVideos.MediaPortal1.Player.PlayerFactory BufferingPlayerFactory
        {
            get { return _bufferingPlayerFactory; }
            set
            {
                _bufferingPlayerFactory = value;
                GUIPropertyManager.SetProperty("#OnlineVideos.buffered", "0");
                GUIPropertyManager.SetProperty("#OnlineVideos.IsBuffering", (value != null).ToString());
            }
        }
        #endregion

        public delegate void TrackVideoPlaybackHandler(ITrackingInfo info, double percentPlayed);
        public event TrackVideoPlaybackHandler TrackVideoPlayback;

        OnlineVideosGuiListItem selectedSitesGroup;
        Category selectedCategory;
        VideoInfo selectedVideo;

        bool preventDialogOnLoad = false;

        int selectedClipIndex = 0;  // used to remember the position of the last selected Trailer

        VideosMode currentVideosDisplayMode = VideosMode.Category;

        List<VideoInfo> currentVideoList = new List<VideoInfo>();
        List<VideoInfo> currentTrailerList = new List<VideoInfo>();
        Player.PlayList currentPlaylist = null;
        Player.PlayListItem currentPlayingItem = null;

        HashSet<string> extendedProperties = new HashSet<string>();

        SmsT9Filter currentFilter = new SmsT9Filter();
        string videosVKfilter = string.Empty; // used for searching in large lists of videos
        LoadParameterInfo loadParamInfo;

        bool GroupsEnabled
        {
            get { return (PluginConfiguration.Instance.SitesGroups != null && PluginConfiguration.Instance.SitesGroups.Count > 0) || PluginConfiguration.Instance.autoGroupByLang; }
        }

        #endregion

        #region filter variables
        List<int> moSupportedMaxResultList;
        Dictionary<String, String> moSupportedOrderByList;
        Dictionary<String, String> moSupportedTimeFrameList;
        Dictionary<String, String> moSupportedSearchCategoryList;

        //selected values
        int miMaxResult;
        string msOrderBy = String.Empty;
        string msTimeFrame = String.Empty;

        //selected indices
        int SelectedMaxResultIndex;
        int SelectedOrderByIndex;
        int SelectedTimeFrameIndex;
        int SelectedSearchCategoryIndex;
        #endregion

        #region search variables
        string lastSearchQuery = string.Empty;
        string lastSearchCategory;
        #endregion

        #region GUIWindow Overrides

        public override string GetModuleName()
        {
            return PluginConfiguration.Instance.BasicHomeScreenName;
        }

        public override int GetID
        {
            get { return WindowId; }
            set { base.GetID = value; }
        }

        public override bool Init()
        {
			OnlineVideosAppDomain.UseSeperateDomain = true;

            bool result = Load(GUIGraphicsContext.Skin + @"\myonlinevideos.xml");

            GUIPropertyManager.SetProperty("#OnlineVideos.desc", " "); GUIPropertyManager.SetProperty("#OnlineVideos.desc", string.Empty);
            GUIPropertyManager.SetProperty("#OnlineVideos.length", " "); GUIPropertyManager.SetProperty("#OnlineVideos.length", string.Empty);
            GUIPropertyManager.SetProperty("#OnlineVideos.aired", " "); GUIPropertyManager.SetProperty("#OnlineVideos.aired", string.Empty);
            GUIPropertyManager.SetProperty("#OnlineVideos.filter", " "); GUIPropertyManager.SetProperty("#OnlineVideos.filter", string.Empty);
            GUIPropertyManager.SetProperty("#OnlineVideos.selectedSite", " "); GUIPropertyManager.SetProperty("#OnlineVideos.selectedSite", string.Empty);
            GUIPropertyManager.SetProperty("#OnlineVideos.selectedSiteUtil", " "); GUIPropertyManager.SetProperty("#OnlineVideos.selectedSiteUtil", string.Empty);
            GUIPropertyManager.SetProperty("#OnlineVideos.currentDownloads", "0");
            CurrentState = State.sites;
            // get last active module settings  
            using (MediaPortal.Profile.Settings settings = new MediaPortal.Profile.MPSettings())
            {
                bool lastActiveModuleSetting = settings.GetValueAsBool("general", "showlastactivemodule", false);
                int lastActiveModule = settings.GetValueAsInt("general", "lastactivemodule", -1);
                preventDialogOnLoad = (lastActiveModuleSetting && (lastActiveModule == GetID));
            }

			StartBackgroundInitialization();

            return result;
        }

        public override void DeInit()
        {
            PluginConfiguration.Instance.Save(true);
            base.DeInit();
        }

        protected override void OnPageLoad()
        {
			GUIPropertyManager.SetProperty("#header.label", PluginConfiguration.Instance.BasicHomeScreenName);

            base.OnPageLoad(); // let animations run

			if (initializationBackgroundWorker.IsBusy)
			{
				GUIWaitCursor.Init(); GUIWaitCursor.Show();
				initializationBackgroundWorker.RunWorkerCompleted += (o, e) =>
				{
					GUIWaitCursor.Hide();
					if (!firstLoadDone) DoFirstLoad();
					else DoSubsequentLoad();
				};
			}
			else
			{
				if (!firstLoadDone) DoFirstLoad();
				else DoSubsequentLoad();
			}
        }

        protected override void OnShowContextMenu()
        {
            if (Gui2UtilConnector.Instance.IsBusy || BufferingPlayerFactory != null) return; // wait for any background action e.g. getting next page videos to finish

            if (CurrentState == State.sites && GetFocusControlId() == GUI_facadeView.GetID)
            {
                // handle a site's context menu
                OnlineVideosGuiListItem selectedItem = GUI_facadeView.SelectedListItem as OnlineVideosGuiListItem;
                if (selectedItem == null || selectedItem.Item == null) return; // only context menu for items with an object backing them

                Sites.SiteUtilBase aSite = selectedItem.Item as Sites.SiteUtilBase;
                if (aSite != null)
                {
                    selectedSite = SiteUserSettingsDialog.ShowDialog(aSite);
                    selectedItem.Item = selectedSite;
                }
            }
            else if (CurrentState == State.categories && GetFocusControlId() == GUI_facadeView.GetID)
            {
                // handle a category's context menu
                OnlineVideosGuiListItem selectedItem = GUI_facadeView.SelectedListItem as OnlineVideosGuiListItem;
                if (selectedItem == null || selectedItem.Item == null) return; // only context menu for items with an object backing them

                Category aCategory = selectedItem.Item as Category;
                if (aCategory != null && !(aCategory is NextPageCategory))
                {
                    GUIDialogMenu dlgCat = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
                    if (dlgCat == null) return;
                    dlgCat.Reset();
					dlgCat.SetHeading(Translation.Instance.Actions);
					if (!(SelectedSite is Sites.FavoriteUtil) && !aCategory.HasSubCategories) dlgCat.Add(Translation.Instance.AddToFavourites);
                    foreach (string entry in SelectedSite.GetContextMenuEntries(aCategory, null)) dlgCat.Add(entry);
                    dlgCat.DoModal(GUIWindowManager.ActiveWindow);
                    if (dlgCat.SelectedId == -1) return;
                    else
                    {
						if (dlgCat.SelectedLabelText == Translation.Instance.AddToFavourites)
                        {
                            OnlineVideoSettings.Instance.FavDB.addFavoriteCategory(aCategory, SelectedSite.Settings.Name);
                        }
                        else
                        {
                            List<ISearchResultItem> itemsToShow = null;
                            Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                            {
                                return SelectedSite.ExecuteContextMenuEntry(aCategory, null, dlgCat.SelectedLabelText, out itemsToShow);
                            },
                            delegate(bool success, object result)
                            {
                                if (itemsToShow != null && itemsToShow.Count > 0)
                                {
                                    SetSearchResultItemsToFacade(itemsToShow, VideosMode.Category);
                                }
                                else
                                {
                                    if (success && result != null && (bool)result) DisplayCategories(selectedCategory, null);
                                }
                            },
                            ": " + dlgCat.SelectedLabelText, true);
                        }
                    }
                }
            }
            else if ((CurrentState == State.videos && GetFocusControlId() == GUI_facadeView.GetID) ||
                (CurrentState == State.details && GetFocusControlId() == GUI_infoList.GetID))
            {
                // handle a video's context menu
                int numItemsShown = (CurrentState == State.videos ? GUI_facadeView.Count : GUI_infoList.Count) - 1; // first item is always ".."
                OnlineVideosGuiListItem selectedItem = CurrentState == State.videos ?
                    GUI_facadeView.SelectedListItem as OnlineVideosGuiListItem : GUI_infoList.SelectedListItem as OnlineVideosGuiListItem;
                if (selectedItem == null || selectedItem.Item == null) return; // only context menu for items with an object backing them

                VideoInfo aVideo = selectedItem.Item as VideoInfo;

                if (aVideo != null)
                {
                    List<string> dialogOptions = new List<string>();
                    GUIDialogMenu dlgSel = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
                    if (dlgSel == null) return;
                    dlgSel.Reset();
					dlgSel.SetHeading(Translation.Instance.Actions);
                    // these context menu entries should only show if the item will not go to the details view
                    if (!(SelectedSite is IChoice && CurrentState == State.videos && aVideo.HasDetails))
                    {
                        if (!(SelectedSite is Sites.FavoriteUtil && aVideo.HasDetails &&
                            (selectedCategory is Sites.FavoriteUtil.FavoriteCategory && (selectedCategory as Sites.FavoriteUtil.FavoriteCategory).Site is IChoice)))
                        {
							dlgSel.Add(Translation.Instance.PlayWith);
                            dialogOptions.Add("PlayWith");
                            if (numItemsShown > 1)
                            {
								dlgSel.Add(Translation.Instance.PlayAll);
                                dialogOptions.Add("PlayAll");
                            }
                            if (!(SelectedSite is Sites.FavoriteUtil) && !(SelectedSite is Sites.DownloadedVideoUtil))
                            {
								dlgSel.Add(Translation.Instance.AddToFavourites);
                                dialogOptions.Add("AddToFav");
                            }
                            if (!(SelectedSite is Sites.DownloadedVideoUtil))
                            {
								dlgSel.Add(Translation.Instance.Download);
                                dialogOptions.Add("Download");

                                if (loadParamInfo != null && !string.IsNullOrEmpty(loadParamInfo.DownloadDir) && System.IO.Directory.Exists(loadParamInfo.DownloadDir))
                                {
                                    if(string.IsNullOrEmpty(loadParamInfo.DownloadMenuEntry))
										dlgSel.Add(Translation.Instance.DownloadUserdefined);
                                    else
                                        dlgSel.Add(loadParamInfo.DownloadMenuEntry);
                                    dialogOptions.Add("UserdefinedDownload");
                                }
                            }
                            List<string> siteSpecificEntries = SelectedSite.GetContextMenuEntries(selectedCategory, aVideo);
                            if (siteSpecificEntries != null) foreach (string entry in siteSpecificEntries) { dlgSel.Add(entry); dialogOptions.Add(entry); }
                        }
                    }
                    // always allow the VK filtering in videos view
                    if (CurrentState == State.videos && numItemsShown > 1)
                    {
						dlgSel.Add(Translation.Instance.Filter);
                        dialogOptions.Add("Filter");
                    }
                    if (dialogOptions.Count > 0)
                    {
                        dlgSel.DoModal(GUIWindowManager.ActiveWindow);
                        if (dlgSel.SelectedId == -1) return;
                        else
                        {
                            switch (dialogOptions[dlgSel.SelectedId - 1])
                            {
                                case "PlayWith":
                                    dialogOptions.Clear();
                                    dlgSel.Reset();
									dlgSel.SetHeading(Translation.Instance.Actions);
                                    dlgSel.Add("MediaPortal");
                                    dialogOptions.Add(OnlineVideos.PlayerType.Internal.ToString());
                                    dlgSel.Add("Windows Media Player");
                                    dialogOptions.Add(OnlineVideos.PlayerType.WMP.ToString());
                                    if (VLCPlayer.IsInstalled)
                                    {
                                        dlgSel.Add("VLC media player");
                                        dialogOptions.Add(OnlineVideos.PlayerType.VLC.ToString());
                                    }
                                    dlgSel.DoModal(GUIWindowManager.ActiveWindow);
                                    if (dlgSel.SelectedId == -1) return;
                                    else
                                    {
                                        OnlineVideos.PlayerType forcedPlayer = (OnlineVideos.PlayerType)Enum.Parse(typeof(OnlineVideos.PlayerType), dialogOptions[dlgSel.SelectedId - 1]);
                                        if (CurrentState == State.videos) selectedVideo = aVideo;
                                        else selectedClipIndex = GUI_infoList.SelectedListItemIndex;
                                        //play the video
                                        currentPlaylist = null;
                                        currentPlayingItem = null;
                                        Play_Step1(new PlayListItem(null, null)
                                                {
                                                    Type = MediaPortal.Playlists.PlayListItem.PlayListItemType.VideoStream,
                                                    Video = aVideo,
                                                    Util = selectedSite is Sites.FavoriteUtil ? OnlineVideoSettings.Instance.SiteUtilsList[selectedVideo.SiteName] : selectedSite,
                                                    ForcedPlayer = forcedPlayer
                                                }, true);
                                    }
                                    break;
                                case "PlayAll":
                                    PlayAll();
                                    break;
                                case "AddToFav":
                                    string suggestedTitle = SelectedSite.GetFileNameForDownload(aVideo, selectedCategory, null);
                                    OnlineVideoSettings.Instance.FavDB.addFavoriteVideo(aVideo, suggestedTitle, SelectedSite.Settings.Name);
                                    break;
                                case "Download":
									SaveVideo_Step1(new DownloadList() { CurrentItem = DownloadInfo.Create(aVideo, selectedCategory, selectedSite) });
                                    break;
                                case "UserdefinedDownload":
									var dlInfo = DownloadInfo.Create(aVideo, selectedCategory, selectedSite);
									dlInfo.OverrideFolder = loadParamInfo.DownloadDir;
									dlInfo.OverrideFileName = loadParamInfo.DownloadFilename;
									SaveVideo_Step1(new DownloadList() { CurrentItem = dlInfo });
                                    break;
                                case "Filter":
                                    if (GetUserInputString(ref videosVKfilter, false)) SetVideosToFacade(currentVideoList, currentVideosDisplayMode);
                                    break;
                                default:
                                    List<ISearchResultItem> itemsToShow = null;
                                    Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                                    {
                                        return SelectedSite.ExecuteContextMenuEntry(selectedCategory, aVideo, dialogOptions[dlgSel.SelectedId - 1], out itemsToShow);
                                    },
                                    delegate(bool success, object result)
                                    {
                                        if (itemsToShow != null && itemsToShow.Count > 0)
                                        {
                                            SetSearchResultItemsToFacade(itemsToShow, VideosMode.Category);
                                        }
                                        else
                                        {
                                            if (success && result != null && (bool)result) DisplayVideos_Category(selectedCategory, true);
                                        }
                                    }, ": " + dialogOptions[dlgSel.SelectedId - 1], true);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public override void OnAction(Action action)
        {
            switch (action.wID)
            {
                case Action.ActionType.ACTION_RECORD:
                    {
                        if (CurrentState == State.videos)
                        {
                            OnlineVideosGuiListItem selectedItem = GUI_facadeView.SelectedListItem as OnlineVideosGuiListItem;
                            if (selectedItem != null)
                            {
                                VideoInfo aVideo = selectedItem.Item as VideoInfo;
                                if (aVideo != null && !(SelectedSite is IChoice && aVideo.HasDetails))
									SaveVideo_Step1(new DownloadList() { CurrentItem = DownloadInfo.Create(aVideo, selectedCategory, selectedSite) });
                            }
                        }
                        else if (CurrentState == State.details)
                        {
                            OnlineVideosGuiListItem selectedItem = GUI_infoList.SelectedListItem as OnlineVideosGuiListItem;
                            if (selectedItem != null)
                            {
                                VideoInfo aVideo = selectedItem.Item as VideoInfo;
                                if (aVideo != null)
									SaveVideo_Step1(new DownloadList() { CurrentItem = DownloadInfo.Create(aVideo, selectedCategory, selectedSite) });
                            }
                        }
                        break;
                    }
                case Action.ActionType.ACTION_STOP:
                    if (BufferingPlayerFactory != null)
                    {
                        ((OnlineVideosPlayer)BufferingPlayerFactory.PreparedPlayer).StopBuffering();
                        Gui2UtilConnector.Instance.StopBackgroundTask();
                        return;
                    }
                    break;
                case Action.ActionType.ACTION_PLAY:
                case Action.ActionType.ACTION_MUSIC_PLAY:
                    if (BufferingPlayerFactory != null)
                    {
                        ((OnlineVideosPlayer)BufferingPlayerFactory.PreparedPlayer).SkipBuffering();
                        return;
                    }
                    break;
                case Action.ActionType.ACTION_PREVIOUS_MENU:
                    if (!currentFilter.IsEmpty())
                    {
                        currentFilter.Clear();
                        switch (CurrentState)
                        {
                            case State.sites: DisplaySites(); break;
                            case State.categories: DisplayCategories(selectedCategory); break;
                            case State.videos: SetVideosToFacade(currentVideoList, currentVideosDisplayMode); break;
                        }
                        return;
                    }
                    if (BufferingPlayerFactory != null)
                    {
                        ((OnlineVideosPlayer)BufferingPlayerFactory.PreparedPlayer).StopBuffering();
                        Gui2UtilConnector.Instance.StopBackgroundTask();
                        return;
                    }
                    if (Gui2UtilConnector.Instance.IsBusy) return; // wait for any background action e.g. dynamic category discovery to finish
                    if (CurrentState != State.groups)
                    {
                        ShowPreviousMenu();
                        return;
                    }
                    break;
                case Action.ActionType.ACTION_KEY_PRESSED:
#if MP11
                    if (GUI_facadeView.CurrentView.Visible && GUI_facadeView.Focus)
#else
                    if (GUI_facadeView.LayoutControl.Visible && GUI_facadeView.Focus)
#endif
                    {
                        // search items (starting from current selected) by title and select first found one
                        char pressedChar = (char)action.m_key.KeyChar;
                        if (char.IsDigit(pressedChar) || (pressedChar == '\b' && !currentFilter.IsEmpty()))
                        {
                            currentFilter.Add(pressedChar);
                            switch (CurrentState)
                            {
                                case State.sites: DisplaySites(); break;
                                case State.categories: DisplayCategories(selectedCategory); break;
                                case State.videos: SetVideosToFacade(currentVideoList, currentVideosDisplayMode); break;
                            }
                            return;
                        }
                        else
                        {
                            if (PluginConfiguration.Instance.useQuickSelect && char.IsLetterOrDigit(pressedChar))
                            {
                                string lowerChar = pressedChar.ToString().ToLower();
                                for (int i = GUI_facadeView.SelectedListItemIndex + 1; i < GUI_facadeView.Count; i++)
                                {
                                    if (GUI_facadeView[i].Label.ToLower().StartsWith(lowerChar))
                                    {
                                        GUI_facadeView.SelectedListItemIndex = i;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
			GUI_btnOrderBy.Label = Translation.Instance.SortOptions;
			GUI_btnMaxResult.Label = Translation.Instance.MaxResults;
			GUI_btnSearchCategories.Label = Translation.Instance.Category;
			GUI_btnTimeFrame.Label = Translation.Instance.Timeframe;
            base.OnAction(action);
        }

        public override bool OnMessage(GUIMessage message)
        {
            switch (message.Message)
            {
                case GUIMessage.MessageType.GUI_MSG_WINDOW_INIT:
                    {
                        bool result = base.OnMessage(message);
                        GUI_btnSearchCategories.RestoreSelection = false;
                        GUI_btnOrderBy.RestoreSelection = false;
                        GUI_btnTimeFrame.RestoreSelection = false;
                        GUI_btnMaxResult.RestoreSelection = false;
                        return result;
                    }
                case GUIMessage.MessageType.GUI_MSG_WINDOW_DEINIT:
                    if (message.Param1 != GUIOnlineVideoFullscreen.WINDOW_FULLSCREEN_ONLINEVIDEO)
                    {
                        // if the plugin was called with a loadParam, reset the states, so when entering without loadParam, the default view will be shown
                        if (loadParamInfo != null)
                        {
                            SelectedSite = null;
                            CurrentState = State.sites;
                            selectedCategory = null;
                        }
                    }
                    break;
            }
            return base.OnMessage(message);
        }

        private void GUIWindowManager_OnNewAction(Action action)
        {
            if (currentPlaylist != null && g_Player.HasVideo && g_Player.Player.GetType().Assembly == typeof(GUIOnlineVideos).Assembly)
            {
                if (action.wID == Action.ActionType.ACTION_NEXT_ITEM)
                {
                    int currentPlaylistIndex = currentPlayingItem != null ? currentPlaylist.IndexOf(currentPlayingItem) : 0;
                    // move to next
                    if (currentPlaylist.Count > currentPlaylistIndex + 1)
                    {
                        currentPlaylistIndex++;
                        Play_Step1(currentPlaylist[currentPlaylistIndex], GUIWindowManager.ActiveWindow == GUIOnlineVideoFullscreen.WINDOW_FULLSCREEN_ONLINEVIDEO);
                    }
                }
                else if (action.wID == Action.ActionType.ACTION_PREV_ITEM)
                {
                    int currentPlaylistIndex = currentPlayingItem != null ? currentPlaylist.IndexOf(currentPlayingItem) : 0;
                    // move to previous
                    if (currentPlaylistIndex - 1 >= 0)
                    {
                        currentPlaylistIndex--;
                        Play_Step1(currentPlaylist[currentPlaylistIndex], GUIWindowManager.ActiveWindow == GUIOnlineVideoFullscreen.WINDOW_FULLSCREEN_ONLINEVIDEO);
                    }
                }
            }
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            if (Gui2UtilConnector.Instance.IsBusy || BufferingPlayerFactory != null) return; // wait for any background action e.g. dynamic category discovery to finish
            if (control == GUI_facadeView && actionType == Action.ActionType.ACTION_SELECT_ITEM)
            {
                currentFilter.Clear();
                GUIPropertyManager.SetProperty("#OnlineVideos.filter", string.Empty);
                if (CurrentState == State.groups)
                {
                    selectedSitesGroup = GUI_facadeView.SelectedListItem as OnlineVideosGuiListItem;
                    if (selectedSitesGroup.Item is SitesGroup)
                        DisplaySites();
                    else
                    {
                        SelectedSite = selectedSitesGroup.Item as Sites.SiteUtilBase;
                        DisplayCategories(null, true);
                    }
                }
                else if (CurrentState == State.sites)
                {
                    if (GUI_facadeView.SelectedListItem.Label == "..")
                    {
                        ShowPreviousMenu();
                    }
                    else
                    {
                        SelectedSite = (GUI_facadeView.SelectedListItem as OnlineVideosGuiListItem).Item as Sites.SiteUtilBase;
                        DisplayCategories(null, true);
                    }
                }
                else if (CurrentState == State.categories)
                {
                    if (GUI_facadeView.SelectedListItemIndex == 0)
                    {
                        ShowPreviousMenu();
                    }
                    else
                    {
                        Category categoryToDisplay = (GUI_facadeView.SelectedListItem as OnlineVideosGuiListItem).Item as Category;
						if (categoryToDisplay is NextPageCategory)
						{
							DisplayCategories_NextPage(categoryToDisplay as NextPageCategory);
						}
						else if (categoryToDisplay.HasSubCategories)
                        {
                            DisplayCategories(categoryToDisplay, true);
                        }
                        else
                        {
                            DisplayVideos_Category(categoryToDisplay, false);
                        }
                    }
                }
                else if (CurrentState == State.videos)
                {
                    ImageDownloader.StopDownload = true;
                    if (GUI_facadeView.SelectedListItemIndex == 0)
                    {
                        ShowPreviousMenu();
                    }
					else if (GUI_facadeView.SelectedListItem.Label == Translation.Instance.NextPage)
                    {
                        DisplayVideos_NextPage();
                    }
                    else
                    {
                        selectedVideo = (GUI_facadeView.SelectedListItem as OnlineVideosGuiListItem).Item as VideoInfo;
                        if (SelectedSite is IChoice && selectedVideo.HasDetails)
                        {
                            // show details view
                            DisplayDetails();
                        }
                        else if (SelectedSite is Sites.FavoriteUtil && selectedVideo.HasDetails &&
                            (selectedCategory is Sites.FavoriteUtil.FavoriteCategory && (selectedCategory as Sites.FavoriteUtil.FavoriteCategory).Site is IChoice))
                        {
                            SelectedSite = (selectedCategory as Sites.FavoriteUtil.FavoriteCategory).Site;
                            // show details view
                            DisplayDetails();
                        }
                        else
                        {
                            //play the video
                            currentPlaylist = null;
                            currentPlayingItem = null;

                            Play_Step1(new PlayListItem(null, null)
                                    {
                                        Type = MediaPortal.Playlists.PlayListItem.PlayListItemType.VideoStream,
                                        Video = selectedVideo,
                                        Util = selectedSite is Sites.FavoriteUtil ? OnlineVideoSettings.Instance.SiteUtilsList[selectedVideo.SiteName] : selectedSite
                                    }, true);
                        }
                    }
                }
            }
            else if (control == GUI_infoList && actionType == Action.ActionType.ACTION_SELECT_ITEM && CurrentState == State.details)
            {
                ImageDownloader.StopDownload = true;
                if (GUI_infoList.SelectedListItemIndex == 0)
                {
                    ShowPreviousMenu();
                }
                else
                {
                    selectedClipIndex = GUI_infoList.SelectedListItemIndex;
                    //play the video
                    currentPlaylist = null;
                    currentPlayingItem = null;
                    Play_Step1(new PlayListItem(null, null)
                    {
                        Type = MediaPortal.Playlists.PlayListItem.PlayListItemType.VideoStream,
                        Video = (GUI_infoList.SelectedListItem as OnlineVideosGuiListItem).Item as VideoInfo,
                        Util = selectedSite is Sites.FavoriteUtil ? OnlineVideoSettings.Instance.SiteUtilsList[selectedVideo.SiteName] : selectedSite
                    }, true);
                }
            }
            else if (control == GUI_btnViewAs)
            {
                ToggleFacadeViewMode();
            }
            else if (control == GUI_btnMaxResult)
            {
                GUIControl.SelectItemControl(GetID, GUI_btnMaxResult.GetID, GUI_btnMaxResult.SelectedItem);
            }
            else if (control == GUI_btnOrderBy)
            {
                GUIControl.SelectItemControl(GetID, GUI_btnOrderBy.GetID, GUI_btnOrderBy.SelectedItem);
                if (CurrentState == State.sites) PluginConfiguration.Instance.siteOrder = (PluginConfiguration.SiteOrder)GUI_btnOrderBy.SelectedItem;
            }
            else if (control == GUI_btnTimeFrame)
            {
                GUIControl.SelectItemControl(GetID, GUI_btnTimeFrame.GetID, GUI_btnTimeFrame.SelectedItem);
            }
            else if (control == GUI_btnUpdate)
            {
                GUIControl.UnfocusControl(GetID, GUI_btnUpdate.GetID);
                switch (CurrentState)
                {
                    case State.sites: DisplaySites(); break;
                    case State.videos: DisplayVideos_Filter(); break;
                }
            }
            else if (control == GUI_btnSearchCategories)
            {
                GUIControl.SelectItemControl(GetID, GUI_btnSearchCategories.GetID, GUI_btnSearchCategories.SelectedItem);
            }
            else if (control == GUI_btnSearch)
            {
                Display_SearchResults();
            }
            else if (control == GUI_btnEnterPin)
            {
                string pin = String.Empty;
                if (GetUserInputString(ref pin, true))
                {
                    if (pin == PluginConfiguration.Instance.pinAgeConfirmation)
                    {
                        OnlineVideoSettings.Instance.AgeConfirmed = true;
                        GUIControl.UnfocusControl(GetID, GUI_btnEnterPin.GetID);
                        if (CurrentState == State.groups) DisplayGroups();
                        else DisplaySites();
                    }
                }
            }
            else if (control == GUI_btnCurrentDownloads)
            {
                // go to current downloads
                Sites.SiteUtilBase aSite = null;
                if (OnlineVideoSettings.Instance.SiteUtilsList.TryGetValue(Translation.Instance.DownloadedVideos, out aSite))
                {
                    Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                    {
                        if (!aSite.Settings.DynamicCategoriesDiscovered)
                        {
                            Log.Instance.Info("Looking for dynamic categories for '{0}'", aSite.Settings.Name);
                            int foundCategories = aSite.DiscoverDynamicCategories();
                            Log.Instance.Info("Found {0} dynamic categories for '{1}'", foundCategories, aSite.Settings.Name);
                        }
                        return aSite.Settings.Categories;
                    },
                    delegate(bool success, object result)
                    {
                        if (success)
                        {
                            Category aCategory = aSite.Settings.Categories.FirstOrDefault(c => c.Name == Translation.Instance.Downloading);
                            if (aCategory != null)
                            {
                                SelectedSite = aSite;
                                selectedCategory = aCategory;
                                DisplayVideos_Category(aCategory, true);
                            }
                        }
                    },
                    Translation.Instance.GettingDynamicCategories, true);
                }
            }
            base.OnClicked(controlId, control, actionType);
        }

        protected override void OnPageDestroy(int newWindowId)
        {
            // only handle if not just going to a full screen video
            if (newWindowId != Player.GUIOnlineVideoFullscreen.WINDOW_FULLSCREEN_ONLINEVIDEO && newWindowId != GUISiteUpdater.WindowId)
            {
                // if a pin was inserted before, reset to false and show the home page in case the user was browsing some adult site last
                if (OnlineVideoSettings.Instance.AgeConfirmed)
                {
                    OnlineVideoSettings.Instance.AgeConfirmed = false;
                    Log.Instance.Debug("Age Confirmed set to false.");
                    if (SelectedSite != null && SelectedSite.Settings.ConfirmAge)
                    {
                        CurrentState = State.sites;
                        SelectedSite = null;
                    }
                }
            }
            base.OnPageDestroy(newWindowId);
        }

        #endregion

        #region new methods

        /// <summary>
        /// This function replaces g_player.ShowFullScreenWindowVideo
        /// </summary>
        /// <returns></returns>
        private static bool ShowFullScreenWindowHandler()
        {
            if (g_Player.HasVideo && (g_Player.Player.GetType().Assembly == typeof(GUIOnlineVideos).Assembly))
            {
                if (GUIWindowManager.ActiveWindow == Player.GUIOnlineVideoFullscreen.WINDOW_FULLSCREEN_ONLINEVIDEO) return true;

                Log.Instance.Info("ShowFullScreenWindow switching to fullscreen.");
                GUIWindowManager.ActivateWindow(Player.GUIOnlineVideoFullscreen.WINDOW_FULLSCREEN_ONLINEVIDEO);
                GUIGraphicsContext.IsFullScreenVideo = true;
                return true;
            }
            return g_Player.ShowFullScreenWindowVideoDefault();
        }

        private void ShowAndEnable(int iControlId)
        {
            GUIControl.ShowControl(GetID, iControlId);
            GUIControl.EnableControl(GetID, iControlId);
        }

        private void HideAndDisable(int iControlId)
        {
            GUIControl.HideControl(GetID, iControlId);
            GUIControl.DisableControl(GetID, iControlId);
        }

        private void DisplayGroups()
        {
            var sitesGroups = PluginConfiguration.Instance.SitesGroups;
            if ((sitesGroups == null || sitesGroups.Count == 0) && PluginConfiguration.Instance.autoGroupByLang) sitesGroups = PluginConfiguration.Instance.GetAutomaticSitesGroups();

            SelectedSite = null;
            GUIControl.ClearControl(GetID, GUI_facadeView.GetID);

            if (OnlineVideoSettings.Instance.FavoritesFirst) AddFavoritesAndDownloadsSitesToFacade();

            foreach (SitesGroup sitesGroup in sitesGroups)
            {
                if (sitesGroup.Sites != null && sitesGroup.Sites.Count > 0)
                {
                    OnlineVideosGuiListItem loListItem = new OnlineVideosGuiListItem(sitesGroup);
                    loListItem.OnItemSelected += OnItemSelected;
                    loListItem.ItemId = GUI_facadeView.Count;
                    GUI_facadeView.Add(loListItem);
                    if (selectedSitesGroup != null && selectedSitesGroup.Label == sitesGroup.Name) GUI_facadeView.SelectedListItemIndex = GUI_facadeView.Count - 1;
                }
            }

            // add the item for all ungrouped sites if there are any
            HashSet<string> groupedSites = new HashSet<string>();
            foreach (SitesGroup sg in sitesGroups)
                foreach (string site in sg.Sites)
                    if (!groupedSites.Contains(site)) groupedSites.Add(site);
			SitesGroup othersGroup = new SitesGroup() { Name = Translation.Instance.Others };
            foreach (string site in OnlineVideoSettings.Instance.SiteUtilsList.Keys)
				if (!groupedSites.Contains(site) && site != Translation.Instance.Favourites && site != Translation.Instance.DownloadedVideos)
                    othersGroup.Sites.Add(site);
            if (othersGroup.Sites.Count > 0)
            {
                OnlineVideosGuiListItem listItem = new OnlineVideosGuiListItem(othersGroup);
                listItem.OnItemSelected += OnItemSelected;
                listItem.ItemId = GUI_facadeView.Count;
                GUI_facadeView.Add(listItem);
                if (selectedSitesGroup != null && selectedSitesGroup.Label == othersGroup.Name) GUI_facadeView.SelectedListItemIndex = GUI_facadeView.Count - 1;
            }

            // add Favorites and Downloads Site as last two Groups (if they are available)
            if (!OnlineVideoSettings.Instance.FavoritesFirst) AddFavoritesAndDownloadsSitesToFacade();

            CurrentState = State.groups;
            UpdateViewState();
        }

        private void AddFavoritesAndDownloadsSitesToFacade()
        {
            Sites.SiteUtilBase aSite;
			if (OnlineVideoSettings.Instance.SiteUtilsList.TryGetValue(Translation.Instance.Favourites, out aSite))
            {
                OnlineVideosGuiListItem listItem = new OnlineVideosGuiListItem(aSite);
                listItem.OnItemSelected += OnItemSelected;
                listItem.ItemId = GUI_facadeView.Count;
                GUI_facadeView.Add(listItem);
                if (selectedSitesGroup != null && selectedSitesGroup.Label == listItem.Label) GUI_facadeView.SelectedListItemIndex = GUI_facadeView.Count - 1;
            }
			if (OnlineVideoSettings.Instance.SiteUtilsList.TryGetValue(Translation.Instance.DownloadedVideos, out aSite))
            {
                OnlineVideosGuiListItem listItem = new OnlineVideosGuiListItem(aSite);
                listItem.OnItemSelected += OnItemSelected;
                listItem.ItemId = GUI_facadeView.Count;
                GUI_facadeView.Add(listItem);
                if (selectedSitesGroup != null && selectedSitesGroup.Label == listItem.Label) GUI_facadeView.SelectedListItemIndex = GUI_facadeView.Count - 1;
            }
        }

        private void DisplaySites()
        {
            lastSearchQuery = string.Empty;
            selectedCategory = null;
            ResetSelectedSite();
            GUIControl.ClearControl(GetID, GUI_facadeView.GetID);

            // set order by options
            GUI_btnOrderBy.Clear();
			GUIControl.AddItemLabelControl(GetID, GUI_btnOrderBy.GetID, Translation.Instance.Default);
			GUIControl.AddItemLabelControl(GetID, GUI_btnOrderBy.GetID, Translation.Instance.Name);
			GUIControl.AddItemLabelControl(GetID, GUI_btnOrderBy.GetID, Translation.Instance.Language);
            GUI_btnOrderBy.SelectedItem = (int)PluginConfiguration.Instance.siteOrder;

            // previous selected group was actually a site or currently selected site Fav or Downl and groups enabled -> skip this step
            if (GroupsEnabled &&
                ((selectedSitesGroup != null && selectedSitesGroup.Item is Sites.SiteUtilBase) ||
                (selectedSite is Sites.FavoriteUtil || selectedSite is Sites.DownloadedVideoUtil)))
            {
                DisplayGroups();
                return;
            }
			var siteutils = OnlineVideoSettings.Instance.SiteUtilsList;
			string[] names = selectedSitesGroup == null ? siteutils.Keys.ToArray() : (selectedSitesGroup.Item as SitesGroup).Sites.ToArray();

            // get names in right order
            switch (PluginConfiguration.Instance.siteOrder)
            {
                case PluginConfiguration.SiteOrder.Name:
                    Array.Sort(names);
                    break;
                case PluginConfiguration.SiteOrder.Language:
                    Dictionary<string, List<string>> sitenames = new Dictionary<string, List<string>>();
                    foreach (string name in names)
                    {
                        Sites.SiteUtilBase aSite;
						if (siteutils.TryGetValue(name, out aSite))
                        {
                            string key = string.IsNullOrEmpty(aSite.Settings.Language) ? "zzzzz" : aSite.Settings.Language; // puts empty lang at the end
                            List<string> listForLang = null;
                            if (!sitenames.TryGetValue(key, out listForLang)) { listForLang = new List<string>(); sitenames.Add(key, listForLang); }
                            listForLang.Add(aSite.Settings.Name);
                        }
                    }
                    string[] langs = new string[sitenames.Count];
                    sitenames.Keys.CopyTo(langs, 0);
                    Array.Sort(langs);
                    List<string> sortedByLang = new List<string>();
                    foreach (string lang in langs) sortedByLang.AddRange(sitenames[lang]);
                    names = sortedByLang.ToArray();
                    break;
            }

            if (GroupsEnabled)
            {
                // add the first item that will go to the groups menu
                OnlineVideosGuiListItem loListItem;
                loListItem = new OnlineVideosGuiListItem("..");
                loListItem.ItemId = 0;
                loListItem.IsFolder = true;
                loListItem.OnItemSelected += OnItemSelected;
                MediaPortal.Util.Utils.SetDefaultIcons(loListItem);
                GUI_facadeView.Add(loListItem);
            }

            int selectedSiteIndex = 0;  // used to remember the position of the last selected site
            currentFilter.StartMatching();
            foreach (string name in names)
            {
                Sites.SiteUtilBase aSite;
                if (currentFilter.Matches(name) &&
					siteutils.TryGetValue(name, out aSite) &&
                    aSite.Settings.IsEnabled &&
                    !(GroupsEnabled & (aSite is Sites.FavoriteUtil | aSite is Sites.DownloadedVideoUtil)) && // don't show Favorites and Downloads site if groups are enabled (because they are added as groups)
                    (!aSite.Settings.ConfirmAge || !OnlineVideoSettings.Instance.UseAgeConfirmation || OnlineVideoSettings.Instance.AgeConfirmed))
                {
                    OnlineVideosGuiListItem loListItem = new OnlineVideosGuiListItem(aSite);
                    loListItem.OnItemSelected += OnItemSelected;
                    if (loListItem.Item == SelectedSite) selectedSiteIndex = GUI_facadeView.Count;
                    loListItem.ItemId = GUI_facadeView.Count;
                    GUI_facadeView.Add(loListItem);
                }
            }
            SelectedMaxResultIndex = -1;
            SelectedOrderByIndex = -1;
            SelectedSearchCategoryIndex = -1;
            SelectedTimeFrameIndex = -1;

            if (selectedSiteIndex < GUI_facadeView.Count)
                GUI_facadeView.SelectedListItemIndex = selectedSiteIndex;
            GUIPropertyManager.SetProperty("#OnlineVideos.filter", currentFilter.ToString());
            CurrentState = State.sites;
            UpdateViewState();
        }

        private void DisplayCategories(Category parentCategory, bool? diveDownOrUpIfSingle = null)
        {
            if (parentCategory == null)
            {
                if (!SelectedSite.Settings.DynamicCategoriesDiscovered)
                {
                    Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                    {
                        Log.Instance.Info("Looking for dynamic categories for '{0}'", SelectedSite.Settings.Name);
                        int foundCategories = SelectedSite.DiscoverDynamicCategories();
                        Log.Instance.Info("Found {0} dynamic categories for '{1}'", foundCategories, SelectedSite.Settings.Name);
                        return SelectedSite.Settings.Categories;
                    },
                    delegate(bool success, object result)
                    {
                        if (success)
                        {
                            SetCategoriesToFacade(parentCategory, result as IList<Category>, diveDownOrUpIfSingle);
                        }
                    },
					Translation.Instance.GettingDynamicCategories, true);
                }
                else
                {
                    SetCategoriesToFacade(parentCategory, SelectedSite.Settings.Categories, diveDownOrUpIfSingle);
                }
            }
            else
            {
                if (!parentCategory.SubCategoriesDiscovered)
                {
                    Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                    {
                        Log.Instance.Info("Looking for subcategories in '{0}' on site '{1}'", parentCategory.Name, SelectedSite.Settings.Name);
                        int foundCategories = SelectedSite.DiscoverSubCategories(parentCategory);
                        Log.Instance.Info("Found {0} subcategories in '{1}' on site '{2}'", foundCategories, parentCategory.Name, SelectedSite.Settings.Name);
                        return parentCategory.SubCategories;
                    },
                    delegate(bool success, object result)
                    {
                        if (success)
                        {
                            SetCategoriesToFacade(parentCategory, result as IList<Category>, diveDownOrUpIfSingle);
                        }
                    },
					Translation.Instance.GettingDynamicCategories, true);
                }
                else
                {
                    SetCategoriesToFacade(parentCategory, parentCategory.SubCategories, diveDownOrUpIfSingle);
                }
            }
        }

		private void DisplayCategories_NextPage(NextPageCategory cat)
		{
			Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
			{
				return SelectedSite.DiscoverNextPageCategories(cat);
			},
			delegate(bool success, object result)
			{
				if (success) SetCategoriesToFacade(cat.ParentCategory, cat.ParentCategory == null ? SelectedSite.Settings.Categories as IList<Category> : cat.ParentCategory.SubCategories, false, true);
			},
			Translation.Instance.GettingNextPageVideos, true);
		}

        private void SetCategoriesToFacade(Category parentCategory, IList<Category> categories, bool? diveDownOrUpIfSingle, bool append = false)
        {
            if (loadParamInfo != null && loadParamInfo.Site == SelectedSite.Settings.Name && parentCategory == null && !string.IsNullOrEmpty(loadParamInfo.Category))
            {
                var foundCat = categories.FirstOrDefault(r => r.Name == loadParamInfo.Category);
                if (foundCat != null)
                {
                    if (foundCat.HasSubCategories)
                    {
                        DisplayCategories(foundCat, true);
                    }
                    else
                    {
                        DisplayVideos_Category(foundCat, false);
                    }
                }
                return;
            }

			int categoryIndexToSelect = (categories != null && categories.Count > 0) ? 1 : 0; // select the first category by default if there is one
			if (append)
			{
				currentFilter.Clear();
				categoryIndexToSelect = GUI_facadeView.Count - 1;
			}

            GUIControl.ClearControl(GetID, GUI_facadeView.GetID);

            // add the first item that will go to the previous menu
            OnlineVideosGuiListItem loListItem;
            loListItem = new OnlineVideosGuiListItem("..");
            loListItem.IsFolder = true;
            loListItem.ItemId = 0;
            loListItem.OnItemSelected += OnItemSelected;
            MediaPortal.Util.Utils.SetDefaultIcons(loListItem);
            GUI_facadeView.Add(loListItem);

            Dictionary<string, bool> imageHash = new Dictionary<string, bool>();
            suggestedView = null;
            currentFilter.StartMatching();
            if (categories != null)
            {
                foreach (Category loCat in categories)
                {
                    if (currentFilter.Matches(loCat.Name))
                    {
                        loListItem = new OnlineVideosGuiListItem(loCat);
                        loListItem.ItemId = GUI_facadeView.Count;
						if (loCat is NextPageCategory)
						{
							loListItem.IconImage = "OnlineVideos\\NextPage.png";
							loListItem.IconImageBig = "OnlineVideos\\NextPage.png";
							loListItem.ThumbnailImage = "OnlineVideos\\NextPage.png";
						}
                        if (!string.IsNullOrEmpty(loCat.Thumb)) imageHash[loCat.Thumb] = true;
                        loListItem.OnItemSelected += OnItemSelected;
                        if (loCat == selectedCategory) categoryIndexToSelect = GUI_facadeView.Count; // select the category that was previously selected
                        GUI_facadeView.Add(loListItem);
                    }
                }

                if (imageHash.Count > 0) ImageDownloader.GetImages<Category>(categories);
#if MP11
                if ((GUI_facadeView.Count > 1 && imageHash.Count == 0) || (GUI_facadeView.Count > 2 && imageHash.Count == 1)) suggestedView = GUIFacadeControl.ViewMode.List;
#else
                if ((GUI_facadeView.Count > 1 && imageHash.Count == 0) || (GUI_facadeView.Count > 2 && imageHash.Count == 1)) suggestedView = GUIFacadeControl.Layout.List;
#endif
            }

            // only set selected index when not doing an automatic dive up (MediaPortal would set the old selected index asynchroneously)
            if (!(categories.Count == 1 && diveDownOrUpIfSingle == false)) GUI_facadeView.SelectedListItemIndex = categoryIndexToSelect;

            GUIPropertyManager.SetProperty("#OnlineVideos.filter", currentFilter.ToString());
            CurrentState = State.categories;
            selectedCategory = parentCategory;
            UpdateViewState();

            // automatically browse up or down if only showing a single category and parameter was set
            if (categories.Count == 1 && diveDownOrUpIfSingle != null)
            {
                if (diveDownOrUpIfSingle.Value)
                    OnClicked(GUI_facadeView.GetID, GUI_facadeView, Action.ActionType.ACTION_SELECT_ITEM);
                else
                    ShowPreviousMenu();
            }
        }

        private void DisplayDetails()
        {
            Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
            {
                return ((IChoice)SelectedSite).getVideoChoices(selectedVideo);
            },
            delegate(bool success, object result)
            {
                if (success)
                {
                    CurrentState = State.details;

                    // make the Thumb of the VideoInfo available to the details view
                    if (string.IsNullOrEmpty(selectedVideo.ImageUrl))
                        GUIPropertyManager.SetProperty("#OnlineVideos.Details.Poster", string.Empty);
                    else
                        GUIPropertyManager.SetProperty("#OnlineVideos.Details.Poster", selectedVideo.ImageUrl);

                    SetVideosToInfoList(result as List<VideoInfo>);
                }
            },
			Translation.Instance.GettingVideoDetails, true);
        }

        private void SetVideosToInfoList(List<VideoInfo> loVideoList)
        {
            SetGuiProperties_ExtendedVideoInfo(null, false);
            currentTrailerList = loVideoList;
            GUIControl.ClearControl(GetID, GUI_facadeView.GetID);
            GUIControl.ClearControl(GetID, GUI_infoList.GetID);
            OnlineVideosGuiListItem loListItem = new OnlineVideosGuiListItem("..");
            loListItem.IsFolder = true;
            loListItem.ItemId = 0;
            loListItem.OnItemSelected += OnItemSelected;
            MediaPortal.Util.Utils.SetDefaultIcons(loListItem);
            GUI_infoList.Add(loListItem);
            Dictionary<string, bool> imageHash = new Dictionary<string, bool>();
            if (loVideoList != null)
            {
                foreach (VideoInfo loVideoInfo in loVideoList)
                {
                    loListItem = new OnlineVideosGuiListItem(loVideoInfo, true);
                    loListItem.ItemId = GUI_infoList.Count;
                    loListItem.OnItemSelected += OnItemSelected;
                    GUI_infoList.Add(loListItem);
                    if (!string.IsNullOrEmpty(loVideoInfo.ImageUrl)) imageHash[loVideoInfo.ImageUrl] = true;
                }
            }
            if (imageHash.Count > 0) ImageDownloader.GetImages<VideoInfo>(currentTrailerList);

            if (loVideoList.Count > 0)
            {
                if (selectedClipIndex == 0 || selectedClipIndex >= GUI_infoList.Count) selectedClipIndex = 1;
                GUI_infoList.SelectedListItemIndex = selectedClipIndex;
                OnItemSelected(GUI_infoList[selectedClipIndex], GUI_infoList);
            }

            UpdateViewState();
        }

        private void DisplayVideos_Category(Category category, bool displayCategoriesOnError)
        {
            Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
            {
                return SelectedSite.getVideoList(category);
            },
            delegate(bool success, object result)
            {
                Category categoryToRestoreOnError = selectedCategory;
                selectedCategory = category;
                if (!success || !SetVideosToFacade(result as List<VideoInfo>, VideosMode.Category))
                {
                    selectedCategory = categoryToRestoreOnError;
                    if (displayCategoriesOnError)// an error occured or no videos were found -> return to the category selection if param was set
                    {
                        DisplayCategories(category.ParentCategory, false);
                    }
                }
            },
			Translation.Instance.GettingCategoryVideos, true);
        }

        private void Display_SearchResults(string query = null)
        {
            bool directSearch = !string.IsNullOrEmpty(query);
            if (!directSearch) query = PluginConfiguration.Instance.searchHistoryType == PluginConfiguration.SearchHistoryType.Simple ? lastSearchQuery : string.Empty;
            List<string> searchHistoryForSite = null;

            if (!directSearch && PluginConfiguration.Instance.searchHistoryType == PluginConfiguration.SearchHistoryType.Extended && PluginConfiguration.Instance.searchHistory != null && PluginConfiguration.Instance.searchHistory.Count > 0 &&
                PluginConfiguration.Instance.searchHistory.ContainsKey(SelectedSite.Settings.Name))
            {
                searchHistoryForSite = PluginConfiguration.Instance.searchHistory[SelectedSite.Settings.Name];
                if (searchHistoryForSite != null && searchHistoryForSite.Count > 0)
                {
                    GUIDialogMenu dlgSel = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
                    if (dlgSel != null)
                    {
                        dlgSel.Reset();
						dlgSel.SetHeading(Translation.Instance.SearchHistory);
						dlgSel.Add(Translation.Instance.NewSearch);
                        int numAdded = 0;
                        for (int i = searchHistoryForSite.Count - 1; i >= 0; i--)
                        {
                            searchHistoryForSite[i] = searchHistoryForSite[i].Trim();
                            if (!string.IsNullOrEmpty(searchHistoryForSite[i]))
                            {
                                dlgSel.Add(searchHistoryForSite[i]);
                                numAdded++;
                            }
                            else
                            {
                                searchHistoryForSite.RemoveAt(i);
                            }
                            // if the user set the number of searchhistoryitems lower than what was already stored - remove older entries
                            if (numAdded >= PluginConfiguration.Instance.searchHistoryNum && i > 0)
                            {
                                searchHistoryForSite.RemoveRange(0, i);
                                break;
                            }
                        }

                        dlgSel.DoModal(GUIWindowManager.ActiveWindow);

                        if (dlgSel.SelectedId == -1) return;

                        if (dlgSel.SelectedLabel == 0) query = "";
                        else query = dlgSel.SelectedLabelText;
                    }
                }
            }

            if (!directSearch)
            {
                if (GetUserInputString(ref query, false))
                {
                    GUIControl.FocusControl(GetID, GUI_facadeView.GetID);
                    query = query.Trim();
                    if (query != String.Empty)
                    {
                        if (null == searchHistoryForSite) searchHistoryForSite = new List<string>();
                        if (searchHistoryForSite.Contains(query))
                            searchHistoryForSite.Remove(query);
                        searchHistoryForSite.Add(query);
                        if (searchHistoryForSite.Count > PluginConfiguration.Instance.searchHistoryNum)
                            searchHistoryForSite.RemoveAt(0);
                        if (PluginConfiguration.Instance.searchHistory.ContainsKey(SelectedSite.Settings.Name))
                            PluginConfiguration.Instance.searchHistory[SelectedSite.Settings.Name] = searchHistoryForSite;
                        else
                            PluginConfiguration.Instance.searchHistory.Add(SelectedSite.Settings.Name, searchHistoryForSite);

                        lastSearchQuery = query;
                    }
                }
                else
                {
                    return; // user cancelled from VK
                }
            }

            SelectedSearchCategoryIndex = GUI_btnSearchCategories.SelectedItem;
            if (query != String.Empty)
            {
                Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                {
					if (moSupportedSearchCategoryList != null && moSupportedSearchCategoryList.Count > 1 && GUI_btnSearchCategories.SelectedLabel != Translation.Instance.All
                        && !string.IsNullOrEmpty(GUI_btnSearchCategories.SelectedLabel) && moSupportedSearchCategoryList.ContainsKey(GUI_btnSearchCategories.SelectedLabel))
                    {
                        string category = moSupportedSearchCategoryList[GUI_btnSearchCategories.SelectedLabel];
                        Log.Instance.Info("Searching for {0} in category {1}", query, category);
                        lastSearchCategory = category;
                        return SelectedSite.DoSearch(query, category);
                    }
                    else
                    {
                        Log.Instance.Info("Searching for {0} in all categories ", query);
                        return SelectedSite.DoSearch(query);
                    }
                },
                delegate(bool success, object result)
                {
                    List<ISearchResultItem> resultList = (result as List<ISearchResultItem>);
                    // set videos to the facade -> if none were found and an empty facade is currently shown, go to previous menu
                    if ((!success || resultList == null || resultList.Count == 0) && GUI_facadeView.Count == 0)
                    {
                        if (loadParamInfo != null && loadParamInfo.ShowVKonFailedSearch && GetUserInputString(ref query, false)) Display_SearchResults(query);
                        else ShowPreviousMenu();
                    }
                    else
                    {
						SetSearchResultItemsToFacade(resultList, VideosMode.Search, Translation.Instance.SearchResults + " [" + lastSearchQuery + "]");
                    }
                },
				Translation.Instance.GettingSearchResults, true);
            }
        }

        private void SetSearchResultItemsToFacade(List<ISearchResultItem> resultList, VideosMode mode = VideosMode.Search, string categoryName = "")
        {
			if (resultList != null && resultList.Count > 0)
			{
				if (resultList[0] is VideoInfo)
				{
					SetVideosToFacade(resultList.ConvertAll(i => i as VideoInfo), mode);
					// if only 1 result found and the current site has a details view for this video - open it right away
					if (SelectedSite is IChoice && resultList.Count == 1 && (resultList[0] as VideoInfo).HasDetails)
					{
						// actually select this item, so fanart can be shown in this and the coming screen! (fanart handler inspects the #selecteditem proeprty of teh facade)
						GUI_facadeView.SelectedListItemIndex = 1;
						selectedVideo = (GUI_facadeView[1] as OnlineVideosGuiListItem).Item as VideoInfo;
						DisplayDetails();
					}
				}
				else
				{
					Category searchCategory = OnlineVideosAppDomain.Domain.CreateInstanceAndUnwrap(typeof(Category).Assembly.FullName, typeof(Category).FullName) as Category;
					searchCategory.Name = categoryName;
					searchCategory.HasSubCategories = true;
					searchCategory.SubCategoriesDiscovered = true;
					searchCategory.SubCategories = resultList.ConvertAll(i => { (i as Category).ParentCategory = searchCategory; return i as Category; });
					SetCategoriesToFacade(searchCategory, searchCategory.SubCategories, true);
				}
			}
			else
			{
				GUIDialogNotify dlg_error = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
				if (dlg_error != null)
				{
					dlg_error.Reset();
					dlg_error.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
					dlg_error.SetHeading(PluginConfiguration.Instance.BasicHomeScreenName);
					dlg_error.SetText(Translation.Instance.NoVideoFound);
					dlg_error.DoModal(GUIWindowManager.ActiveWindow);
				}
			}
        }

        private void DisplayVideos_Filter()
        {
            miMaxResult = -1;
            SelectedMaxResultIndex = GUI_btnMaxResult.SelectedItem;
            SelectedOrderByIndex = GUI_btnOrderBy.SelectedItem;
            SelectedTimeFrameIndex = GUI_btnTimeFrame.SelectedItem;
            try
            {
                miMaxResult = Int32.Parse(GUI_btnMaxResult.SelectedLabel);
            }
            catch (Exception) { }
            msOrderBy = String.Empty;
            try
            {
                msOrderBy = moSupportedOrderByList[GUI_btnOrderBy.SelectedLabel];
            }
            catch (Exception) { }
            msTimeFrame = String.Empty;
            try
            {
                msTimeFrame = moSupportedTimeFrameList[GUI_btnTimeFrame.SelectedLabel];
            }
            catch (Exception) { }

            Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
            {
                if (currentVideosDisplayMode == VideosMode.Search)
                {
                    Log.Instance.Info("Filtering search result");
                    //filtering a search result
                    if (String.IsNullOrEmpty(lastSearchCategory))
                    {
                        return ((IFilter)SelectedSite).filterSearchResultList(lastSearchQuery, miMaxResult, msOrderBy, msTimeFrame);
                    }
                    else
                    {
                        return ((IFilter)SelectedSite).filterSearchResultList(lastSearchQuery, lastSearchCategory, miMaxResult, msOrderBy, msTimeFrame);
                    }
                }
                else
                {
                    if (SelectedSite.HasFilterCategories) // just for setting the category
                        return SelectedSite.Search(string.Empty, moSupportedSearchCategoryList[GUI_btnSearchCategories.SelectedLabel]);
                    if (SelectedSite is IFilter)
                        return ((IFilter)SelectedSite).filterVideoList(selectedCategory, miMaxResult, msOrderBy, msTimeFrame);
                }
                return null;
            },
            delegate(bool success, object result)
            {
                if (success) SetVideosToFacade(result as List<VideoInfo>, currentVideosDisplayMode);
            }
			, Translation.Instance.GettingFilteredVideos, true);
        }

        private void DisplayVideos_NextPage()
        {
            Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
            {
                return SelectedSite.getNextPageVideos();
            },
            delegate(bool success, object result)
            {
                if (success) SetVideosToFacade(result as List<VideoInfo>, currentVideosDisplayMode, true);
            },
			Translation.Instance.GettingNextPageVideos, true);
        }

        private bool SetVideosToFacade(List<VideoInfo> videos, VideosMode mode, bool append = false)
        {
            // Check for received data
            if (videos == null || videos.Count == 0)
            {
                GUIDialogNotify dlg_error = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (dlg_error != null)
                {
                    dlg_error.Reset();
                    dlg_error.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
                    dlg_error.SetHeading(PluginConfiguration.Instance.BasicHomeScreenName);
					dlg_error.SetText(Translation.Instance.NoVideoFound);
                    dlg_error.DoModal(GUIWindowManager.ActiveWindow);
                }
                return false;
            }

            int indextoSelect = -1;
            if (append)
            {
                currentFilter.Clear();
                indextoSelect = currentVideoList.Count + 1;
                currentVideoList.AddRange(videos);
            }
            else
            {
                currentVideoList = videos;
            }

            GUIControl.ClearControl(GetID, GUI_facadeView.GetID);
            // add the first item that will go to the previous menu
            OnlineVideosGuiListItem backItem = new OnlineVideosGuiListItem("..");
            backItem.ItemId = 0;
            backItem.IsFolder = true;
            backItem.OnItemSelected += OnItemSelected;
            MediaPortal.Util.Utils.SetDefaultIcons(backItem);
            GUI_facadeView.Add(backItem);

            // add the items
            Dictionary<string, bool> imageHash = new Dictionary<string, bool>();
            currentFilter.StartMatching();

            foreach (VideoInfo videoInfo in currentVideoList)
            {
                videoInfo.CleanDescriptionAndTitle();
                if (!currentFilter.Matches(videoInfo.Title) || FilterOut(videoInfo.Title) || FilterOut(videoInfo.Description)) continue;
                if (!string.IsNullOrEmpty(videosVKfilter) && !videoInfo.Title.ToLower().Contains(videosVKfilter.ToLower())) continue;

                OnlineVideosGuiListItem listItem = new OnlineVideosGuiListItem(videoInfo);
                listItem.ItemId = GUI_facadeView.Count;
                listItem.OnItemSelected += OnItemSelected;
                GUI_facadeView.Add(listItem);

                if (listItem.Item == selectedVideo) GUI_facadeView.SelectedListItemIndex = GUI_facadeView.Count - 1;
                if (!string.IsNullOrEmpty(videoInfo.ImageUrl)) imageHash[videoInfo.ImageUrl] = true;
            }
            // fall back to list view if there are no items with thumbs or more than one item and all have the same thumb
            suggestedView = null;
#if MP11
            if ((GUI_facadeView.Count > 1 && imageHash.Count == 0) || (GUI_facadeView.Count > 2 && imageHash.Count == 1)) suggestedView = GUIFacadeControl.ViewMode.List;
#else
            if ((GUI_facadeView.Count > 1 && imageHash.Count == 0) || (GUI_facadeView.Count > 2 && imageHash.Count == 1)) suggestedView = GUIFacadeControl.Layout.List;
#endif

            if (SelectedSite.HasNextPage)
            {
				OnlineVideosGuiListItem nextPageItem = new OnlineVideosGuiListItem(Translation.Instance.NextPage);
                nextPageItem.ItemId = GUI_facadeView.Count;
                nextPageItem.IsFolder = true;
                nextPageItem.IconImage = "OnlineVideos\\NextPage.png";
                nextPageItem.IconImageBig = "OnlineVideos\\NextPage.png";
                nextPageItem.ThumbnailImage = "OnlineVideos\\NextPage.png";
                nextPageItem.OnItemSelected += OnItemSelected;
                GUI_facadeView.Add(nextPageItem);
            }

            if (indextoSelect > -1 && indextoSelect < GUI_facadeView.Count) GUI_facadeView.SelectedListItemIndex = indextoSelect;

            if (imageHash.Count > 0) ImageDownloader.GetImages<VideoInfo>(currentVideoList);

            string filterstring = currentFilter.ToString();
            if (!string.IsNullOrEmpty(filterstring) && !string.IsNullOrEmpty(videosVKfilter)) filterstring += " & ";
            filterstring += videosVKfilter;
            GUIPropertyManager.SetProperty("#OnlineVideos.filter", filterstring);

            currentVideosDisplayMode = mode;
            CurrentState = State.videos;
            UpdateViewState();
            return true;
        }

        private void ShowPreviousMenu()
        {
            ImageDownloader.StopDownload = true;

            if (CurrentState == State.sites)
            {
                if (GroupsEnabled)
                    DisplayGroups();
                else
                    OnPreviousWindow();
            }
            else if (CurrentState == State.categories)
            {
                if (selectedCategory == null)
                {
                    // if plugin was called with loadParameter set to the current site and return locked -> go to previous window 
                    if (loadParamInfo != null && loadParamInfo.Return == LoadParameterInfo.ReturnMode.Locked && loadParamInfo.Site == selectedSite.Settings.Name)
                        OnPreviousWindow();
                    else
                        DisplaySites();
                }
                else
                {
                    // if plugin was called with loadParameter set to the current site and return locked and currently displaying subcategories of category from loadParam -> go to previous window 
                    if (loadParamInfo != null && loadParamInfo.Return == LoadParameterInfo.ReturnMode.Locked && loadParamInfo.Site == selectedSite.Settings.Name && loadParamInfo.Category == selectedCategory.Name)
                        OnPreviousWindow();
                    else
                        DisplayCategories(selectedCategory.ParentCategory, false);
                }
            }
            else if (CurrentState == State.videos)
            {
                videosVKfilter = string.Empty;
                // if plugin was called with loadParameter set to the current site with searchstring and return locked and currently displaying the searchresults or videos for the category from loadParam -> go to previous window 
                if (loadParamInfo != null && loadParamInfo.Return == LoadParameterInfo.ReturnMode.Locked && loadParamInfo.Site == selectedSite.Settings.Name &&
                    (currentVideosDisplayMode == VideosMode.Search ||
                    (currentVideosDisplayMode == VideosMode.Category && selectedCategory != null && loadParamInfo.Category == selectedCategory.Name))
                   )
                    OnPreviousWindow();
                else
                {
                    if (selectedCategory == null || selectedCategory.ParentCategory == null) DisplayCategories(null, false);
                    else DisplayCategories(selectedCategory.ParentCategory, false);
                }
            }
            else if (CurrentState == State.details)
            {
                if (selectedCategory is Sites.FavoriteUtil.FavoriteCategory && !(SelectedSite is Sites.FavoriteUtil))
                {
                    SelectedSite = (selectedCategory as Sites.FavoriteUtil.FavoriteCategory).FavSite;
                }
                GUIControl.UnfocusControl(GetID, GUI_infoList.GetID);
                GUI_infoList.Focus = false;
                selectedClipIndex = 0;
                SetVideosToFacade(currentVideoList, currentVideosDisplayMode);
            }
        }

        void OnItemSelected(GUIListItem item, GUIControl parent)
        {
            OnlineVideosGuiListItem ovItem = item as OnlineVideosGuiListItem;
            if (parent == GUI_infoList)
            {
                SetGuiProperties_ExtendedVideoInfo(ovItem != null ? ovItem.Item as VideoInfo : null, true);
            }
            else
            {
                SetGuiProperties_ExtendedVideoInfo(ovItem != null ? ovItem.Item as VideoInfo : null, false);
                GUIPropertyManager.SetProperty("#OnlineVideos.desc", ovItem != null ? ovItem.Description : string.Empty);
                GUIPropertyManager.SetProperty("#OnlineVideos.length", ovItem != null && ovItem.Item is VideoInfo ? VideoInfo.GetDuration((ovItem.Item as VideoInfo).Length) : string.Empty);
                GUIPropertyManager.SetProperty("#OnlineVideos.aired", ovItem != null && ovItem.Item is VideoInfo ? (ovItem.Item as VideoInfo).Airdate : string.Empty);
            }
        }

        private void removeInvalidEntries(List<string> loUrlList)
        {
            // remove all invalid entries from the list of playback urls
            if (loUrlList != null)
            {
                int i = 0;
                while (i < loUrlList.Count)
                {
                    if (String.IsNullOrEmpty(loUrlList[i]) || !Utils.IsValidUri(loUrlList[i]))
                    {
                        Log.Instance.Debug("removed invalid url: '{0}'", loUrlList[i]);
                        loUrlList.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        internal static bool GetUserInputString(ref string sString, bool password)
        {
            VirtualKeyboard keyBoard = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
            if (keyBoard == null) return false;
            keyBoard.Reset();
#if MP11
            keyBoard.IsSearchKeyboard = true;
#endif
            keyBoard.Text = sString;
            keyBoard.Password = password;
            keyBoard.DoModal(GUIWindowManager.ActiveWindow); // show it...
            if (keyBoard.IsConfirmed) sString = keyBoard.Text;
            return keyBoard.IsConfirmed;
        }

        void g_Player_PlayBackEnded(g_Player.MediaType type, string filename)
        {
            if (currentPlaylist != null)
            {
                if (g_Player.Player.GetType().Assembly == typeof(GUIOnlineVideos).Assembly)
                {
                    PlayNextPlaylistItem();
                }
                else
                {
                    // some other playback ended, and a playlist is still set here -> clear it
                    currentPlaylist = null;
                    currentPlayingItem = null;
                }
            }
            else
            {
                TrackPlayback();
                currentPlayingItem = null;
            }
        }

        void PlayNextPlaylistItem()
        {
            int currentPlaylistIndex = currentPlayingItem != null ? currentPlaylist.IndexOf(currentPlayingItem) : 0;
            if (currentPlaylist.Count > currentPlaylistIndex + 1)
            {
                // if playing a playlist item, move to the next            
                currentPlaylistIndex++;
                Play_Step1(currentPlaylist[currentPlaylistIndex], GUIWindowManager.ActiveWindow == GUIOnlineVideoFullscreen.WINDOW_FULLSCREEN_ONLINEVIDEO);
            }
            else
            {
                // if last item -> clear the list
                TrackPlayback();
                currentPlaylist = null;
                currentPlayingItem = null;
            }
        }

        void g_Player_PlayBackStopped(g_Player.MediaType type, int stoptime, string filename)
        {
            if (stoptime > 0 && g_Player.Duration > 0 && (stoptime / g_Player.Duration) > 0.8) TrackPlayback();
            currentPlayingItem = null;
        }

        void TrackPlayback()
        {
            double percent = g_Player.Duration > 0 ? g_Player.CurrentPosition / g_Player.Duration : 0;
            if (TrackVideoPlayback != null && currentPlayingItem != null && currentPlayingItem.Util != null && currentPlayingItem.Video != null)
            {
                new System.Threading.Thread((item) =>
                {
                    var myItem = item as PlayListItem;
                    ITrackingInfo info = myItem.Util.GetTrackingInfo(myItem.Video);
                    if (info.VideoKind == VideoKind.TvSeries || info.VideoKind == VideoKind.Movie) TrackVideoPlayback(info, percent);
                }) { IsBackground = true, Name = "OnlineVideosTracking" }.Start(currentPlayingItem);
            }
        }

        private void Play_Step1(PlayListItem playItem, bool goFullScreen)
        {
            if (!string.IsNullOrEmpty(playItem.FileName))
            {
                Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                {
                    return SelectedSite.getPlaylistItemUrl(playItem.Video, currentPlaylist[0].ChosenPlaybackOption, currentPlaylist.IsPlayAll);
                },
                delegate(bool success, object result)
                {
                    if (success) Play_Step2(playItem, new List<String>() { result as string }, goFullScreen);
                    else if (currentPlaylist != null && currentPlaylist.Count > 1) PlayNextPlaylistItem();
                }
				, Translation.Instance.GettingPlaybackUrlsForVideo, true);
            }
            else
            {
                Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                {
                    return SelectedSite.getMultipleVideoUrls(playItem.Video, currentPlaylist != null && currentPlaylist.Count > 1);
                },
                delegate(bool success, object result)
                {
                    if (success) Play_Step2(playItem, result as List<String>, goFullScreen);
                    else if (currentPlaylist != null && currentPlaylist.Count > 1) PlayNextPlaylistItem();
                }
				, Translation.Instance.GettingPlaybackUrlsForVideo, true);
            }
        }

        private void Play_Step2(PlayListItem playItem, List<String> loUrlList, bool goFullScreen)
        {
            removeInvalidEntries(loUrlList);

            // if no valid urls were returned show error msg
            if (loUrlList == null || loUrlList.Count == 0)
            {
                GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (dlg != null)
                {
                    dlg.Reset();
                    dlg.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
					dlg.SetHeading(Translation.Instance.Error);
					dlg.SetText(Translation.Instance.UnableToPlayVideo);
                    dlg.DoModal(GUIWindowManager.ActiveWindow);
                }
                return;
            }
            // create playlist entries if more than one url
            if (loUrlList.Count > 1)
            {
                Player.PlayList playbackItems = new Player.PlayList();
                foreach (string url in loUrlList)
                {
                    VideoInfo vi = playItem.Video.CloneForPlayList(url, url == loUrlList[0]);
                    string url_new = url;
                    if (url == loUrlList[0])
                    {
                        url_new = SelectedSite.getPlaylistItemUrl(vi, string.Empty, currentPlaylist != null && currentPlaylist.IsPlayAll);
                    }
                    PlayListItem pli = new PlayListItem(string.Format("{0} - {1} / {2}", playItem.Video.Title, (playbackItems.Count + 1).ToString(), loUrlList.Count), url_new);
                    pli.Type = MediaPortal.Playlists.PlayListItem.PlayListItemType.VideoStream;
                    pli.Video = vi;
                    pli.Util = playItem.Util;
                    pli.ForcedPlayer = playItem.ForcedPlayer;
                    playbackItems.Add(pli);
                }
                if (currentPlaylist == null)
                {
                    currentPlaylist = playbackItems;
                }
                else
                {
                    int currentPlaylistIndex = currentPlayingItem != null ? currentPlaylist.IndexOf(currentPlayingItem) : 0;
                    currentPlaylist.InsertRange(currentPlaylistIndex, playbackItems);
                }
                // make the first item the current to be played now
                playItem = playbackItems[0];
                loUrlList = new List<string>(new string[] { playItem.FileName });
            }
            // if multiple quality choices are available show a selection dialogue (also on playlist playback)
            string lsUrl = loUrlList[0];
            bool resolve = DisplayPlaybackOptions(playItem.Video, ref lsUrl); // resolve only when any playbackoptions were set
            if (lsUrl == "-1") return; // the user did not chose an option but canceled the dialog
            if (resolve)
            {
                playItem.ChosenPlaybackOption = lsUrl;
                if (playItem.Video.GetType().FullName == typeof(VideoInfo).FullName)
                {
                    Play_Step3(playItem, playItem.Video.GetPlaybackOptionUrl(lsUrl), goFullScreen);
                }
                else
                {
                    // display wait cursor as GetPlaybackOptionUrl might do webrequests when overridden
                    Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                    {
                        return playItem.Video.GetPlaybackOptionUrl(lsUrl);
                    },
                    delegate(bool success, object result)
                    {
                        if (success) Play_Step3(playItem, result as string, goFullScreen);
                    }
					, Translation.Instance.GettingPlaybackUrlsForVideo, true);
                }
            }
            else
            {
                Play_Step3(playItem, lsUrl, goFullScreen);
            }
        }

        void Play_Step3(PlayListItem playItem, string lsUrl, bool goFullScreen)
        {
            // check for valid url and cut off additional parameter
            if (String.IsNullOrEmpty(lsUrl) ||
                !Utils.IsValidUri((lsUrl.IndexOf("&&&&") > 0) ? lsUrl.Substring(0, lsUrl.IndexOf("&&&&")) : lsUrl))
            {
                GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (dlg != null)
                {
                    dlg.Reset();
                    dlg.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
					dlg.SetHeading(Translation.Instance.Error);
					dlg.SetText(Translation.Instance.UnableToPlayVideo);
                    dlg.DoModal(GUIWindowManager.ActiveWindow);
                }
                return;
            }

            // stop player if currently playing some other video
            if (g_Player.Playing) g_Player.Stop();

            currentPlayingItem = null;

            // translate rtmp urls to the local proxy
            if (new Uri(lsUrl).Scheme.ToLower().StartsWith("rtmp"))
            {
                lsUrl = ReverseProxy.Instance.GetProxyUri(RTMP_LIB.RTMPRequestHandler.Instance,
                                string.Format("http://127.0.0.1/stream.flv?rtmpurl={0}", System.Web.HttpUtility.UrlEncode(lsUrl)));
            }

            OnlineVideos.MediaPortal1.Player.PlayerFactory factory = new OnlineVideos.MediaPortal1.Player.PlayerFactory(playItem.ForcedPlayer != null ? playItem.ForcedPlayer.Value : playItem.Util.Settings.Player, lsUrl);
            if (factory.PreparedPlayerType != PlayerType.Internal)
            {
                // external players can only be created on the main thread
                Play_Step4(playItem, lsUrl, goFullScreen, factory, true);
            }
            else
            {
                Log.Instance.Info("Preparing graph for playback of {0}", lsUrl);
                bool? prepareResult = ((OnlineVideosPlayer)factory.PreparedPlayer).PrepareGraph();
                switch (prepareResult)
                {
                    case true:// buffer in background
                        Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                        {
                            try
                            {
                                Log.Instance.Info("Start prebuffering ...");
                                BufferingPlayerFactory = factory;
                                if (((OnlineVideosPlayer)factory.PreparedPlayer).BufferFile())
                                {
                                    Log.Instance.Info("Prebuffering finished.");
                                    return true;
                                }
                                else
                                {
                                    Log.Instance.Info("Prebuffering failed.");
                                    return null;
                                }
                            }
                            finally
                            {
                                BufferingPlayerFactory = null;
                            }
                        },
                        delegate(bool success, object result)
                        {
                            Play_Step4(playItem, lsUrl, goFullScreen, factory, result as bool?);
                        },
						Translation.Instance.StartingPlayback, false);
                        break;
                    case false:// play without buffering in background
                        Play_Step4(playItem, lsUrl, goFullScreen, factory, prepareResult);
                        break;
                    default: // error building graph
                        GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                        if (dlg != null)
                        {
                            dlg.Reset();
                            dlg.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
							dlg.SetHeading(Translation.Instance.Error);
							dlg.SetText(Translation.Instance.UnableToPlayVideo);
                            dlg.DoModal(GUIWindowManager.ActiveWindow);
                        }
                        break;
                }
            }
        }

        void Play_Step4(PlayListItem playItem, string lsUrl, bool goFullScreen, OnlineVideos.MediaPortal1.Player.PlayerFactory factory, bool? factoryPrepareResult)
        {
            if (factoryPrepareResult == null)
            {
                bool showMessage = true;
                if (factory.PreparedPlayer is OnlineVideosPlayer && (factory.PreparedPlayer as OnlineVideosPlayer).BufferingStopped == true) showMessage = false;
                factory.PreparedPlayer.Dispose();
                if (showMessage)
                {
                    GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                    if (dlg != null)
                    {
                        dlg.Reset();
                        dlg.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
						dlg.SetHeading(Translation.Instance.Error);
						dlg.SetText(Translation.Instance.UnableToPlayVideo);
                        dlg.DoModal(GUIWindowManager.ActiveWindow);
                    }
                }
            }
            else
            {
                (factory.PreparedPlayer as OVSPLayer).GoFullscreen = goFullScreen;

                if (!string.IsNullOrEmpty(playItem.Video.SubtitleUrl) && Utils.IsValidUri(playItem.Video.SubtitleUrl))
                {
                    // download subtitle file before starting playback
                    Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                    {
                        string subs = Sites.SiteUtilBase.GetWebData(playItem.Video.SubtitleUrl);
                        if (!string.IsNullOrEmpty(subs))
                        {
                            string subFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "OnlineVideoSubtitles.txt");
                            System.IO.File.WriteAllText(subFile, subs, System.Text.Encoding.UTF8);
                            (factory.PreparedPlayer as OVSPLayer).SubtitleFile = subFile;
                        }
                        return true;
                    },
                    delegate(bool success, object result)
                    {
                        Play_Step5(playItem, lsUrl, factory);
                    },
					Translation.Instance.DownloadingSubtitle, true);
                }
                else
                {
                    Play_Step5(playItem, lsUrl, factory);
                }
            }
        }

        private void Play_Step5(PlayListItem playItem, string lsUrl, OnlineVideos.MediaPortal1.Player.PlayerFactory factory)
        {
            IPlayerFactory savedFactory = g_Player.Factory;
            g_Player.Factory = factory;
            bool playing = g_Player.Play(lsUrl, g_Player.MediaType.Video);
            g_Player.Factory = savedFactory;

            if (g_Player.Player != null && g_Player.HasVideo)
            {
                if (!string.IsNullOrEmpty(playItem.Video.StartTime))
                {
                    Log.Instance.Info("Found starttime: {0}", playItem.Video.StartTime);
                    double seconds = playItem.Video.GetSecondsFromStartTime();
                    if (seconds > 0.0d)
                    {
                        Log.Instance.Info("SeekingAbsolute: {0}", seconds);
                        g_Player.SeekAbsolute(seconds);
                    }
                }
                currentPlayingItem = playItem;
                SetGuiProperties_PlayingVideo(playItem);
            }
        }

        private void PlayAll()
        {
            currentPlaylist = new Player.PlayList() { IsPlayAll = true };
            currentPlayingItem = null;
            List<VideoInfo> loVideoList = (SelectedSite is IChoice && currentState == State.details) ? currentTrailerList : currentVideoList;
            foreach (VideoInfo video in loVideoList)
            {
                // when not in details view of a site with details view only include videos that don't have details
                if (currentState != State.details && SelectedSite is IChoice && video.HasDetails) continue;
                currentPlaylist.Add(new Player.PlayListItem(video.Title, null)
                {
                    Type = MediaPortal.Playlists.PlayListItem.PlayListItemType.VideoStream,
                    Video = video,
                    Util = selectedSite is Sites.FavoriteUtil ? OnlineVideoSettings.Instance.SiteUtilsList[video.SiteName] : SelectedSite
                });
            }
            if (currentPlaylist.Count > 0) Play_Step1(currentPlaylist[0], true);
        }

        private void SaveVideo_Step1(DownloadList saveItems)
        {
            if (string.IsNullOrEmpty(OnlineVideoSettings.Instance.DownloadDir))
            {
                GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (dlg != null)
                {
                    dlg.Reset();
                    dlg.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
					dlg.SetHeading(Translation.Instance.Error);
					dlg.SetText(Translation.Instance.SetDownloadFolderInConfig);
                    dlg.DoModal(GUIWindowManager.ActiveWindow);
                }
                return;
            }

            if (!string.IsNullOrEmpty(saveItems.CurrentItem.Url))
            {
                Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                {
                    return saveItems.CurrentItem.Util.getPlaylistItemUrl(saveItems.CurrentItem.VideoInfo, saveItems.ChosenPlaybackOption);
                },
                delegate(bool success, object result)
                {
                    if (success) SaveVideo_Step2(saveItems, new List<string>() { result as string });
                },
				Translation.Instance.GettingPlaybackUrlsForVideo, true);
            }
            else
            {
                Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                {
                    return saveItems.CurrentItem.Util.getMultipleVideoUrls(saveItems.CurrentItem.VideoInfo);
                },
                delegate(bool success, object result)
                {
                    if (success) SaveVideo_Step2(saveItems, result as List<String>);
                },
				Translation.Instance.GettingPlaybackUrlsForVideo, true);
            }
        }

        private void SaveVideo_Step2(DownloadList saveItems, List<String> loUrlList)
        {
            removeInvalidEntries(loUrlList);

            // if no valid urls were returned show error msg
            if (loUrlList == null || loUrlList.Count == 0)
            {
                GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (dlg != null)
                {
                    dlg.Reset();
                    dlg.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
					dlg.SetHeading(Translation.Instance.Error);
					dlg.SetText(Translation.Instance.UnableToDownloadVideo);
                    dlg.DoModal(GUIWindowManager.ActiveWindow);
                }
                return;
            }
            // create download list if more than one url
            if (loUrlList.Count > 1)
            {
                saveItems.DownloadItems = new List<DownloadInfo>();
                foreach (string url in loUrlList)
                {
                    VideoInfo vi = saveItems.CurrentItem.VideoInfo.CloneForPlayList(url, url == loUrlList[0]);
                    string url_new = url;
                    if (url == loUrlList[0])
                    {
                        url_new = saveItems.CurrentItem.Util.getPlaylistItemUrl(vi, string.Empty);
                    }
                    DownloadInfo pli = DownloadInfo.Create(vi, saveItems.CurrentItem.Category, saveItems.CurrentItem.Util);
                    pli.Title = string.Format("{0} - {1} / {2}", vi.Title, (saveItems.DownloadItems.Count + 1).ToString(), loUrlList.Count);
                    pli.Url = url_new;
					pli.OverrideFolder = saveItems.CurrentItem.OverrideFolder;
					pli.OverrideFileName = saveItems.CurrentItem.OverrideFileName;
                    saveItems.DownloadItems.Add(pli);
                }
                // make the first item the current to be played now
                saveItems.CurrentItem = saveItems.DownloadItems[0];
                loUrlList = new List<string>(new string[] { saveItems.CurrentItem.Url });
            }
            // if multiple quality choices are available show a selection dialogue
            string lsUrl = loUrlList[0];
            bool resolve = DisplayPlaybackOptions(saveItems.CurrentItem.VideoInfo, ref lsUrl);
            if (lsUrl == "-1") return; // user canceled the dialog -> don't download
            if (resolve)
            {
                saveItems.ChosenPlaybackOption = lsUrl;
                if (saveItems.CurrentItem.VideoInfo.GetType().FullName == typeof(VideoInfo).FullName)
                {
                    SaveVideo_Step3(saveItems, saveItems.CurrentItem.VideoInfo.GetPlaybackOptionUrl(lsUrl));
                }
                else
                {
                    // display wait cursor as GetPlaybackOptionUrl might do webrequests when overridden
                    Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
                    {
                        return saveItems.CurrentItem.VideoInfo.GetPlaybackOptionUrl(lsUrl);
                    },
                    delegate(bool success, object result)
                    {
                        if (success) SaveVideo_Step3(saveItems, result as string);
                    }
					, Translation.Instance.GettingPlaybackUrlsForVideo, true);
                }
            }
            else
            {
                SaveVideo_Step3(saveItems, lsUrl);
            }
        }
        private void SaveVideo_Step3(DownloadList saveItems, string url)
        {
            // check for valid url and cut off additional parameter
            if (String.IsNullOrEmpty(url) ||
                !Utils.IsValidUri((url.IndexOf("&&&&") > 0) ? url.Substring(0, url.IndexOf("&&&&")) : url))
            {
                GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (dlg != null)
                {
                    dlg.Reset();
                    dlg.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
					dlg.SetHeading(Translation.Instance.Error);
					dlg.SetText(Translation.Instance.UnableToDownloadVideo);
                    dlg.DoModal(GUIWindowManager.ActiveWindow);
                }
                return;
            }

            // translate rtmp urls to the local proxy
            if (new Uri(url).Scheme.ToLower().StartsWith("rtmp"))
            {
                url = ReverseProxy.Instance.GetProxyUri(RTMP_LIB.RTMPRequestHandler.Instance,
                                string.Format("http://127.0.0.1/stream.flv?rtmpurl={0}", System.Web.HttpUtility.UrlEncode(url)));
            }

            saveItems.CurrentItem.Url = url;
            if (string.IsNullOrEmpty(saveItems.CurrentItem.Title)) saveItems.CurrentItem.Title = saveItems.CurrentItem.VideoInfo.Title;

			if (!string.IsNullOrEmpty(saveItems.CurrentItem.OverrideFolder))
			{
				if (!string.IsNullOrEmpty(saveItems.CurrentItem.OverrideFileName))
					saveItems.CurrentItem.LocalFile = System.IO.Path.Combine(saveItems.CurrentItem.OverrideFolder, saveItems.CurrentItem.OverrideFileName);
				else
					saveItems.CurrentItem.LocalFile = System.IO.Path.Combine(saveItems.CurrentItem.OverrideFolder, saveItems.CurrentItem.Util.GetFileNameForDownload(saveItems.CurrentItem.VideoInfo, saveItems.CurrentItem.Category, url));
			}
			else
			{
				saveItems.CurrentItem.LocalFile = System.IO.Path.Combine(System.IO.Path.Combine(OnlineVideoSettings.Instance.DownloadDir, saveItems.CurrentItem.Util.Settings.Name), saveItems.CurrentItem.Util.GetFileNameForDownload(saveItems.CurrentItem.VideoInfo, saveItems.CurrentItem.Category, url));
			}

            if (saveItems.DownloadItems != null && saveItems.DownloadItems.Count > 1)
            {
                saveItems.CurrentItem.LocalFile = string.Format(@"{0}\{1} - {2}#{3}{4}",
                    System.IO.Path.GetDirectoryName(saveItems.CurrentItem.LocalFile),
                    System.IO.Path.GetFileNameWithoutExtension(saveItems.CurrentItem.LocalFile),
                    (saveItems.DownloadItems.IndexOf(saveItems.CurrentItem) + 1).ToString().PadLeft((saveItems.DownloadItems.Count).ToString().Length, '0'),
                    (saveItems.DownloadItems.Count).ToString(),
                    System.IO.Path.GetExtension(saveItems.CurrentItem.LocalFile));
            }

            saveItems.CurrentItem.LocalFile = Utils.GetNextFileName(saveItems.CurrentItem.LocalFile);
            saveItems.CurrentItem.ThumbFile = string.IsNullOrEmpty(saveItems.CurrentItem.VideoInfo.ThumbnailImage) ? saveItems.CurrentItem.VideoInfo.ImageUrl : saveItems.CurrentItem.VideoInfo.ThumbnailImage;

			if (DownloadManager.Instance.Contains(url))
            {
                GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (dlg != null)
                {
                    dlg.Reset();
                    dlg.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
					dlg.SetHeading(Translation.Instance.Error);
					dlg.SetText(Translation.Instance.AlreadyDownloading);
                    dlg.DoModal(GUIWindowManager.ActiveWindow);
                }
                return;
            }

            // make sure the target dir exists
            if (!(System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(saveItems.CurrentItem.LocalFile))))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(saveItems.CurrentItem.LocalFile));
            }

			DownloadManager.Instance.Add(url, saveItems.CurrentItem);

            GUIPropertyManager.SetProperty("#OnlineVideos.currentDownloads", DownloadManager.Instance.Count.ToString());

            System.Threading.Thread downloadThread = new System.Threading.Thread((System.Threading.ParameterizedThreadStart)delegate(object o)
            {
				DownloadList dlList = o as DownloadList;
				try
				{
					IDownloader dlHelper = null;
					if (dlList.CurrentItem.Url.ToLower().StartsWith("mms://")) dlHelper = new MMSDownloadHelper();
					else dlHelper = new HTTPDownloader();
					dlList.CurrentItem.Downloader = dlHelper;
					dlList.CurrentItem.Start = DateTime.Now;
					Exception exception = dlHelper.Download(dlList.CurrentItem);
					if (exception != null) Log.Instance.Warn("Error downloading '{0}', Msg: {1}", dlList.CurrentItem.Url, exception.Message);
					OnDownloadFileCompleted(dlList, exception);
				}
				catch (System.Threading.ThreadAbortException)
				{
					// the thread was aborted on purpose, let it finish gracefully
					System.Threading.Thread.ResetAbort();
				}
				catch (Exception ex)
				{
					Log.Instance.Warn("Error downloading '{0}', Msg: {1}", dlList.CurrentItem.Url, ex.Message);
					OnDownloadFileCompleted(dlList, ex);
				}
            });
            downloadThread.IsBackground = true;
            downloadThread.Name = "OVDownload";
            downloadThread.Start(saveItems);

            GUIDialogNotify dlgNotify = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
            if (dlgNotify != null)
            {
                dlgNotify.Reset();
                dlgNotify.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
				dlgNotify.SetHeading(Translation.Instance.DownloadStarted);
                dlgNotify.SetText(saveItems.CurrentItem.Title);
                dlgNotify.DoModal(GUIWindowManager.ActiveWindow);
            }
        }

        private void OnDownloadFileCompleted(DownloadList saveItems, Exception error)
        {
            DownloadManager.Instance.Remove(saveItems.CurrentItem.Url);

            GUIPropertyManager.SetProperty("#OnlineVideos.currentDownloads", DownloadManager.Instance.Count.ToString());

            if (error != null && !saveItems.CurrentItem.Downloader.Cancelled)
            {
                GUIDialogNotify loDlgNotify = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (loDlgNotify != null)
                {
                    loDlgNotify.Reset();
                    loDlgNotify.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
					loDlgNotify.SetHeading(Translation.Instance.Error);
					loDlgNotify.SetText(string.Format(Translation.Instance.DownloadFailed, saveItems.CurrentItem.Title));
                    loDlgNotify.DoModal(GUIWindowManager.ActiveWindow);
                }
            }
            else
            {
                // if the image given was an url -> check if thumb exists otherwise download
                if (saveItems.CurrentItem.ThumbFile.ToLower().StartsWith("http"))
                {
                    string thumbFile = Utils.GetThumbFile(saveItems.CurrentItem.ThumbFile);
                    if (System.IO.File.Exists(thumbFile)) saveItems.CurrentItem.ThumbFile = thumbFile;
                    else if (ImageDownloader.DownloadAndCheckImage(saveItems.CurrentItem.ThumbFile, thumbFile)) saveItems.CurrentItem.ThumbFile = thumbFile;
                }
                // save thumb for this video as well if it exists
                if (!saveItems.CurrentItem.ThumbFile.ToLower().StartsWith("http") && System.IO.File.Exists(saveItems.CurrentItem.ThumbFile))
                {
                    string localImageName = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(saveItems.CurrentItem.LocalFile),
                        System.IO.Path.GetFileNameWithoutExtension(saveItems.CurrentItem.LocalFile))
                        + System.IO.Path.GetExtension(saveItems.CurrentItem.ThumbFile);
                    System.IO.File.Copy(saveItems.CurrentItem.ThumbFile, localImageName, true);
                }

                // get file size
                int fileSize = saveItems.CurrentItem.KbTotal;
                if (fileSize <= 0)
                {
                    try { fileSize = (int)((new System.IO.FileInfo(saveItems.CurrentItem.LocalFile)).Length / 1024); }
                    catch { }
                }

                GUIDialogNotify loDlgNotify = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                if (loDlgNotify != null)
                {
                    loDlgNotify.Reset();
                    loDlgNotify.SetImage(GUIOnlineVideos.GetImageForSite("OnlineVideos", type: "Icon"));
                    if (saveItems.CurrentItem.Downloader.Cancelled)
						loDlgNotify.SetHeading(Translation.Instance.DownloadCancelled);
                    else
						loDlgNotify.SetHeading(Translation.Instance.DownloadComplete);
                    loDlgNotify.SetText(string.Format("{0}{1}", saveItems.CurrentItem.Title, fileSize > 0 ? " ( " + fileSize.ToString("n0") + " KB)" : ""));
                    loDlgNotify.DoModal(GUIWindowManager.ActiveWindow);
                }
            }

            // download the next if list not empty and not last in list and not cancelled by the user
            if (saveItems.DownloadItems != null && saveItems.DownloadItems.Count > 1 && !saveItems.CurrentItem.Downloader.Cancelled)
            {
                int currentDlIndex = saveItems.DownloadItems.IndexOf(saveItems.CurrentItem);
                if (currentDlIndex >= 0 && currentDlIndex + 1 < saveItems.DownloadItems.Count)
                {
                    saveItems.CurrentItem = saveItems.DownloadItems[currentDlIndex + 1];
                    GUIWindowManager.SendThreadCallbackAndWait((p1, p2, data) => { SaveVideo_Step1(saveItems); return 0; }, 0, 0, null);
                }
            }
        }

        private bool FilterOut(String fsStr)
        {
            if (fsStr == String.Empty)
            {
                return false;
            }
            if (PluginConfiguration.Instance.FilterArray != null)
            {
                foreach (String lsFilter in PluginConfiguration.Instance.FilterArray)
                {
                    if (fsStr.IndexOf(lsFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Log.Instance.Info("Filtering out:{0}\n based on filter:{1}", fsStr, lsFilter);
                        return true;
                        //return false;
                    }
                }
            }
            return false;
        }

        private void UpdateViewState()
        {
            switch (CurrentState)
            {
                case State.groups:
                    GUIPropertyManager.SetProperty("#header.label", PluginConfiguration.Instance.BasicHomeScreenName);
                    GUIPropertyManager.SetProperty("#header.image", GetImageForSite("OnlineVideos"));
                    ShowAndEnable(GUI_facadeView.GetID);
                    HideFilterButtons();
                    HideSearchButtons();
                    if (OnlineVideoSettings.Instance.UseAgeConfirmation && !OnlineVideoSettings.Instance.AgeConfirmed)
                        ShowAndEnable(GUI_btnEnterPin.GetID);
                    else
                        HideAndDisable(GUI_btnEnterPin.GetID);
                    currentView = PluginConfiguration.Instance.currentGroupView;
                    SetFacadeViewMode();
					GUIPropertyManager.SetProperty("#itemtype", Translation.Instance.Groups);
                    break;
                case State.sites:
                    GUIPropertyManager.SetProperty("#header.label", PluginConfiguration.Instance.BasicHomeScreenName + (selectedSitesGroup != null ? ": " + selectedSitesGroup.Label : ""));
                    GUIPropertyManager.SetProperty("#header.image", GetImageForSite("OnlineVideos"));
                    ShowAndEnable(GUI_facadeView.GetID);
                    HideFilterButtons();
                    ShowOrderButtons();
                    HideSearchButtons();
                    if (OnlineVideoSettings.Instance.UseAgeConfirmation && !OnlineVideoSettings.Instance.AgeConfirmed)
                        ShowAndEnable(GUI_btnEnterPin.GetID);
                    else
                        HideAndDisable(GUI_btnEnterPin.GetID);
                    currentView = PluginConfiguration.Instance.currentSiteView;
                    SetFacadeViewMode();
					GUIPropertyManager.SetProperty("#itemtype", Translation.Instance.Sites);
                    break;
                case State.categories:
                    string cat_headerlabel = selectedCategory != null ? selectedCategory.RecursiveName() : SelectedSite.Settings.Name;
                    GUIPropertyManager.SetProperty("#header.label", cat_headerlabel);
                    GUIPropertyManager.SetProperty("#header.image", GetImageForSite(SelectedSite.Settings.Name, SelectedSite.Settings.UtilName));
                    ShowAndEnable(GUI_facadeView.GetID);
                    HideFilterButtons();
                    if (SelectedSite.CanSearch) ShowSearchButtons(); else HideSearchButtons();
                    HideAndDisable(GUI_btnEnterPin.GetID);
                    currentView = suggestedView != null ? suggestedView.Value : PluginConfiguration.Instance.currentCategoryView;
                    SetFacadeViewMode();
					GUIPropertyManager.SetProperty("#itemtype", Translation.Instance.Categories);
                    break;
                case State.videos:
                    switch (currentVideosDisplayMode)
                    {
						case VideosMode.Search: GUIPropertyManager.SetProperty("#header.label", Translation.Instance.SearchResults + " [" + lastSearchQuery + "]"); break;
                        default:
                            {
                                string proposedLabel = SelectedSite.getCurrentVideosTitle();
                                GUIPropertyManager.SetProperty("#header.label", proposedLabel != null ? proposedLabel : selectedCategory != null ? selectedCategory.RecursiveName() : ""); break;
                            }
                    }
                    GUIPropertyManager.SetProperty("#header.image", GetImageForSite(SelectedSite.Settings.Name, SelectedSite.Settings.UtilName));
                    ShowAndEnable(GUI_facadeView.GetID);
                    if (SelectedSite is IFilter) ShowFilterButtons(); else HideFilterButtons();
                    if (SelectedSite.CanSearch) ShowSearchButtons(); else HideSearchButtons();
                    if (SelectedSite.HasFilterCategories) ShowCategoryButton();
                    HideAndDisable(GUI_btnEnterPin.GetID);
                    currentView = suggestedView != null ? suggestedView.Value : PluginConfiguration.Instance.currentVideoView;
                    SetFacadeViewMode();
					GUIPropertyManager.SetProperty("#itemtype", Translation.Instance.Videos);
                    break;
                case State.details:
                    GUIPropertyManager.SetProperty("#header.label", selectedVideo.Title);
                    GUIPropertyManager.SetProperty("#header.image", GetImageForSite(SelectedSite.Settings.Name, SelectedSite.Settings.UtilName));
                    HideAndDisable(GUI_facadeView.GetID);
                    HideFilterButtons();
                    HideSearchButtons();
                    HideAndDisable(GUI_btnEnterPin.GetID);
                    break;
            }
            if (CurrentState == State.details)
            {
                GUI_infoList.Focus = true;
                GUIControl.FocusControl(GetID, GUI_infoList.GetID);
            }
            else
            {
                GUI_facadeView.Focus = true;
                GUIControl.FocusControl(GetID, GUI_facadeView.GetID);
            }
        }

        private void ShowOrderButtons()
        {
            ShowAndEnable(GUI_btnOrderBy.GetID);
            ShowAndEnable(GUI_btnUpdate.GetID);
        }

        private void HideFilterButtons()
        {
            HideAndDisable(GUI_btnMaxResult.GetID);
            HideAndDisable(GUI_btnTimeFrame.GetID);
            HideAndDisable(GUI_btnOrderBy.GetID);
            HideAndDisable(GUI_btnUpdate.GetID);
        }

        private void ShowFilterButtons()
        {
            GUI_btnMaxResult.Clear();
            GUI_btnOrderBy.Clear();
            GUI_btnTimeFrame.Clear();

            moSupportedMaxResultList = ((IFilter)SelectedSite).getResultSteps();
            foreach (int step in moSupportedMaxResultList)
            {
                GUIControl.AddItemLabelControl(GetID, GUI_btnMaxResult.GetID, step + "");
            }
            moSupportedOrderByList = ((IFilter)SelectedSite).getOrderbyList();
            foreach (String orderBy in moSupportedOrderByList.Keys)
            {
                GUIControl.AddItemLabelControl(GetID, GUI_btnOrderBy.GetID, orderBy);
            }
            moSupportedTimeFrameList = ((IFilter)SelectedSite).getTimeFrameList();
            foreach (String time in moSupportedTimeFrameList.Keys)
            {
                GUIControl.AddItemLabelControl(GetID, GUI_btnTimeFrame.GetID, time);
            }

            ShowAndEnable(GUI_btnMaxResult.GetID);
            ShowAndEnable(GUI_btnOrderBy.GetID);
            ShowAndEnable(GUI_btnTimeFrame.GetID);
            ShowAndEnable(GUI_btnUpdate.GetID);

            if (SelectedMaxResultIndex > -1)
            {
                GUIControl.SelectItemControl(GetID, GUI_btnMaxResult.GetID, SelectedMaxResultIndex);
            }
            if (SelectedOrderByIndex > -1)
            {
                GUIControl.SelectItemControl(GetID, GUI_btnOrderBy.GetID, SelectedOrderByIndex);
            }
            if (SelectedTimeFrameIndex > -1)
            {
                GUIControl.SelectItemControl(GetID, GUI_btnTimeFrame.GetID, SelectedTimeFrameIndex);
            }
        }

        private void HideSearchButtons()
        {
            HideAndDisable(GUI_btnSearchCategories.GetID);
            HideAndDisable(GUI_btnSearch.GetID);
        }

        private void ShowSearchButtons()
        {
            GUI_btnSearchCategories.Clear();
            moSupportedSearchCategoryList = SelectedSite.GetSearchableCategories();
			GUIControl.AddItemLabelControl(GetID, GUI_btnSearchCategories.GetID, Translation.Instance.All);
            foreach (String category in moSupportedSearchCategoryList.Keys)
            {
                GUIControl.AddItemLabelControl(GetID, GUI_btnSearchCategories.GetID, category);
            }
            if (moSupportedSearchCategoryList.Count >= 1)
            {
                ShowAndEnable(GUI_btnSearchCategories.GetID);
            }
            ShowAndEnable(GUI_btnSearch.GetID);
            if (SelectedSearchCategoryIndex > -1)
            {
                Log.Instance.Info("restoring search category...");
                GUIControl.SelectItemControl(GetID, GUI_btnSearchCategories.GetID, SelectedSearchCategoryIndex);
                Log.Instance.Info("Search category restored to " + GUI_btnSearchCategories.SelectedLabel);
            }
        }

        private void ShowCategoryButton()
        {
            Log.Instance.Debug("Showing Category button");
            GUI_btnSearchCategories.Clear();
            moSupportedSearchCategoryList = SelectedSite.GetSearchableCategories();
            foreach (String category in moSupportedSearchCategoryList.Keys)
                GUIControl.AddItemLabelControl(GetID, GUI_btnSearchCategories.GetID, category);
            if (moSupportedSearchCategoryList.Count > 1)
            {
                ShowAndEnable(GUI_btnSearchCategories.GetID);
                ShowAndEnable(GUI_btnUpdate.GetID);
            }
        }

        private void ToggleFacadeViewMode()
        {
            switch (currentView)
            {
#if MP11
                case GUIFacadeControl.ViewMode.List:
                    currentView = GUIFacadeControl.ViewMode.SmallIcons; break;
                case GUIFacadeControl.ViewMode.SmallIcons:
                    currentView = GUIFacadeControl.ViewMode.LargeIcons; break;
                case GUIFacadeControl.ViewMode.LargeIcons:
                    currentView = GUIFacadeControl.ViewMode.List; break;
#else
                case GUIFacadeControl.Layout.List:
                    currentView = GUIFacadeControl.Layout.SmallIcons; break;
                case GUIFacadeControl.Layout.SmallIcons:
                    currentView = GUIFacadeControl.Layout.LargeIcons; break;
                case GUIFacadeControl.Layout.LargeIcons:
                    currentView = GUIFacadeControl.Layout.List; break;
#endif
            }
            switch (CurrentState)
            {
                case State.groups: PluginConfiguration.Instance.currentGroupView = currentView; break;
                case State.sites: PluginConfiguration.Instance.currentSiteView = currentView; break;
                case State.categories: PluginConfiguration.Instance.currentCategoryView = currentView; break;
                case State.videos: PluginConfiguration.Instance.currentVideoView = currentView; break;
            }
            if (CurrentState != State.details) SetFacadeViewMode();
        }

        protected void SetFacadeViewMode()
        {
            if (GUI_facadeView == null) return;

            string strLine = String.Empty;
            switch (currentView)
            {
#if MP11
                case GUIFacadeControl.ViewMode.List:
                    strLine = Translation.LayoutList;
                    break;
                case GUIFacadeControl.ViewMode.SmallIcons:
                    strLine = Translation.LayoutIcons;
                    break;
                case GUIFacadeControl.ViewMode.LargeIcons:
                    strLine = Translation.LayoutBigIcons;
                    break;
#else
                case GUIFacadeControl.Layout.List:
					strLine = Translation.Instance.LayoutList;
                    break;
                case GUIFacadeControl.Layout.SmallIcons:
					strLine = Translation.Instance.LayoutIcons;
                    break;
                case GUIFacadeControl.Layout.LargeIcons:
					strLine = Translation.Instance.LayoutBigIcons;
                    break;
#endif
            }
            GUIControl.SetControlLabel(GetID, GUI_btnViewAs.GetID, strLine);

            //set object count label
            int itemcount = GUI_facadeView.Count;
            if (itemcount > 0)
            {
                if (GUI_facadeView[0].Label == "..") itemcount--;
                if (itemcount > 0 && (GUI_facadeView[GUI_facadeView.Count - 1] as OnlineVideosGuiListItem).Item == null) itemcount--;
            }
            GUIPropertyManager.SetProperty("#itemcount", itemcount.ToString());

            // keep track of the currently selected item (is lost when switching view)
            int rememberIndex = GUI_facadeView.SelectedListItemIndex;
#if MP11
            GUI_facadeView.View = currentView; // explicitly set the view (fixes bug that facadeView.list isn't working at startup
#else
            GUI_facadeView.CurrentLayout = currentView; // explicitly set the view (fixes bug that facadeView.list isn't working at startup
#endif
            if (rememberIndex > -1) GUIControl.SelectItemControl(GetID, GUI_facadeView.GetID, rememberIndex);
        }

        /// <summary>
        /// Displays a modal dialog, with a list of the PlaybackOptions to the user, 
        /// only if PlaybackOptions holds more than one entry.
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <param name="defaultUrl">will be set to -1 when the user canceled the dialog, will not be touched if no playbackoptions are set, otherwise set to the chosen key</param>
        /// <returns>true when a choice from the PlaybackOptions was made (or only one option was available)</returns>
        private bool DisplayPlaybackOptions(VideoInfo videoInfo, ref string defaultUrl)
        {
            // with no options set, return the VideoUrl field
            if (videoInfo.PlaybackOptions == null || videoInfo.PlaybackOptions.Count == 0) return false;
            // with just one option set, return that options url
            if (videoInfo.PlaybackOptions.Count == 1)
            {
                defaultUrl = videoInfo.PlaybackOptions.First().Key;
            }
            else
            {
                int defaultOption = -1;
                // show a list of available options and let the user decide
                GUIDialogMenu dlgSel = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
                if (dlgSel != null)
                {
                    dlgSel.Reset();
					dlgSel.SetHeading(Translation.Instance.SelectSource);
                    int option = 0;
                    foreach (string key in videoInfo.PlaybackOptions.Keys)
                    {
                        dlgSel.Add(key);
                        if (videoInfo.PlaybackOptions[key] == defaultUrl) defaultOption = option;
                        option++;
                    }
                }
                if (defaultOption != -1) dlgSel.SelectedLabel = defaultOption;
                dlgSel.DoModal(GUIWindowManager.ActiveWindow);
                defaultUrl = (dlgSel.SelectedId == -1) ? "-1" : dlgSel.SelectedLabelText;
            }
            return true;
        }

        internal static string GetImageForSite(string siteName, string utilName = "", string type = "Banner")
        {
            // use png with the same name as the Site - first check subfolder of current skin (allows skinners to use custom icons)
            string image = string.Format(@"{0}\Media\OnlineVideos\{1}s\{2}.png", GUIGraphicsContext.Skin, type, siteName);
            if (!System.IO.File.Exists(image))
            {
                // use png with the same name as the Site
                image = string.Format(@"{0}\OnlineVideos\{1}s\{2}.png", Config.GetFolder(Config.Dir.Thumbs), type, siteName);
                if (!System.IO.File.Exists(image))
                {
                    image = string.Empty;
                    // if that does not exist, try image with the same name as the Util
                    if (!string.IsNullOrEmpty(utilName))
                    {
                        image = string.Format(@"{0}\OnlineVideos\{1}s\{2}.png", Config.GetFolder(Config.Dir.Thumbs), type, utilName);
                        if (!System.IO.File.Exists(image)) image = string.Empty;
                    }
                }
            }
            return image;
        }

        internal void SetGuiProperties_PlayingVideo(PlayListItem playItem)
        {
            // first reset our own properties
            GUIPropertyManager.SetProperty("#Play.Current.OnlineVideos.SiteIcon", string.Empty);

            // start a thread that will set the properties in 2 seconds (otherwise MediaPortal core logic would overwrite them)
            if (playItem == null || playItem.Video == null) return;
            new System.Threading.Thread(delegate(object o)
            {
				try
				{
					VideoInfo video = (o as PlayListItem).Video;
					string alternativeTitle = (o as PlayListItem).Description;
					Sites.SiteUtilBase site = (o as PlayListItem).Util;

					System.Threading.Thread.Sleep(2000);
					Log.Instance.Info("Setting Video Properties.");

					string quality = "";
					if (video.PlaybackOptions != null && video.PlaybackOptions.Count > 1)
					{
						var enumer = video.PlaybackOptions.GetEnumerator();
						while (enumer.MoveNext())
						{
							string compareTo = enumer.Current.Value.ToLower().StartsWith("rtmp") ?
								ReverseProxy.Instance.GetProxyUri(RTMP_LIB.RTMPRequestHandler.Instance, string.Format("http://127.0.0.1/stream.flv?rtmpurl={0}", System.Web.HttpUtility.UrlEncode(enumer.Current.Value)))
								: enumer.Current.Value;
							if (compareTo == g_Player.CurrentFile)
							{
								quality = " (" + enumer.Current.Key + ")";
								break;
							}
						}
					}

					if (!string.IsNullOrEmpty(alternativeTitle))
						GUIPropertyManager.SetProperty("#Play.Current.Title", alternativeTitle);
					else if (!string.IsNullOrEmpty(video.Title))
						GUIPropertyManager.SetProperty("#Play.Current.Title", video.Title + (string.IsNullOrEmpty(quality) ? "" : quality));
					if (!string.IsNullOrEmpty(video.Description)) GUIPropertyManager.SetProperty("#Play.Current.Plot", video.Description);
					if (!string.IsNullOrEmpty(video.ThumbnailImage)) GUIPropertyManager.SetProperty("#Play.Current.Thumb", video.ThumbnailImage);

					if (!string.IsNullOrEmpty(video.Airdate)) GUIPropertyManager.SetProperty("#Play.Current.Year", video.Airdate);
					else if (!string.IsNullOrEmpty(video.Length)) GUIPropertyManager.SetProperty("#Play.Current.Year", VideoInfo.GetDuration(video.Length));

					if (site != null) GUIPropertyManager.SetProperty("#Play.Current.OnlineVideos.SiteIcon", GetImageForSite(site.Settings.Name, site.Settings.UtilName, "Icon"));
				}
				catch (Exception ex)
				{
					Log.Instance.Warn("Error setting playing video properties: {0}", ex.ToString());
				}
            }) { IsBackground = true, Name = "OVPlaying" }.Start(playItem);

            TrackPlayback();
        }

        /// <summary>
        /// Processes extended properties which might be available
        /// if the VideoInfo.Other object is using the IVideoDetails interface
        /// </summary>
        /// <param name="videoInfo">if this param is null, the <see cref="selectedVideo"/> will be used</param>
        private void SetGuiProperties_ExtendedVideoInfo(VideoInfo videoInfo, bool DetailsItem)
        {
            string prefix = "#OnlineVideos.";
            if (!DetailsItem)
            {
                ResetExtendedGuiProperties(prefix); // remove everything
                if (videoInfo == null) videoInfo = selectedVideo; // set everything for the selected video in the next step if given video is null
                prefix = prefix + "Details.";
            }
            else
            {
                prefix = prefix + "DetailsItem.";
                ResetExtendedGuiProperties(prefix); // remove all entries for the last selected "DetailsItem" (will be set for the parameter in the next step)
            }

            if (videoInfo != null)
            {
				Dictionary<string, string> custom = videoInfo.GetExtendedProperties();
				if (custom != null)
				{
					foreach (string property in custom.Keys)
					{
						string label = prefix + property;
						string value = custom[property];
						SetExtendedGuiProperty(label, value);
					}
				}
            }
        }

        /// <summary>
        /// Set an extended property in the GUIPropertyManager
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetExtendedGuiProperty(string key, string value)
        {
            lock (extendedProperties)
            {
                extendedProperties.Add(key);
                GUIPropertyManager.SetProperty(key, value);
            }
        }

        /// <summary>
        /// Clears all known set extended property values
        /// </summary>
        /// <param name="prefix">prefix</param>
        public void ResetExtendedGuiProperties(string prefix)
        {
            lock (extendedProperties)
            {
                if (extendedProperties.Count == 0)
                {
                    return;
                }

                string[] keys = extendedProperties.Where(s => s.StartsWith(prefix)).ToArray();
                for (int i = 0; i < keys.Length; i++)
                {
                    GUIPropertyManager.SetProperty(keys[i], string.Empty);
                    extendedProperties.Remove(keys[i]);
                }
            }
        }

        private void ResetSelectedSite()
        {
            GUIPropertyManager.SetProperty("#OnlineVideos.selectedSite", string.Empty);
            GUIPropertyManager.SetProperty("#OnlineVideos.selectedSiteUtil", string.Empty);
            GUIPropertyManager.SetProperty("#OnlineVideos.desc", string.Empty);
        }

		public void ResetToFirstView()
		{
			selectedSitesGroup = null;
			SelectedSite = null;
			selectedCategory = null;
			selectedVideo = null;
			currentVideoList = new List<VideoInfo>();
			currentTrailerList = new List<VideoInfo>();
			currentPlaylist = null;
			currentPlayingItem = null;
			CurrentState = State.groups;
		}

        #endregion
    }
}