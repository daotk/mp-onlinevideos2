﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OnlineVideos;
using OnlineVideos.Sites;
using System.Windows;
using System.Net;
using System.Globalization;
using System.Windows.Controls;

namespace Standalone
{
    [ValueConversion(typeof(object), typeof(ImageSource))]
    public class ThumbnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string file = string.Empty;
			if (value is string && !string.IsNullOrEmpty((string)value))
			{
				try { if (System.IO.Path.IsPathRooted(value as string) && System.IO.File.Exists(value as string)) file = value as string; }
				catch { }
			}
			else
			{
				SiteUtilBase site = value as SiteUtilBase;
				if (site == null)
				{
					var siteViewModel = value as ViewModels.Site;
					if (siteViewModel != null) site = siteViewModel.Model;
				}
				if (site != null)
				{
					string subDir = string.IsNullOrEmpty(parameter as string) ? "Icons" : parameter as string;
					// use Icon with the same name as the Site
					string image = System.IO.Path.Combine(OnlineVideoSettings.Instance.ThumbsDir, subDir + @"/" + site.Settings.Name + ".png");
					if (System.IO.File.Exists(image)) file = image;
					else
					{
						// if that does not exist, try icon with the same name as the Util
						image = System.IO.Path.Combine(OnlineVideoSettings.Instance.ThumbsDir, subDir + @"/" + site.Settings.UtilName + ".png");
						if (System.IO.File.Exists(image)) file = image;
					}
				}
			}

            if (string.IsNullOrEmpty(file)) return null;

            // load the image, specify CacheOption so the file is not locked
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(file);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                if (value == null || !string.IsNullOrEmpty(value as string)) return Visibility.Hidden;
                else return Visibility.Visible;
            }
            else
            {
                if (value == null || !string.IsNullOrEmpty(value as string)) return Visibility.Visible;
                else return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    public class ZeroVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value is int) && (int)value > 0) return Visibility.Visible;
            else return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            string lang = value.ToString();
            string filename = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"LanguageFlags\" + lang + ".png");
            if (System.IO.File.Exists(filename)) return filename;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Category), typeof(string))]
    public class CategoryPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "";
            Category c = value as Category;
            while (c != null)
            {
                result = c.Name + (result == "" ? "" : " / ") + result;
                c = c.ParentCategory;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(PlayListItem), typeof(string))]
    public class PlayListPositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "";
            PlayListItem pl = value as PlayListItem;
            PlayList pList = (App.Current.MainWindow as OnlineVideosMainWindow).CurrentPlayList;
            if (pList != null && pList.Count > 1)
            {
                int index = pList.IndexOf(pl);
                if (index > -1) result = string.Format("{0} / {1}", index+1, pList.Count);
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string), typeof(string))]
    public class LanguageCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = value as string;
            try
            {
                name = name != "--" ? System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag(name).DisplayName : "Global";
            }
            catch
            {
                var temp = System.Globalization.CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(
                    ci => ci.IetfLanguageTag == name || ci.ThreeLetterISOLanguageName == name || ci.TwoLetterISOLanguageName == name || ci.ThreeLetterWindowsLanguageName == name);
                if (temp != null)
                {
                    name = temp.DisplayName;
                }
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

	[ValueConversion(typeof(Translation), typeof(string))]
	public class TranslationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Translation.Instance.GetByName(parameter as string);
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

    [ValueConversion(typeof(string), typeof(string))]
    public class EmailToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string email = value as string;
            return email.Substring(0, email.IndexOf('@'));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(OnlineVideos.OnlineVideosWebservice.SiteState), typeof(SolidColorBrush))]
    public class SiteStateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((OnlineVideos.OnlineVideosWebservice.SiteState)value)
            {
                case OnlineVideos.OnlineVideosWebservice.SiteState.Reported: return new SolidColorBrush(Color.FromArgb(255, 255, 240, 79));
                case OnlineVideos.OnlineVideosWebservice.SiteState.Broken: return new SolidColorBrush(Colors.Red);
                default: return new SolidColorBrush(Colors.Green);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(ListViewItem), typeof(Visibility))]
    public class SiteVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListViewItem lvItem  = (value as ListViewItem);
            OnlineVideos.OnlineVideosWebservice.Site onlineSite = lvItem.DataContext as OnlineVideos.OnlineVideosWebservice.Site;
            SiteSettings ss = OnlineVideoSettings.Instance.SiteSettingsList.FirstOrDefault(i => i.Name == onlineSite.Name);
            if ((parameter as string) == "Add")
            {
                if (ss != null) return Visibility.Hidden;
                else return Visibility.Visible;
            }
            else
            {
                if (ss != null) return Visibility.Visible;
                else return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(long), typeof(DateTime))]
    public class LongToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new DateTime((long)value).ToString((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(float), typeof(string))]
    public class BufferPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // format the percentage nicely depeding on range
            float percent = (float)value;
            string formatString = "###";
            if (percent == 0f) return ""; //formatString = "0.0";
            else if (percent < 1f) formatString = ".00";
            else if (percent < 10f) formatString = "0.0";
            else if (percent < 100f) formatString = "##";
            return string.Format("{0}: {1} %", Translation.Instance.Buffered, percent.ToString(formatString, System.Globalization.CultureInfo.InvariantCulture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
