using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppMiniProject.Models;
using FluentNHibernate.Mapping;

namespace ContactAppMiniProject.Mappings
{
    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {

            Table("Contacts");
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.FirstName);
            Map(c => c.LastName);
            Map(c => c.IsActive);           
            HasMany(c => c.Details).Inverse().Cascade.All().Not.LazyLoad();
            References(c=>c.User).Column("UserId").Nullable().Cascade.None();

        }
    }
}