using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppMiniProject.Models;
using FluentNHibernate.Mapping;

namespace ContactAppMiniProject.Mappings
{
    public class DetailMap:ClassMap<ContactDetail>
    {
        public DetailMap()
        {
            Table("Details");
            Id(d => d.Id).GeneratedBy.GuidComb();
            Map(d => d.PhoneNumber);
            Map(d=>d.Email);
            References(d => d.Contact).Column("ContactId").Nullable().Cascade.None();
        }
    }
}