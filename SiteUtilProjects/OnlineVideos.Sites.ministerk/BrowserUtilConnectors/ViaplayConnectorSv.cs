﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineVideos.Sites.BrowserUtilConnectors
{
    public class ViaplayConnectorSv : ViaplayConnectorBase
    {
        public override string BaseUrl
        {
            get { return "http://viaplay.se"; }
        }

        public override string LoginUrl
        {
            get { return "https://account.viaplay.se/login"; }
        }
    }
}
