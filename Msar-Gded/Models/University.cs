using System.ComponentModel.DataAnnotations;

namespace Msar_Gded.Models
{
    public class University
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الجامعة مطلوب")]
        [Display(Name = "اسم الجامعة")]
        public string Name { get; set; }

        // Navigation
        public ICollection<Student>? Students { get; set; }
        public ICollection<Request>? RequestsAsCurrent { get; set; }
        public ICollection<Request>? RequestsAsDestination { get; set; }
    }

}
