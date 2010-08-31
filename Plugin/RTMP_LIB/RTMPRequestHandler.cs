﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading;

namespace RTMP_LIB
{
    public class RTMPRequestHandler : OnlineVideos.IRequestHandler
    {
        #region Singleton
        private static RTMPRequestHandler _Instance = null;
        public static RTMPRequestHandler Instance
        {
            get
            {
                if (_Instance == null) _Instance = new RTMPRequestHandler();
                return _Instance;
            }
        }
        private RTMPRequestHandler() { }
        #endregion

        bool invalidHeader = false;

        public bool DetectInvalidPackageHeader()
        {
            return invalidHeader;
        }

        public void HandleRequest(string url, HybridDSP.Net.HTTP.HTTPServerRequest request, HybridDSP.Net.HTTP.HTTPServerResponse response)
        {
            Thread.CurrentThread.Name = "RTMPProxy";
            Logger.Log("Request");
            RTMP rtmp = null;
            Stream responseStream = null;
            try
            {
                NameValueCollection paramsHash = System.Web.HttpUtility.ParseQueryString(new Uri(url).Query);

                Link link = new Link();
                if (!string.IsNullOrEmpty(paramsHash["rtmpurl"])) link = Link.FromRtmpUrl(new Uri(paramsHash["rtmpurl"]));
                if (!string.IsNullOrEmpty(paramsHash["app"])) link.app = paramsHash["app"];
                if (!string.IsNullOrEmpty(paramsHash["tcUrl"])) link.tcUrl = paramsHash["tcUrl"];
                if (!string.IsNullOrEmpty(paramsHash["hostname"])) link.hostname = paramsHash["hostname"];
                if (!string.IsNullOrEmpty(paramsHash["port"])) link.port = int.Parse(paramsHash["port"]);
                if (!string.IsNullOrEmpty(paramsHash["playpath"])) link.playpath = paramsHash["playpath"];
                if (!string.IsNullOrEmpty(paramsHash["subscribepath"])) link.subscribepath = paramsHash["subscribepath"];
                if (!string.IsNullOrEmpty(paramsHash["pageurl"])) link.pageUrl = paramsHash["pageurl"];
                if (!string.IsNullOrEmpty(paramsHash["swfurl"])) link.swfUrl = paramsHash["swfurl"];
                if (!string.IsNullOrEmpty(paramsHash["swfsize"])) link.SWFSize = int.Parse(paramsHash["swfsize"]);
                if (!string.IsNullOrEmpty(paramsHash["swfhash"])) link.SWFHash = Link.ArrayFromHexString(paramsHash["swfhash"]);
                if (!string.IsNullOrEmpty(paramsHash["auth"])) link.auth = paramsHash["auth"];
                if (!string.IsNullOrEmpty(paramsHash["token"])) link.token = paramsHash["token"];
                if (!string.IsNullOrEmpty(paramsHash["conn"])) link.extras = Link.ParseAMF(paramsHash["conn"]);
                if (link.tcUrl != null && link.tcUrl.ToLower().StartsWith("rtmpe")) link.protocol = Protocol.RTMPE;

                rtmp = new RTMP();
                bool connected = rtmp.Connect(link);
                
                if (connected)
                {
                    request.KeepAlive = true; // keep connection alive
                    response.ContentType = "video/x-flv";
                    response.KeepAlive = true;
                    //response.ChunkedTransferEncoding = true;
                    
                    FLVStream fs = new FLVStream();
                    
                    fs.WriteFLV(rtmp, delegate()
                    {
                        // we must set a content length for the File Source filter, otherwise it thinks we have no content
                        // but don't set a length if it is our user agent, so a download will always be complete
                        if (request.Get("User-Agent") != OnlineVideos.OnlineVideoSettings.Instance.UserAgent)
                            response.ContentLength = fs.EstimatedLength;      
                                                
                        responseStream = response.Send();
                        return responseStream;
                    }, response._session._socket);

                    invalidHeader = rtmp.invalidRTMPHeader;

                    if (responseStream != null)
                    {
                        if (request.Get("User-Agent") != OnlineVideos.OnlineVideoSettings.Instance.UserAgent)
                        {
                            // keep appending "0" - bytes until we filled the estimated length when sending data to the File Source filter
                            long zeroBytes = fs.EstimatedLength - fs.Length;
                            while (zeroBytes > 0)
                            {
                                int chunk = (int)Math.Min(4096, zeroBytes);
                                byte[] buffer = new byte[chunk];
                                responseStream.Write(buffer, 0, chunk);
                                zeroBytes -= chunk;
                            }
                        }
                        responseStream.Close();
                    }
                }
                else
                {
                    response.StatusAndReason = HybridDSP.Net.HTTP.HTTPServerResponse.HTTPStatus.HTTP_INTERNAL_SERVER_ERROR;
                    response.Send().Close();
                }
            }
            catch (Exception ex)
            {
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                else
                {
                    // no data to play was ever received and send to the requesting client -> send an error now
                    response.ContentLength = 0;
                    response.StatusAndReason = HybridDSP.Net.HTTP.HTTPServerResponse.HTTPStatus.HTTP_INTERNAL_SERVER_ERROR;
                    response.Send().Close();
                }
                Logger.Log(ex.ToString());                
            }
            finally
            {
                if (rtmp != null) rtmp.Close();
            }

            Logger.Log("Request finished.");
        }
    }
}
