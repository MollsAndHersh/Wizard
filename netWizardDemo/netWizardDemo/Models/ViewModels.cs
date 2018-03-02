using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace netWizardDemo.Models
{
    public class VMNewUser
    {
        public string Username { get; set; }

        [Display(Name ="First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        public string EMail { get; set; }

        public List<MAddress> Addresses { get; set; } = new List<MAddress>();
        public List<VMApplicationWizard> Applications { get; set; } = new List<VMApplicationWizard>();
    }
    public class VMApplicationWizard
    {
        public int ApplicationId { get; set; }
    }
}