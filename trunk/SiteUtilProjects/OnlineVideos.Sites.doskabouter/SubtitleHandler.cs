﻿#if SUBTITLE
extern alias MySubtitleDownloader; //resolves conflicts with HtmlAgilityPack elsewhere in this project
using MySubtitleDownloader.SubtitleDownloader.Core;
#endif
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Threading;

namespace OnlineVideos.Subtitles
{
    /// <summary>
    /// Helper class to easily incorporate subtitles into a siteutil that supports ITrackingInfo
    /// Class needs the SubtitleDownloader http://www.team-mediaportal.com/extensions/movies-videos/subtitledownloader plugin
    /// </summary>
    public class SubtitleHandler
    {
        // keep sdObject an object, so that the constructor doesn't throw an ecxeption is subtitledownloader isn't installed
        private object sdObject = null;
        private bool tryLoadSubtitles = false;

        //smallest value has the highest prio
        private Dictionary<string, int> languagePrios = new Dictionary<string, int>();
        public delegate ITrackingInfo GetTrackingInfo(VideoInfo video);

        public SubtitleHandler(string className, string languages)
        {
            Log.Debug(String.Format("Create subtitlehandler for '{0}', languages '{1}'", className, languages));

            string[] langs = languages.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < langs.Length; i++)
                languagePrios.Add(langs[i], i);

            className = className.Trim();
            tryLoadSubtitles = !String.IsNullOrEmpty(className);

            try
            {
                if (tryLoadSubtitles)
                {
#if SUBTITLE
                    tryLoadSubtitles = tryLoad(className);
#endif
                }
                else
                    Log.Debug("SubtitleDownloader: classname empty");
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                    Log.Debug("SubtitleDownloader not installed");
                else
                    Log.Warn("Exception while creating SubtitleDownloader " + e.ToString());
                tryLoadSubtitles = false;
            }
        }

        public void SetSubtitleText(VideoInfo video, GetTrackingInfo getTrackingInfo)
        {
            if (tryLoadSubtitles)
            {
                ITrackingInfo it = getTrackingInfo(video);
                if (sdObject != null && it != null && String.IsNullOrEmpty(video.SubtitleText))
                    try
                    {
#if SUBTITLE
                        setSubtitleText(video, it);
#endif
                    }
                    catch (Exception e)
                    {
                        Log.Warn("SubtitleDownloader: " + e.ToString());
                    }
            }
        }

        public void SetSubtitleText(VideoInfo video, GetTrackingInfo getTrackingInfo, out Thread thread)
        {
            thread = null;
            if (tryLoadSubtitles)
            {
                ITrackingInfo it = getTrackingInfo(video);
                if (sdObject != null && it != null && String.IsNullOrEmpty(video.SubtitleText))
                {
                    thread = new Thread(
                        delegate()
                        {
                            try
                            {
#if SUBTITLE
                                setSubtitleText(video, it);
#endif
                            }
                            catch (Exception e)
                            {
                                Log.Warn("SubtitleDownloader: " + e.ToString());
                            }
                        });
                    thread.Start();
                }
            }
        }

        public void WaitForSubtitleCompleted(Thread thread)
        {
            if (thread != null)
                thread.Join();
        }

#if SUBTITLE

        // keep all references to subtitledownloader in separate methods, so that methods that are called from siteutil don't throw an ecxeption
        private bool tryLoad(string className)
        {
            Assembly subAssembly = Assembly.GetAssembly(typeof(ISubtitleDownloader));
            Type tt = subAssembly.GetType(String.Format("SubtitleDownloader.Implementations.{0}.{0}Downloader", className), false, true);
            if (tt == null)
            {
                Log.Debug("Subtitlehandler for " + className + " cannot be created");
                return false;
            }
            else
            {
                sdObject = (ISubtitleDownloader)Activator.CreateInstance(tt);
                Log.Debug("Subtitlehandler " + sdObject.ToString() + " successfully created");
                return true;
            }
        }

        private void setSubtitleText(VideoInfo video, ITrackingInfo it)
        {
            ISubtitleDownloader sd = (ISubtitleDownloader)sdObject;
            List<Subtitle> results;
            if (it.VideoKind == VideoKind.Movie)
            {
                SearchQuery qu = new SearchQuery(it.Title);
                qu.Year = (int)it.Year;
                qu.LanguageCodes = languagePrios.Keys.ToArray();
                results = sd.SearchSubtitles(qu);
            }
            else
            {
                EpisodeSearchQuery qu = new EpisodeSearchQuery(it.Title, (int)it.Season, (int)it.Episode);
                qu.LanguageCodes = languagePrios.Keys.ToArray();
                results = sd.SearchSubtitles(qu);
            }
            Log.Debug("Subtitles found:" + results.Count.ToString());
            if (results.Count > 0)
            {
                int minValue = int.MaxValue;
                Subtitle minSub = results[0];

                foreach (Subtitle sub in results)
                {
                    Log.Debug("Subtitle " + sub.ProgramName + " " + sub.LanguageCode);
                    if (languagePrios.ContainsKey(sub.LanguageCode))
                    {
                        int prio = languagePrios[sub.LanguageCode];
                        if (prio < minValue)
                        {
                            minValue = prio;
                            minSub = sub;
                        }
                    }
                }

                List<FileInfo> subtitleFiles = sd.SaveSubtitle(minSub);
                if (subtitleFiles.Count > 0)
                {
                    // for new version after 1.3.0.0
                    /*
                    string subFile = Path.Combine(Path.GetTempPath(), "OnlineVideoSubtitles.txt");
                    File.Copy(subtitleFiles[0].FullName, subFile, true);
                    video.SubtitleUrl = Uri.UriSchemeFile + Uri.SchemeDelimiter + subFile;
                    */

                    string s = File.ReadAllText(subtitleFiles[0].FullName, System.Text.Encoding.UTF8);
                    if (s.IndexOf('�') != -1)
                        video.SubtitleText = File.ReadAllText(subtitleFiles[0].FullName, System.Text.Encoding.Default);
                    else
                        video.SubtitleText = s;
                }
                foreach (FileInfo fi in subtitleFiles)
                    fi.Delete();
            }
        }
#endif
    }
}
