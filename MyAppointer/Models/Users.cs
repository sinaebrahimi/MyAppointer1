//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAppointer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc; 
    
    public partial class Users
    {
        public Users()
        {
            this.Appointments = new HashSet<Appointments>();
            this.JobOwners = new HashSet<JobOwners>();
            this.Jobs = new HashSet<Jobs>();
        }
    
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(32)]
        [DataType(DataType.EmailAddress)]
        [Remote("doesUserNameExist", "User", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile")]
        [Required]
        [StringLength(11)]
        [Remote("doesPhoneNumberExist", "User", HttpMethod = "POST", ErrorMessage = "Phone Number already exists. Please enter a different phne number.")]
        //^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$
        [RegularExpression(@"^09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{7}$", ErrorMessage = "Enter Your Mobile Phone Number like this: 09127404062")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Your First Name and Last Name")]
        public string FullName { get; set; }
        public string About { get; set; }
        public string City { get; set; }
        public string Role { get; set; }
    
        public virtual ICollection<Appointments> Appointments { get; set; }
        public virtual ICollection<JobOwners> JobOwners { get; set; }
        public virtual ICollection<Jobs> Jobs { get; set; }
    }
}
