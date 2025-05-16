using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Msar_Gded.Models.ViewModels;
using Msar_Gded.Models;
using Msar_Gded.Enums;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace Msar_Gded.Controllers
{

    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Route(string id)
        {
            if (id == null)
            {
                TempData["Error"] = "الرجاء إدحال الرقم القومي.";
                LogInVm vm = new LogInVm();
                return View("HomePage", vm);
            }

            if (id.Length != 14)
            {
                TempData["Error"] = "الرقم القومي يجب ان يحتوي علي 14 رقم ";
                LogInVm vm = new LogInVm();
                return View("HomePage", vm);
            }

            if (!Regex.IsMatch(id, @"^\d{14}$"))
            {
                TempData["Error"] = "الرقم القومي يجب أن يتكون أرقام فقط";
                LogInVm vm = new LogInVm();
                return View("HomePage", vm);
            }


            var student = await _context.Students.FirstOrDefaultAsync(s => s.NationalId == id);

            if (student == null)
            {
                return RedirectToAction("RequestForm", "Request", new { id = id });
            }
            return RedirectToAction("StudentProfile", new { id = id });


        }

        [AllowAnonymous]

        public async Task<IActionResult> StudentProfile(string id)
        {


            ViewBag.controllerName = "Request";
            ViewBag.entity = "طلب";
            ViewBag.theEntity = "الطلب";
            ViewBag.pluralEntity = "طلبات";
            ViewBag.thePluralEntity = "الطلبات";
            ViewBag.placeHolder = "طلبات";

            if (id == null)
            {
                return NotFound();
            }

            if (!_context.Students.Select(t => t.NationalId).Contains(id))
            {
                TempData["Error"] = "عذرا , هذا الطالب غير موجود";
                return NotFound();

                //  return RedirectToAction(nameof(Index));
            }

            try
            {
                var student = await _context.Students.
                Include(s => s.University).Include(s => s.College)
                .FirstOrDefaultAsync(s => s.NationalId == id);

                var studentRequests = await _context.Requests
                    .Where(r => r.StudentId == id)
                    .Include(r => r.Student)
                    .Include(r => r.CurrentCollege).Include(r => r.DestinationCollege)
                    .Include(r => r.CurrentUniversity).Include(r => r.DestinationUniversity)
                    .Include(r => r.Status).ToListAsync();

                var  model = new StudentProfile
                {
                    NationalId = student.NationalId,
                    Name = student.Name,
                    PhoneNumber = student.PhoneNumber,
                    BirthDate = student.BirthDate,
                    BirthPlace = student.BirthPlace,
                    GraduationDate = student.GraduationDate,
                    HighSchoolDivision = student.HighSchoolDivision,
                    EducationalZone = student.EducationalZone,
                    PureDegree = student.PureDegree,
                    DegreeWithLanguage = student.DegreeWithLanguage,
                    TotalDegree = student.TotalDegree,
                    UniversityGrade = student.UniversityGrade,
                    UniversityId = student.UniversityId,
                    CollegeId = student.CollegeId,
                    requests = studentRequests
                };

                ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name");
                ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name");

                return View(model);

            }
            catch (Exception e)
            {
                return NotFound();
            }


        }


        [AllowAnonymous]

        public IActionResult ViewFile(string nationalId, string fileType)
        {
            var student = _context.Students.FirstOrDefault(s => s.NationalId == nationalId);
            if (student == null) return NotFound();

            byte[] fileData = null;
            string fileName = "";
            string contentType = "image/jpeg"; // can adjust based on stored metadata

            if (fileType == "Certificate" && student.SchoolCertificate != null)
            {
                fileData = student.SchoolCertificate;
                fileName = "SchoolCertificate.jpg";
            }
            else if (fileType == "Status" && student.LeterOfStatus != null)
            {
                fileData = student.LeterOfStatus;
                fileName = "LetterOfStatus.jpg";
            }

            if (fileData == null) return NotFound();

            return File(fileData, contentType, fileName);
        }



    }
}
