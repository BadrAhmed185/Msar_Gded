using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Msar_Gded.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string NationalId { get; set; }
        public string FullName { get; set; }


        //[Display(Name = "القسم")]
        //public string? Department { get; set; }

        //[Display(Name = "العنوان")]
       // public string? Address { get; set; }

        [Display(Name = "الدور")]
        public string? Role { get; set; }


    }
}
