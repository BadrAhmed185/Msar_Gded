using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Msar_Gded.Enums;

namespace Msar_Gded.Models
{
    public class Student
    {
        [Key]
        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [Display(Name = "الرقم القومي")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب ألا يزيد عن 100 حرف")]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(11, ErrorMessage = ".رقم الهاتف يجب أن يكون 11 رقم")]
        [RegularExpression("^0\\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يكون 11 رقمًا ويبدأ بصفر")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الميلاد")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "مكان الميلاد مطلوب")]
        [Display(Name = "جهه الميلاد")]
        public string BirthPlace { get; set; }


        [Required(ErrorMessage = " تاريخ التخرج مطلوب")]
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ التخرج")]
        public DateTime GraduationDate { get; set; }


        [Required(ErrorMessage = "شعبة الثانوية مطلوبة")]
        [Display(Name = "شعبة الثانوية")]
        public HighSchoolDivision HighSchoolDivision { get; set; } = HighSchoolDivision.Literary;

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


        [Required(ErrorMessage = "الرجاء إختيار الصف الجامعي")]
        [Display(Name = "الفرقة المقيد بها")]
        public string UniversityGrade { get; set; }

        [Display(Name = "شهادة الثانوية")]
        public byte[] SchoolCertificate { get; set; }

        [NotMapped]
        [Display(Name = "رفع شهادة الثانوية")]
        public IFormFile? SchoolCertificateUpload { get; set; }

        [Display(Name = "بيان حالة مختوم.")]
        public byte[] LeterOfStatus { get; set; }

        [NotMapped]
        // [Display(Name = "رفع خطاب الحالة")]
        public IFormFile? LeterOfStatusUpload { get; set; }

        [Required(ErrorMessage = "الجامعة مطلوبة")]
        [Display(Name = "الجامعة")]
        public int UniversityId { get; set; }

        [Required(ErrorMessage = "الكلية مطلوبة")]
        [Display(Name = "الكلية")]
        public int CollegeId { get; set; }

        // Navigation

        [ForeignKey("UniversityId")]
        public University? University { get; set; }


        [ForeignKey("CollegeId")]
        public College? College { get; set; }
        public ICollection<Request>? Requests { get; set; }
    }
}
