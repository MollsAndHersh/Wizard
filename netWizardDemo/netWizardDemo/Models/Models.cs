using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace netWizardDemo.Models
{
    public class MUser
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        [Display(Name ="First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        public string EMail { get; set; }

        public virtual List<MAddress> Addresses { get; set; } = new List<MAddress>();
        public virtual List<MApplication> Applications { get; set; } = new List<MApplication>();
    }
    public class MApplication
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual List<MUser> Users { get; set; } = new List<MUser>();
    }
    public class MAddress
    {
        public int Id { get; set; }
        [Display(Name = "Street Address")]
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        [Required]
        public string Zip { get; set; }

        public virtual List<MUser> Users { get; set; } = new List<MUser>();
    }
}