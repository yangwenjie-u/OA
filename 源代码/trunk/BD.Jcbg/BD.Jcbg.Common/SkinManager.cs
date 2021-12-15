using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
	public static class SkinManager
	{
        public static string GetUserRoot()
        {
            return "/skins/default/";
        }

        public static string GetUserRoot(string filepath)
        {
            return "/skins/default/" + filepath;
        }

        public static string GetStaticCdnUrl()
        {
            if (!string.IsNullOrEmpty(Configs.StaticCdnUrl))
            {
                return Configs.StaticCdnUrl + "/skins/default/";
            }
            else
            {
                return "/skins/default/";
            }
        }

		public static string GetCssPath(string filename)
		{
            return GetStaticCdnUrl() + "css/" + RemoveChar(filename);
		}

		public static string GetImagePath(string filename)
		{
            return GetStaticCdnUrl() + "images/" + RemoveChar(filename);
		}
		public static string GetJsPath(string filename)
		{
            return GetStaticCdnUrl() + "js/" + RemoveChar(filename);
		}

        public static string GetLayuiPath(string filename)
        {
            return GetStaticCdnUrl() + "layui/" + RemoveChar(filename);
        }

        public static string GetEasyuiPath(string filename)
        {
            return GetStaticCdnUrl() + "css/easyui/" + RemoveChar(filename);
        }

        public static string GetBootstrapPath(string filename)
        {
            return GetStaticCdnUrl() + "bootstrap/" + RemoveChar(filename);
        }

		public static string GetMy97JsPath(string filename)
        {
            return GetUserRoot() + "my97datepicker/" + filename;
        }

		public static string GetCalendarCssPath(string filename)
		{
            return GetUserRoot() + "calendar/css/" + filename;
		}

		public static string GetCalendarJsPath(string filename)
		{
            return GetUserRoot() + "calendar/js/" + filename;
		}

		public static string GetCkEditroPath(string filename)
		{
            return GetUserRoot() + "ckeditor/" + filename;
		}

		public static string GetFileImagePath(string filename)
		{
            return GetUserRoot() + "images/fileimage16/" + filename;
		}

        public static string GetCheckTreePath(string filename)
        {
            return GetUserRoot() + "checktree/" + filename;
        }

        public static string GetTreeTablePath(string filename)
        {
            return GetUserRoot() + "treetable/" + filename;
        }

        public static string GetJcropPath(string filename)
        {
            return GetUserRoot() + "tapmodo-jcrop/" + filename;
        }

        public static string GetDatePath(string filename)
        {
            return GetUserRoot() + "date/" + filename;
        }

        public static string GetPdfViewPath(string filename)
        {
            return GetUserRoot() + "pdfjs/" + filename;
        }

        public static string GetAudioJsPath(string filename)
        {
            return GetUserRoot() + "audiojs/" + filename;
        }

        public static string GetPlayerPath(string filename)
        {
            return GetUserRoot() + "player/" + filename;
        }

        public static string GetHkwsPath(string filename)
        {
            return GetUserRoot() + "ckplayer/" + filename;
        }
		public static string GetWelcomezs(string filename)
        {
            return GetUserRoot() + "welcomezs/" + filename;
        }
        public static string GetJqueryMobilePath(string filename)
        {
            return GetUserRoot() + "jquery-mobile/" + filename;
        }

        public static string GetXwwzPath(string filename)
        {
            return GetUserRoot() + "xwwz/" + filename;
        }
        public static string GetXwwzHTPath(string filename)
        {
            return GetUserRoot() + "xwwzht/" + filename;
        }

        public static string RemoveChar(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (fileName.Substring(0, 1) == @"/")
                {
                    fileName = fileName.Substring(1, fileName.Length - 1);
                }

                return fileName.ToLower();
            }

            return "";
        }
	}
}
