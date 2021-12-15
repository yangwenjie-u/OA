using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class numt2string
    {
        /// <summary>
        /// 数字转成大写
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string num2String(double num)
        {
            if (num >= 1000000000)
            {
                Console.WriteLine("num is too large");
                return "";
            }
            string result = "";
            string front = "";//整数部分 
            string back = "";//小数部分 
            string[] num_strs = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };//大写数字数组 
            string[] num_dw = { "", "拾", "佰", "仟", "万", "拾万", "佰万", "仟万", "亿" };//大写数字单位数组 
            string[] money_dw = { "分", "角", "圆" };//人民币单位数组 
            string str_num = num.ToString();
            string[] strs = str_num.Split('.');
            if (num < 0)//负数的话 
            {
                result += "负";
                str_num = str_num.Replace("-", "");
            }

            int num_f = 0;//整数部分 
            int num_back = 0;//小数部分 

            if (strs.Length == 2)
            {
                front = strs[0];
                back = strs[1];

                num_f = Convert.ToInt32(front);
                num_back = Convert.ToInt32(back);


            }
            else
            {
                front = num.ToString();
                num_f = Convert.ToInt32(front);
            }

            for (int i = 8; i >= 0; i--)//从八个0 就是亿开始 
            {
                string cs = "1";//除数 
                for (int j = 1; j <= i; j++)//除数补零 
                {
                    cs += "0";
                }
                int num_cs = Convert.ToInt32(cs);
                int s = num_f / num_cs;//商 
                if (s == 0)//商为0意味着除数没有这么大 直接跳到下一次循环 
                {
                    continue;
                }
                else
                {
                    result += num_strs[s] + num_dw[i];//针对这一位生成结果 
                    num_f = num_f % num_cs;//整数部分重新赋值成余数继续循环 
                }
            }
            result += money_dw[2];//整数位添加货币单位 
            //以下针对结果进行处理使之合理化 
            //循环加零 如2003 不处理将是两千三圆 处理后为两千零三圆 
            //算法为在数字单位数组中从佰开始遍历（第三位）找结果字符串中是否含有它的上一位 如没有则需在此单位的下一位置加入一个“零” 
            for (int i = 2; i < num_dw.Length; i++)
            {
                if (result.IndexOf(num_dw[i]) == -1)
                {
                    continue;
                }

                if (result.IndexOf(num_dw[i - 1]) == -1)
                {
                    result = result.Insert(result.IndexOf(num_dw[i]) + 1, "零");
                }
            }
            //以下处理多出的“万” 如出现二十万两万。。。应为二十二万 
            //算法为保留最后出现的万字 其他去掉 
            string[] strs1 = result.Split('万');
            result = "";
            //以万字拆分字符串后 遍历结果数组 在结果数组的前一位加上万字 
            for (int i = 0; i < strs1.Length; i++)
            {
                result += strs1[i];
                if (i == strs1.Length - 2)
                {
                    result += "万";
                }

            }
            //以下处理录入0时 直接生成零圆 
            if (result == money_dw[2])
            {
                result = "零" + money_dw[2];
            }

            //小数部分处理 
            if (back != null && back != "")
            {
                if (back.Length > 2)//只截取前两位 到分 在小的无货币单位支持 没有意义 
                {
                    back = back.Substring(0, 2);
                }
                num_back = Convert.ToInt32(back);

                int s = num_back / 10;
                int ys = num_back % 10;
                if (s == 0)//只有角一位 
                {
                    result += (num_strs[ys] + money_dw[1]);
                }
                else
                {
                    result += (num_strs[s] + money_dw[1] + num_strs[ys] + money_dw[0]);
                }
            }

            return result;
        }

    }
}
