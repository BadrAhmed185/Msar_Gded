using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Msar_Gded.Models.ViewModels
{
    public class LogInVm
    {

        public LogInVm() { }

       // [Required(ErrorMessage ="HbalFeElGabal")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

      //  [Required(ErrorMessage = "HbalFeElGabal")]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }



        //[Display(Name = "تذكرني")]
        //public bool RememberMe { get; set; }
    }
}
