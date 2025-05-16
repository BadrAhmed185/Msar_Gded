using System.ComponentModel.DataAnnotations;

namespace Msar_Gded.Enums
{
    public enum HighSchoolDivision
    {
        [Display(Name = "علمي علوم")]
        ScienceBiology = 1,

        [Display(Name = "علمي رياضة")]
        ScienceMath = 2,

        [Display(Name = "أدبي")]
        Literary = 3
    }
}

