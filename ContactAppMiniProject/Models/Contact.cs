using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAppMiniProject.Models
{
    public class Contact
    {
        public virtual Guid Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }


        [Display(Name = "Active Status")]
        public virtual bool IsActive { get; set; }
        public virtual IList<ContactDetail> Details { get; set; } = new List<ContactDetail>();
        public virtual User User { get; set; }

    }
}