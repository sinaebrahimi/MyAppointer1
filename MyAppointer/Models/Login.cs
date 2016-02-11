using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAppointer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc; 

    public class Login
    {
    
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(32)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
