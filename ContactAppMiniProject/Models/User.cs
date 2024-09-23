using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace ContactAppMiniProject.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public virtual string UserName { get; set; }
        [Required]
        public virtual string Password { get; set; }
                
        [Display(Name = "Admin Status")]
        public virtual bool IsAdmin { get; set; }
        
        [Display(Name = "Active Status")]
        public virtual bool IsActive { get; set; }
        public virtual IList<Contact> Contacts { get; set; }
        public virtual Role Role { get; set; }

    }
}