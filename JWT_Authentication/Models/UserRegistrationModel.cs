using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Authentication.Models
{
    public class UserRegistrationModel
    {
        //[Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        //[EmailAddress]
       // [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
    public class StudentModel
    {
        [Key]
        public string id { get; set; }

        public string name { get; set; }
        public string rollno { get; set; }
        public string standard { get; set; }
        public string address { get; set; }
        public string phonenumber { get; set; }
        public string gender { get; set; }
        public string city { get; set; }

    }
}
