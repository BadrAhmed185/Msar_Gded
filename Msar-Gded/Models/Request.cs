using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Msar_Gded.Enums;

namespace Msar_Gded.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "تاريخ الطلب مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ مل~ الإستمارة")]
        public DateTime DateOfRequest { get; set; }


        [Required(ErrorMessage = "الرجاء تحديد نوع الطلب")]

        public RequestType TypeOfRequest { get; set; } = RequestType.Transfer;



        [Required(ErrorMessage = "الجامعة الحالية مطلوبة")]
        [Display(Name = "الجامعة الحالية")]
        public int? CurrentUniversityId { get; set; }

        [Required(ErrorMessage = "الكلية الحالية مطلوبة")]
        [Display(Name = "الكلية الحالية")]
        public int? CurrentCollegeId { get; set; }

        [Required(ErrorMessage = "الجامعة المراد التحويل إليها مطلوبة")]
        [Display(Name = "الجامعة المراد التحويل إليها")]
        public int DestinationUniversityId { get; set; }

        [Required(ErrorMessage = "الكلية المراد التحويل إليها مطلوبة")]
        [Display(Name = "الكلية المراد التحويل إليها")]
        public int DestinationCollegeId { get; set; }


        [Display(Name = "كود الدفع")]
        public int? PaymentCode { get; set; }

        [Display(Name = "يحتاج مراجعة")]
        public bool? NeedRivision { get; set; }

        [Display(Name = "الحالة")]
        public int StatusId { get; set; } = 1;

        public string StudentId { get; set; }


        // Navigation
        [ForeignKey("StatusId")]
        public Status? Status { get; set; }

        [ForeignKey("CurrentUniversityId")]
        public University? CurrentUniversity { get; set; }


        [ForeignKey("CurrentCollegeId")]
        public College? CurrentCollege { get; set; }

        [ForeignKey("DestinationUniversityId")]
        public University? DestinationUniversity { get; set; }
        [ForeignKey("DestinationCollegeId")]
        public College? DestinationCollege { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
    }


}
