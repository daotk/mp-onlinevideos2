﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineVideos.MediaPortal2
{
    public static class Guids
    {
		public static readonly string WorkflowStateCategoriesName = "OnlineVideos_Categories_WorkflowState";

        public static readonly Guid WorkFlowModelOV = new Guid("C418243F-5BD3-4637-8871-DA6545387929");
        public static readonly Guid WorkflowStateSites = new Guid("F9D7500D-EC5C-4FEF-8FAE-E4DED8A22CE0");
        public static readonly Guid WorkflowStateVideos = new Guid("FF474A1A-CA39-4247-BFEA-4E7B578F482B");
        public static readonly Guid WorkflowStateDetails = new Guid("F7DF593E-A606-4096-B8E1-BE702C43A325");
		public static readonly Guid DialogStateSearch = new Guid("F068C0DE-3763-4BA1-A59F-24435DBF0227");

		public static readonly Guid WorkflowStateSiteSettings = new Guid("BFDE18C2-0019-43D5-8ED7-8C9C426CE4A1");
		
		public static readonly Guid WorkFlowModelSiteManagement = new Guid("C39D6682-90B5-4813-8A28-A1C9118D4F3E");
		public static readonly Guid WorkflowStateSiteManagement = new Guid("026DF45C-86CB-44AE-9174-114810A6BAFF");
		public static readonly Guid DialogStateSiteUpdate = new Guid("F6E1864A-4B9E-45FF-8200-23F4C1798562");

		public static readonly Guid FilterOwnerAction = new Guid("{17DF1977-A632-4303-974E-B78B63836F75}");
		public static readonly Guid FilterLanguageAction = new Guid("{0721D166-A47D-4F07-BA9A-169204B0FB85}");
		public static readonly Guid FilterStateAction = new Guid("{DE583520-FCE1-4F0C-AD35-BF096BFDCABD}");
		public static readonly Guid SortSitesAction = new Guid("{2B2F6396-F2A8-4BBD-B483-B0A96697D8A5}");
    }

	public static class Constants
	{
		public const string KEY_PLAYBACK_URLS = "ovsPlaybackUrls";
		public const string KEY_VALUE = "ovsItemValue";

		public const string CONTEXT_VAR_ITEMS = "Items";
		public const string CONTEXT_VAR_COMMAND = "Command";
	}
}
