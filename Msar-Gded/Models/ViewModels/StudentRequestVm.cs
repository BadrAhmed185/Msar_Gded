using Msar_Gded.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Msar_Gded.Models.ViewModels
{
    public class StudentRequestVm
    {
        // Student fields
        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [Display(Name = "الرقم القومي")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب ألا يزيد عن 100 حرف")]
        [MinLength(3 , ErrorMessage = "الرجاء إدخال الإسم الثلاثي")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(11, ErrorMessage = ".رقم الهاتف يجب أن يكون 11 رقم")]
        [RegularExpression("^0\\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يكون 11 رقمًا ويبدأ بصفر")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [PastDate(ErrorMessage = "يجب أن يكون تاريخ الميلاد من الماضي.")]

        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الميلاد")]
        public DateTime BirthDate { get; set; }= DateTime.Now;


        [Required(ErrorMessage = "مكان الميلاد مطلوب")]
        [Display(Name = "جهه الميلاد")]
        public string BirthPlace { get; set; }


        [Required(ErrorMessage = " تاريخ التخرج مطلوب")]
        [PastDate(ErrorMessage = "يجب أن يكون تاريخ التخرج من الماضي.")]

        [DataType(DataType.Date)]
        [Display(Name = "تاريخ التخرج")]
        public DateTime GraduationDate { get; set; } = DateTime.Now;




        [Required(ErrorMessage = "شعبة الثانوية مطلوبة")]
        [Display(Name = "شعبة الثانوية")]
        public HighSchoolDivision HighSchoolDivision { get; set; } /*= HighSchoolDivision.Literary;*/

        [Required(ErrorMessage = "المنطقة التعليمية مطلوبة")]
        [Display(Name = "المنطقة التعليمية")]
        public string EducationalZone { get; set; }

        [Required(ErrorMessage = "مجموع الدرجات بدون المستوي مطلوب")]
        // [Range(0, 410, ErrorMessage = "الدرجة يجب أن تكون بين 0 و 410")]
        [Display(Name = "مجموع الدرجات بدون المستوي")]
        public double PureDegree { get; set; }

        [Required(ErrorMessage = "درحة المستوي الرفيع مطلوبة ")]
        //  [Range(0, 410, ErrorMessage = "الدرجة يجب أن تكون بين 0 و 410")]
        [Display(Name = "درحة المستوي الرفيع")]
        public double DegreeWithLanguage { get; set; }

        [Required(ErrorMessage = "المجموع الكلي مطلوب")]
        [Range(0, 410, ErrorMessage = "المجموع يجب أن يكون بين 0 و 410")]
        [Display(Name = "المجموع الكلي بدرجات المستوي الرفيع")]
        public double TotalDegree { get; set; }


        [Display(Name = "شهادة الثانوية")]
        public byte[]? SchoolCertificate { get; set; }


       // [Required(ErrorMessage = "الرجاء رفع شهاده الثانوية العامه")]
        [Display(Name = "رفع شهادة الثانوية")]
        public IFormFile? SchoolCertificateUpload { get; set; }

        [Display(Name = "بيان حالة مختوم.")]
        public byte[]? LeterOfStatus { get; set; }

       // [Required(ErrorMessage = "الرجاء رفع بيان حاله مختوم")]
        // [Display(Name = "رفع خطاب الحالة")]
        public IFormFile? LeterOfStatusUpload { get; set; }

        [Required(ErrorMessage = "الجامعة مطلوبة")]
        [Display(Name = "الجامعة")]
        public int UniversityId { get; set; }

        [Required(ErrorMessage = "الكلية مطلوبة")]
        [Display(Name = "الكلية")]
        public int CollegeId { get; set; }























        // Request fields
        public int? requestId { get; set; } // for edit  , review, and delete

        public int statusId { get; set; } = 1;

        [Required(ErrorMessage = "تاريخ الطلب مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ مل~ الإستمارة")]
        public DateTime DateOfRequest { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "الرجاء تحديد نوع الطلب")]
        public RequestType TypeOfRequest { get; set; }



        [Display(Name = "الجامعة الحالية")]
        public int? CurrentUniversityId { get; set; }


        [Display(Name = "الكلية الحالية")]
        public int? CurrentCollegeId { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار الفرقة الحالية المقيد بها")]
        [Display(Name = "الفرقة المقيد بها")]
        public string UniversityGrade { get; set; }

        [Required(ErrorMessage = "الجامعة المراد التحويل إليها مطلوبة")]
        [Display(Name = "الجامعة المراد التحويل إليها")]
        public int DestinationUniversityId { get; set; }

        [Required(ErrorMessage = "الكلية المراد التحويل إليها مطلوبة")]
        [Display(Name = "الكلية المراد التحويل إليها")]
        public int DestinationCollegeId { get; set; }
    }


    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date && date > DateTime.Now)
            {
                // Custom error message
                return new ValidationResult(ErrorMessage ?? "يجب أن يكون تاريخ التخرج من الماضي.");
            }
            return ValidationResult.Success;
        }
    }


}
