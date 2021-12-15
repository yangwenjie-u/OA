using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity.Jc
{
    /// <summary>
    /// 接口对象中品铭项目类
    /// </summary>
    public class InterfaceXspmProject
    {
        //总数
        public int total;
        //信息
        public string msg;
        //记录
        public IList<XspmProject> rows;
    }

    public class XspmProject
    {
        //项目名
        public string ProjectName;

        public string ProjectCategory;

        public string RegNumber;

        public string Coordinate;

        public string BuildingArea;

        public string Supervisor;

        public string BuildManager;

        public string AddUserName;

        public string AddTrueName;

        public string AddDate;

        public IList<XspmUnit> Units;
    }

    /// <summary>
    /// 企业信息
    /// </summary>
    public class XspmUnit
    {
        public string UnitCode;

        public string UnitName;

        public string LegalName;

        public string LegalPhone;

        public int UnitType;

        //自定义字段
        //企业编号
        public string qybh;
    }
}
