using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAppMiniProject.Models
{
    public class ContactDetail
    {
        public virtual Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public virtual string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10)]
        public virtual string PhoneNumber { get; set; }

        public virtual Contact Contact { get; set; }




    }
}