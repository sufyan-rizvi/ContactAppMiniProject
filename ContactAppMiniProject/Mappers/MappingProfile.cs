using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ContactAppMiniProject.DTOs;
using ContactAppMiniProject.Models;
using FluentNHibernate.Mapping;

namespace ContactAppMiniProject.Mappers
{
    public class MappingProfile : Profile
    {
        public override string ProfileName => base.ProfileName;

        protected override void Configure()
        {
            CreateMap<Contact, ContactDTO>();
        }
    }
}