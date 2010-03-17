﻿using System;
using MediaPortal.Player;

namespace OnlineVideos.Player
{
    public class PlayerFactory : IPlayerFactory
    {
        PlayerType playerType = PlayerType.Auto;        

        public PlayerFactory(PlayerType playerType)
        {
            this.playerType = playerType;
        }
        
        public IPlayer Create(string filename)
        {
            return Create(filename, g_Player.MediaType.Video);
        }  

        public IPlayer Create(string filename, g_Player.MediaType type)
        {
            switch (playerType)
            {
                case PlayerType.Internal:
                    return new OnlineVideosPlayer();
                case PlayerType.WMP:
                    return new WMPVideoPlayer();
                default:
                    Uri uri = new Uri(filename);

                    if (uri.Scheme == "rtsp" || uri.Scheme == "mms" || uri.PathAndQuery.Contains(".asf"))
                    {
                        return new OnlineVideosPlayer();
                    }
                    else if (uri.PathAndQuery.Contains(".asx"))
                    {
                        return new WMPVideoPlayer();
                    }
                    else
                    {
                        foreach (string anExt in OnlineVideoSettings.Instance.VideoExtensions.Keys)
                        {
                            if (uri.PathAndQuery.Contains(anExt))
                            {
                                if (anExt == ".wmv" && !string.IsNullOrEmpty(uri.Query))
                                {
                                    return new WMPVideoPlayer();
                                }
                                else
                                {
                                    return new OnlineVideosPlayer();
                                }
                            }
                        }
                        return new WMPVideoPlayer();
                    }
            }            
        }              
    }
}
