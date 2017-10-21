using Blank.NancyCore.Abstract.DataModels;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blank.NancyCore.Impl.DapperMaps
{
    class MenuMap : EntityMap<Menu>
    {
        public MenuMap()
        {
            Map(t => t.Id).ToColumn("id");
            Map(t => t.ParentId).ToColumn("parent_id");
            Map(t => t.Name).ToColumn("name");
            Map(t => t.Url).ToColumn("url");
            Map(t => t.Class).ToColumn("class");
        }
    }
}
