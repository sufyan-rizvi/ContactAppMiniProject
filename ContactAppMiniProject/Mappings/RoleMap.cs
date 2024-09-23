using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppMiniProject.Models;
using FluentNHibernate.Mapping;

namespace ContactAppMiniProject.Mappings
{
    public class RoleMap:ClassMap<Role>
    {
        public RoleMap()
        {
            Table("Roles");
            Id(u => u.Id).GeneratedBy.GuidComb();
            Map(u => u.RoleName);
            References(u=>u.User).Column("UserId").Unique().Cascade.None();
        }
    }
}