using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.JC.JS.Common
{
    /// <summary>
    /// 产生随机数
    /// </summary>
    public static class RandomNumber
    {
        private static char[] constant1 =
        {   
            '0','1','2','3','4','5','6','7','8','9'
        };
        private static char[] constant2 =
        {   
            '0','1','2','3','4','5','6','7','8','9',  
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'  
        };
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="constanttype">1:0到9的随机数，2:0到9,a到z,A到Z随机数</param>
        /// <param name="length">随机数位数</param>
        /// <returns></returns>
        public static string GetNew(RandomType constanttype, int length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                if (constanttype == RandomType.Number)
                    newRandom.Append(constant1[rd.Next(constant1.Length)]);
                else if (constanttype == RandomType.NumberAndChar)
                    newRandom.Append(constant2[rd.Next(constant2.Length)]);
            }
            return newRandom.ToString();

        }
    }

    public enum RandomType
    {
        Number,         // 数字
        NumberAndChar   // 数字和大小写字母
    }
}
