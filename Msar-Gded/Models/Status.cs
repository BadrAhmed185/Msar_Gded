using System.ComponentModel.DataAnnotations;

namespace Msar_Gded.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الحالة مطلوب")]
        [Display(Name = "اسم الحالة")]
        public string Name { get; set; }

        // Navigation
        public ICollection<Request>? Requests { get; set; }
    }

}
