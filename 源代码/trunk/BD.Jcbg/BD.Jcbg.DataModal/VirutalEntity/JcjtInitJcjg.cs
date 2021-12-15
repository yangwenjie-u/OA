using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class JcjtInitJcjg
    {
        //检测机构编号
        public string qybh { get; set; }
        //检测中心名称
        public string qymc { get; set; }
        //社会统一信用代码，无则填组织机构代码
        public string zzjgdm { get; set; }
        //检测中心地址
        public string qydd { get; set; }
        //检测中心电话
        public string lxdh { get; set; }
        //检测中心邮编
        public string zcdyb { get; set; }
        //检测中心负责人
        public string qyfzr { get; set; }
        //负责人手机
        public string lxsj { get; set; }
        //机构缩写
        public string jgsx { get; set; }
        //委托单编号模式
        public string wtdbhms { get; set; }
        //登记单编号模式
        public string djdbhms { get; set; }
        //试验编号模式
        public string sybhms { get; set; }
        //试验报告编号模式
        public string sybgbhms { get; set; }
        //委托单打印数量
        public string wtddysl { get; set; }
        //试验报告打印数量
        public string sybgdysl { get; set; }

    }

}
