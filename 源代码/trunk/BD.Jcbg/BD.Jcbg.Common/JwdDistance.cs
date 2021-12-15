using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class JwdDistance
    {
        private const double EARTH_RADIUS = 6378.137;//地球半径,单位千米
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// 根据经纬度计算距离，用于判断试验地点和工程地点距离，返回单位千米
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="lng1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="lng2">经度2</param>
        /// <returns></returns>
        public static double GetDistance2(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }


        private static double OD(double a, double b, double c)
        {
            while (a > c) a -= c - b;
            while (a < b) a += c - b;
            return a;
        }
        private static double SD(double a, double b, double c)
        {
            if(b != null){
              a = Math.Max(a, b);
            };
            if(c != null)
            {
              a = Math.Min(a, c);
            }
            return a;
        }
        public static double GetDistance(double a_lat, double a_lng, double b_lat, double b_lng)
        {
            var a = Math.PI * OD(a_lat, -180, 180) / 180;
            var b = Math.PI * OD(b_lat, -180, 180) / 180;
            var c = Math.PI * SD(a_lng, -74, 74) / 180;
            var d = Math.PI * SD(b_lng, -74, 74) / 180;
            return 6370996.81 * Math.Acos(Math.Sin(c) * Math.Sin(d) + Math.Cos(c) * Math.Cos(d) * Math.Cos(b-a));
        }

        //使用并保留小数点后两位
        //var m =getDistance(106.486654,29.490295,106.581515,29.615467).toFixed(2);
        //获取到的单位是 米


    }
}
