using Msar_Gded.Enums;
using System.ComponentModel.DataAnnotations;

namespace Msar_Gded.Models.ViewModels
{
    public class StudentProfile
    {


        [Display(Name = "الرقم القومي")]
        public string NationalId { get; set; }


        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        [PastDate(ErrorMessage = "يجب أن يكون تاريخ الميلاد من الماضي.")]

        [Display(Name = "تاريخ الميلاد")]
        public DateTime BirthDate { get; set; } = DateTime.Now;


 
        [Display(Name = "جهه الميلاد")]
        public string BirthPlace { get; set; }



        [DataType(DataType.Date)]
        [Display(Name = "تاريخ التخرج")]
        public DateTime GraduationDate { get; set; } = DateTime.Now;




   
        [Display(Name = "شعبة الثانوية")]
        public HighSchoolDivision HighSchoolDivision { get; set; } /*= HighSchoolDivision.Literary;*/

        [Display(Name = "المنطقة التعليمية")]
        public string EducationalZone { get; set; }


        // [Range(0, 410, ErrorMessage = "الدرجة يجب أن تكون بين 0 و 410")]
        [Display(Name = "مجموع الدرجات بدون المستوي")]
        public double PureDegree { get; set; }

        [Display(Name = "درحة المستوي الرفيع")]
        public double DegreeWithLanguage { get; set; }


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

        [Display(Name = "الجامعة")]
        public int UniversityId { get; set; }

        [Display(Name = "الكلية")]
        public int CollegeId { get; set; }

        [Display(Name = "الصف الجامعي")]
        public string UniversityGrade { get; set; }


        public List<Request> requests;
    }
}
