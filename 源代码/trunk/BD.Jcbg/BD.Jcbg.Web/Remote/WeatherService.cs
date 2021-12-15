using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.Remote
{
	public static class WeatherService
	{
		#region 天气预报
		private static Dictionary<DateTime, string> _weathers = new Dictionary<DateTime, string>();
        
		public static string GetTemperatureForecast()
		{
			string ret = "";
			DateTime dt = DateTime.Now.Date;
			if (_weathers.TryGetValue(dt, out ret))
			{
				if (ret != null && ret.Length > 0)
					return ret;
			}
			try
			{
                string url = Configs.GetConfigItem("weatherurl");
                string city = Configs.GetConfigItem("weathercity");
				if (url.Length > 0 && city.Length > 0)
				{
					EndpointAddress epAddress = new EndpointAddress(url);
					WSHttpBinding bind = new WSHttpBinding();
					bind.Security.Mode = SecurityMode.None;
					bind.MaxBufferPoolSize = 100000000;
					WCFWeatherServiceClient client = new WCFWeatherServiceClient(bind, epAddress);
					CS.DataModal.Entities.Weather weather = client.GetCityWeather(city);

					client.Close();
					ret = "今天：" + weather._weather1 + " " + weather._temp12 + "～" + weather._temp11 + "℃ " + weather._wind1
						+ " 明天：" + weather._weather2 + "，" + weather._temp22 + "～" + weather._temp21 + "℃，" + weather._wind2;
					_weathers.Add(dt, ret);
				}

			}
			catch (Exception e)
			{
                SysLog4.WriteError(e.Message);
			}
			return ret;

		}

        #endregion

        #region 等效龄期
        public static string Get600days()
        {
            string ret = "";
            try
            {
                string url = Configs.GetConfigItem("dayurl600");
                string cityid = Configs.GetConfigItem("daycity600");
                string format = Configs.GetConfigItem("dayformat600");
                if (cityid != "" && format != "" && url != "")
                {
                    EndpointAddress epAddress = new EndpointAddress(url);
                    WSHttpBinding bind = new WSHttpBinding();
                    bind.Security.Mode = SecurityMode.None;
                    bind.MaxBufferPoolSize = 100000000;
                    WCFWeatherServiceClient client = new WCFWeatherServiceClient(bind, epAddress);
                    ret = client.Get600CDay(cityid, format);
                    client.Close();
                }



            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ret;

        }
        #endregion


    }
}