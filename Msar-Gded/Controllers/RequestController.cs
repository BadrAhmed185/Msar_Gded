using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Msar_Gded.Models;
using Msar_Gded.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Msar_Gded.Enums;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;


namespace Msar_Gded.Controllers
{

    public class RequestController : Controller
    {
        private readonly AppDbContext _context;

            public RequestController(AppDbContext context)
        {
            _context = context;
        }

        ////////////// The actions for the student [AllowAnonymous]

        // GET: Student/RequestForm
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RequestForm(string id)
        {

            if (id == null)
                return NotFound();



            var student = await _context.Students.Include(s => s.Requests)
                                           .Include(s => s.University)
                                           .Include(s => s.College)
                                           .FirstOrDefaultAsync(s => s.NationalId == id);

            var model = new StudentRequestVm();

            if (student != null)
            {
                TempData["StudentExists"] = true;
                // If student exists, pre-fill their personal data (readonly)
                model = new StudentRequestVm
                {

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
                };


            }

            model.NationalId = id;

            // Load universities and colleges for dropdowns
            ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name");
            ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name");


            return View(model);
        }

        // POST: Student/RequestForm
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestForm(StudentRequestVm model)
        {

            var student = _context.Students.Include(s => s.University)
                               .Include(s => s.College)
                               .FirstOrDefault(s => s.NationalId == model.NationalId);

            if (ModelState.IsValid)
            {
                try
                {



                    if (student == null && !await CreateStudent(model))
                    {
                        ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name", model.UniversityId);
                        ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name", model.CollegeId);
                        TempData["Error"] = "من فضلك تحقق من البيانات المدخلة.";
                        return View(model);
                    }
             


                    //becaue they will not be filled in the view
                    {
                        model.CurrentUniversityId = model.UniversityId;
                        model.CurrentCollegeId = model.CollegeId;
                    }

                    if(student != null){
                        TempData["StudentExists"] = true;
                        student.UniversityId = model.UniversityId;
                        student.CollegeId = model.CollegeId;
                        student.UniversityGrade = model.UniversityGrade;
                    }



                    var request = new Models.Request
                    {
                        DateOfRequest = model.DateOfRequest,
                        TypeOfRequest = model.TypeOfRequest,
                        // PaymentCode = model.PaymentCode,
                        //  NeedRivision = model.NeedRivision,
                        CurrentUniversityId = model.CurrentUniversityId,
                        CurrentCollegeId = model.CurrentCollegeId,
                        DestinationUniversityId = model.DestinationUniversityId,
                        DestinationCollegeId = model.DestinationCollegeId,
                        //  StatusId = model.StatusId,
                        StudentId = model.NationalId
                    };

                    _context.Requests.Add(request);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "تم حفظ البيانات و إضافة الطلب بنجاح!";

                    return RedirectToAction("StudentProfile", "Student", new { id = model.NationalId });
                }
                catch (Exception)
                {
                    TempData["Error"] = "حدث خطأ أثناء حفظ البيانات.";
                    ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name", model.UniversityId);
                    ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name", model.CollegeId);

                    return View(model);
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["Errors"] = errors;
            ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name", model.UniversityId);
            ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name", model.CollegeId);
            TempData["Error"] = "من فضلك تحقق من البيانات المدخلة.";
            if (student != null) TempData["StudentExists"] = true;


            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Edit(int id)
            {
            

                if (id == null)
                {
                    return NotFound();
                }

            var request = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.CurrentCollege).Include(r => r.DestinationCollege)
                .Include(r => r.CurrentUniversity).Include(r => r.DestinationUniversity)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(s => s.Id == id);


            var model = new StudentRequestVm();

            if (request != null)
            {
                TempData["StudentExists"] = true;
                // If student exists, pre-fill their personal data (readonly)
                model = new StudentRequestVm
                {
                    NationalId = request.Student.NationalId,
                    Name = request.Student.Name,
                    PhoneNumber = request.Student.PhoneNumber,
                    BirthDate = request.Student.BirthDate,
                    BirthPlace = request.Student.BirthPlace,
                    GraduationDate = request.Student.GraduationDate,
                    HighSchoolDivision = request.Student.HighSchoolDivision,
                    EducationalZone = request.Student.EducationalZone,
                    PureDegree = request.Student.PureDegree,
                    DegreeWithLanguage = request.Student.DegreeWithLanguage,
                    TotalDegree = request.Student.TotalDegree,
                    UniversityGrade = request.Student.UniversityGrade,
                    UniversityId = request.Student.UniversityId,
                    CollegeId = request.Student.CollegeId,


                    requestId = request.Id,
                    DateOfRequest = request.DateOfRequest,
                    TypeOfRequest = request.TypeOfRequest,
                    CurrentUniversityId = request.CurrentUniversityId,
                    CurrentCollegeId = request.CurrentCollegeId,
                    DestinationUniversityId = request.DestinationUniversityId,
                    DestinationCollegeId = request.DestinationCollegeId,
                    //statusId = request.StatusId

                };

            }

            // Load universities and colleges for dropdowns
            ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name");
            ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name");
            ViewData["Statuses"] = new SelectList(_context.Statuses, "Id", "Name");

            return View(model);

        }



        [HttpPost]
        [AllowAnonymous]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(StudentRequestVm model)
        {
            if(ModelState.IsValid)
            {
                var request = await _context.Requests
                    .Include(r => r.Student)
                    .Include(r => r.CurrentCollege).Include(r => r.DestinationCollege)
                    .Include(r => r.CurrentUniversity).Include(r => r.DestinationUniversity)
                    .Include(r => r.Status)
                    .FirstOrDefaultAsync(s => s.Id == model.requestId);


                if (model.CurrentUniversityId.HasValue && model.CurrentUniversityId.HasValue)
                {
                    request.Student.UniversityId = model.CurrentUniversityId.Value;
                    request.Student.CollegeId = model.CurrentCollegeId.Value;

                }
                else {
                    ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name", model.UniversityId);
                    ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name", model.CollegeId);
                    ViewData["Statuses"] = new SelectList(_context.Statuses, "Id", "Name", model.statusId);

                    TempData["Error"] = "الرجاء التحقق من البيانات المدخله.";
                    return View(model);

                }
                request.Student.UniversityGrade = model.UniversityGrade;

               request.TypeOfRequest = model.TypeOfRequest;
                 request.CurrentUniversityId =  model.CurrentUniversityId;
                request.CurrentCollegeId = model.CurrentCollegeId;
                 request.DestinationUniversityId = model.DestinationUniversityId;
                request.DestinationCollegeId = model.DestinationCollegeId;

                request.StatusId = _context.Statuses.Where(s => s.Name == "يحتاج لمراجعة بعد التعديل").Select(s => s.Id).FirstOrDefault();


                await _context.SaveChangesAsync();

                TempData["Success"] = "تم تعديل الطلب بنجاح";
                return RedirectToAction("StudentProfile", "Student", new { id = model.NationalId });


            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["Errors"] = errors;
            ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name", model.UniversityId);
            ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name", model.CollegeId);
            ViewData["Statuses"] = new SelectList(_context.Statuses, "Id", "Name", model.statusId);

            TempData["Error"] = "الرجاء التحقق من البيانات المدخله.";
            return View(model);

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
            .Include(r => r.Student)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (request == null)
            {
                ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name", request.Student.UniversityId);
                ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name", request.Student.CollegeId);
                ViewData["Statuses"] = new SelectList(_context.Statuses, "Id", "Name", request.StatusId);
                TempData["Error"] = "هذا الطلب غير موجود.";
                RedirectToAction("StudentProfile", "Student", new { id = request.Student.NationalId });
            }
            _context.Remove(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم حذف  بنجاح.";
            return RedirectToAction("StudentProfile", "Student", new { id = request.Student.NationalId });

        }




        ////////////// The actions for the Employees  

        [HttpGet]
        public async Task<IActionResult> GetRequests(int? statusId, int? typeOfRequestId)
        {

            ViewBag.controllerName = "Request";
            ViewBag.entity = "طلب";
            ViewBag.theEntity = "الطلب";
            ViewBag.pluralEntity = "طلبات";
            ViewBag.thePluralEntity = "الطلبات";
            ViewBag.placeHolder = "طلبات";



            var requests = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.CurrentCollege).Include(r => r.DestinationCollege)
            .Include(r => r.CurrentUniversity).Include(r => r.DestinationUniversity)
            .Include(r => r.Status).ToListAsync();

            if (statusId != null)
            {
                requests = requests.Where(r => r.StatusId == statusId).ToList(); ;
            }

            if (typeOfRequestId != null)
            {
                requests = requests.Where(r => r.TypeOfRequest == (RequestType)typeOfRequestId).ToList(); ;
            }

            return PartialView("_ShowAllRequests", requests);
        }
        [HttpGet]
        public async Task <IActionResult> ReviewRequest(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.CurrentCollege).Include(r => r.DestinationCollege)
                .Include(r => r.CurrentUniversity).Include(r => r.DestinationUniversity)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            var model = new ReviewRequestVm();

            if (request != null)
            {
                TempData["StudentExists"] = true;
                // If student exists, pre-fill their personal data (readonly)
                model = new ReviewRequestVm
                {
                    NationalId = request.Student.NationalId,
                    Name = request.Student.Name,
                    PhoneNumber = request.Student.PhoneNumber,
                    BirthDate = request.Student.BirthDate,
                    BirthPlace = request.Student.BirthPlace,
                    GraduationDate = request.Student.GraduationDate,
                    HighSchoolDivision = request.Student.HighSchoolDivision,
                    EducationalZone = request.Student.EducationalZone,
                    PureDegree = request.Student.PureDegree,
                    DegreeWithLanguage = request.Student.DegreeWithLanguage,
                    TotalDegree = request.Student.TotalDegree,
                    UniversityGrade = request.Student.UniversityGrade,
                    UniversityId = request.Student.UniversityId,
                    CollegeId = request.Student.CollegeId,


                    requestId = request.Id,
                    DateOfRequest = request.DateOfRequest,
                    TypeOfRequest = request.TypeOfRequest,
                    CurrentUniversityId = request.CurrentUniversityId,
                    CurrentCollegeId = request.CurrentCollegeId,
                    DestinationUniversityId = request.DestinationUniversityId,
                    DestinationCollegeId = request.DestinationCollegeId,
                    //statusId = request.StatusId

                };

            }

            // Load universities and colleges for dropdowns
            ViewData["Universities"] = new SelectList(_context.Universities, "Id", "Name");
            ViewData["Colleges"] = new SelectList(_context.Colleges, "Id", "Name");
            ViewData["Statuses"] = new SelectList(_context.Statuses, "Id", "Name");



            return View(model);

        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewRequest(ReviewRequestVm model)
        {
            
            if(model.statusId != 0)
            {
                var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == model.requestId);

                if (request == null)
                {
                    return NotFound();
                }    

                request.StatusId = model.statusId;

                    await _context.SaveChangesAsync();

                TempData["Success"] = "تم مراجعة الطلب بنجاح";
                return RedirectToAction("Index", "Admin", new { load = "Request/GetRequests" });


            }


            TempData["Error"] = "من فضلك أختار تقييم الطلب.";
            return RedirectToAction("ReviewRequest", new {id = model.requestId});



        }


        public async Task<IActionResult> Search(string keyword, int? statusId, int? typeOfRequestId)
        {


            var requests = await _context.Requests
                .Include(r => r.Student)
                .Include(r => r.CurrentCollege).Include(r => r.DestinationCollege)
            .Include(r => r.CurrentUniversity).Include(r => r.DestinationUniversity)
            .Include(r => r.Status).ToListAsync();



            if (statusId != null)
            {
                requests = requests.Where(r => r.StatusId == statusId).ToList();
            }

            if (typeOfRequestId != null)
            {
                requests = requests.Where(r => r.TypeOfRequest == (RequestType)typeOfRequestId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {

                if (keyword.All(char.IsDigit))
                {

                    requests = requests.Where(u => u.Student.NationalId.Contains(keyword)).ToList();
                }
                else
                {
                    requests = requests.Where(u => u.Student.Name.Contains(keyword)).ToList();

                }
            }

            return PartialView("_RequestSearchResult", requests.ToList());
        }




        [AllowAnonymous]
        private async Task<bool> CreateStudent(StudentRequestVm model)

        {
            try
            {


                    // If student does not exist, create a new student and request
                    var university = await _context.Universities.FindAsync(model.UniversityId);
                    var college = await _context.Colleges.FindAsync(model.CollegeId);

                    if (university == null || college == null)
                    {            
                        return false;
                    }


                    var newStudent = new Student
                    {
                        NationalId = model.NationalId,
                        Name = model.Name,
                        PhoneNumber = model.PhoneNumber,
                        BirthDate = model.BirthDate,
                        BirthPlace = model.BirthPlace,
                        GraduationDate = model.GraduationDate,
                        HighSchoolDivision = model.HighSchoolDivision,
                        EducationalZone = model.EducationalZone,
                        PureDegree = model.PureDegree,
                        DegreeWithLanguage = model.DegreeWithLanguage,
                        TotalDegree = model.TotalDegree,
                        UniversityGrade = model.UniversityGrade,
                        UniversityId = model.UniversityId,
                        CollegeId = model.CollegeId
                    };

         
                    // Handle file upload for certificates
                    if (model.SchoolCertificateUpload != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await model.SchoolCertificateUpload.CopyToAsync(memoryStream);
                            newStudent.SchoolCertificate = memoryStream.ToArray();
                        }
                    }

                    if (model.LeterOfStatusUpload != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await model.LeterOfStatusUpload.CopyToAsync(memoryStream);
                            newStudent.LeterOfStatus = memoryStream.ToArray();
                        }
                    }

                    _context.Students.Add(newStudent);
                    await _context.SaveChangesAsync();
                    return true;
                    //TempData["Success"] = "تم إضافة البيانات بنجاح!";
                }         
            catch (Exception)
            {
                return false;
            }
        }






        //[HttpPost]
        //public async Task<IActionResult> Search(string keyword, [FromBody] List<Request> reqList)
        //{
        //    IEnumerable<Request> requests = reqList;

        //    if (!string.IsNullOrWhiteSpace(keyword))
        //    {
        //        requests = reqList.Where(u => u.Student.Name.Contains(keyword));
        //    }

        //    return PartialView("_RequestSearchResult", requests.ToList());
        //}


    }



}
