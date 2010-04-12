﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using HybridDSP.Net.HTTP;

namespace OnlineVideos {

    #region Interfaces

    public interface IProxyHandler { }

    public interface IRequestHandler : IProxyHandler {
        void HandleRequest(string url, HTTPServerRequest request, HTTPServerResponse response);
    }

    public interface ISimpleRequestHandler : IProxyHandler {
        void UpdateRequest(HttpWebRequest request);
    }

    #endregion

    /// <summary>
    /// Generic Reverse Proxy Server
    /// </summary>
    public static class ReverseProxy {

        const int ProxyPortRangeStart = 30006;

        static HTTPServer _server = null;
        static int _serverPort = 0;
        static List<IProxyHandler> _handlers;

        static ReverseProxy() {
            _serverPort = getFirstAvailablePort();
            _handlers = new List<IProxyHandler>();
            _server = new HTTPServer(new RequestHandlerFactory(), _serverPort);
            _server.OnServerException += new HTTPServer.ServerCaughtException(delegate(Exception ex) { Log.Error(ex.Message); });
            _server.Start();
        }

        public static void StopListening() {
            _server.Stop();
        }

        /// <summary>
        /// Add a new request handler
        /// </summary>
        /// <param name="handler"></param>
        public static void AddHandler(IProxyHandler handler) {
            if (!_handlers.Contains(handler)) {
                _handlers.Add(handler);
            }
        }

        /// <summary>
        /// Returns the proxy url based on the handler and original uri
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetProxyUri(IProxyHandler handler, string uri) {
            int index = _handlers.IndexOf(handler);
            return string.Format("http://127.0.0.1:{0}/{1}/{2}", _serverPort, index, uri.Substring(7));
        }

        class RequestHandlerFactory : IHTTPRequestHandlerFactory {
            public IHTTPRequestHandler CreateRequestHandler(HTTPServerRequest request) {
                return new RequestHandler();
            }
        }

        class RequestHandler : IHTTPRequestHandler {
            
            public bool DetectInvalidPackageHeader() {
                return false;
            }

            public void HandleRequest(HTTPServerRequest request, HTTPServerResponse response) {
                if (_handlers.Count == 0)
                    return;

                // Create the actual url from the request
                string uri = request.URI;
                int startIndex = uri.IndexOf('/', 1);
                string url = "http:/" + uri.Substring(startIndex);

                string parsedHandlerIndex = uri.Substring(1, startIndex - 1);
                int handlerIndex;

                // dont continue if the handler index is invalid
                if (!int.TryParse(parsedHandlerIndex, out handlerIndex)) {
                    return;
                } else if (handlerIndex >= _handlers.Count)
                    return;

                IProxyHandler handler = _handlers[handlerIndex];

                // if the handler implements IRequestHandler it will take over 
                // the complete request so execute and return
                if (handler is IRequestHandler) {
                    ((IRequestHandler)handler).HandleRequest(url, request, response);
                    return;
                } // else only continue if handler implements ISimpleRequestHandler
                else if (!(handler is ISimpleRequestHandler))
                    return;

                HttpWebRequest proxyRequest = WebRequest.Create(url) as HttpWebRequest;

                if (proxyRequest == null) {
                    response.Status = HTTPServerResponse.HTTPStatus.HTTP_NOT_FOUND;
                    response.Send().Close();
                }
                else {
                    // updates the proxy request
                    ((ISimpleRequestHandler)handler).UpdateRequest(proxyRequest);

                    WebResponse siteResponse = proxyRequest.GetResponse();
                    // copy response settings
                    response.ContentType = siteResponse.ContentType;
                    response.ContentLength = siteResponse.ContentLength;
                    // restream data
                    Stream responseStream = response.Send();
                    Stream siteResponseStream = siteResponse.GetResponseStream();
                    int read = 0;
                    while (read < siteResponse.ContentLength) {
                        byte[] data = new byte[1024];
                        int fetched = siteResponseStream.Read(data, 0, 1024);
                        read += fetched;
                        responseStream.Write(data, 0, fetched);
                        if (fetched == 0 || read >= siteResponse.ContentLength) break;
                    }
                    responseStream.Flush();
                    responseStream.Close();
                }
            }
        }

        /// <summary>
        /// Gets the first available port number
        /// </summary>
        /// <returns></returns>
        private static int getFirstAvailablePort() {
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            List<TcpConnectionInformation> connections = new List<TcpConnectionInformation>(properties.GetActiveTcpConnections());
            foreach (TcpConnectionInformation c in connections) {
                int port = c.LocalEndPoint.Port;
                if (port >= ProxyPortRangeStart)
                    portArray.Add(port);
            }

            //getting active tcp listners - WCF service listening in tcp
            List<IPEndPoint> endPoints = new List<IPEndPoint>(properties.GetActiveTcpListeners());
            foreach (IPEndPoint p in endPoints) {
                int port = p.Port;
                if (port >= ProxyPortRangeStart)
                    portArray.Add(port);
            }

            //getting active udp listeners
            endPoints = new List<IPEndPoint>(properties.GetActiveTcpListeners());
            foreach (IPEndPoint p in endPoints) {
                int port = p.Port;
                if (port >= ProxyPortRangeStart)
                    portArray.Add(port);
            }

            portArray.Sort();
            for (int i = ProxyPortRangeStart; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }

    }

}

