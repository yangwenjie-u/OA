using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.JC.JS.Common.Entities
{
    /// <summary>
    /// zdzd集合
    /// </summary>
    public class CollectionZdzd
    {
        public IList<EntityZdzd> Zdzds = new List<EntityZdzd>();

        public void Add(EntityZdzd zdzd){
            Zdzds.Add(zdzd);
        }

        public void AddRange(IList<EntityZdzd> zdzds)
        {
            ((List<EntityZdzd>)Zdzds).AddRange(zdzds);
        }

        

        
    }
}
