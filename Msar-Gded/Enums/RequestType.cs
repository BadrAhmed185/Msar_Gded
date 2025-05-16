using System.ComponentModel.DataAnnotations;

namespace Msar_Gded.Enums
{
    public enum RequestType
    {
        [Display(Name = "تحويلي")]
        Transfer = 1,

        [Display(Name = "نقل قيدي")]
        MoveRecord = 2,

        [Display(Name = "تعديل ترشيحي")]
        EditNomination = 3
    }

}
