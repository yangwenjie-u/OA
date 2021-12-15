using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class jcjtSyxm
    {
       public string xsflbh1 { get; set; } //一级编号
       public string xsflbh { get; set; } //二级编号
       public string syxmbh { get; set; } //试验项目编号
       public string syxmmc { get; set; } //试验项目名称
       public string fxbh { get; set; } //分项编号
       public string fxmc { get; set; } //分项名称
       public int sfxyfb { get; set; } //是否允许分包
       public int dcdb { get; set; } //是否人员到场对比
       public int sfyx { get; set; } //是否有效
       public int sfxcxm { get; set; } //是否现场项目
       public int xssx { get; set; } //显示顺序
       public int wtddyfs { get; set; } //委托单打印数量

       public int jgbgtxsj { get; set; } //监管报告提醒时间

       public string xmlx { get; set; } //项目类型--基础项目,1,|组合项目,2|子项目,3

       public int scbg { get; set; } //允许手动上传报告

       public int scjzqytp { get; set; } //启用见证确认

       public int sfxytjry { get; set; } //是否需要同检人员

    }
}
