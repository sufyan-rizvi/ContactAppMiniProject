using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppMiniProject.Models;
using FluentNHibernate.Mapping;

namespace ContactAppMiniProject.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(u => u.Id).GeneratedBy.GuidComb();
            Map(u => u.UserName);
            Map(u => u.Password);
            Map(u => u.IsActive);
            Map(u => u.IsAdmin);
            HasMany(u => u.Contacts).Inverse().Cascade.All();
            HasOne(u => u.Role).PropertyRef(u => u.User).Cascade.All();
        }
    }
}