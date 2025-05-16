using Msar_Gded.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Msar_Gded.Models;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Microsoft.EntityFrameworkCore;

namespace Msar_Gded.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthController : Controller
    {

        // service Layer of Identity
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;





        // SignInManager which creating cookie
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly AppDbContext _context;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            LogInVm vm = new LogInVm();
            return View("HomePage" , vm);


        }

        public IActionResult Welcome()
        {
            return View();
        }





        [HttpGet]
        public IActionResult SignUp()
        {

            //  ViewBag.Colleges = _context.Colleges.Select(x => x.Name).ToList();  
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpVm newUser)
        {


            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            // ViewBag.Colleges = _context.Colleges.Select(x => x.Name).ToList();



            if (ModelState.IsValid)
            {


                // Check if the user already exists
                var existingUser = await _userManager.FindByNameAsync(newUser.UserName);
                if (existingUser != null)
                {
                   TempData["Error"] = "اسم المستخدم موجود بالفعل. الرجاء اختيار اسم آخر.";
                    return View(newUser);
                }

                // Check if the PhoneNumber already exists
                var existingPhoneNumber = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == newUser.PhoneNumber);
                if (existingPhoneNumber != null)
                {
                    TempData["Error"] =  "رقم الهاتف مستخدم من قبل شخص اخر";
                    return View(newUser);
                }

                ApplicationUser userModel = new ApplicationUser
                {
                    UserName = newUser.UserName,
                    FullName = newUser.FullName,
                    NationalId = newUser.NationalId,
                    PhoneNumber = newUser.PhoneNumber,
                    Role = newUser.Role
               
                };

                IdentityResult result = await _userManager.CreateAsync(userModel, newUser.Password);



                if (result.Succeeded)
                {
                    TempData["Success"] = "تم تسجيل المستخدم بنجاح";
                    await _userManager.AddToRoleAsync(userModel, newUser.Role);
                    return RedirectToAction("Index", "Admin", new { load = "Auth/ShowAll" });
                }
                else
                {
                    TempData["Error"] = "يبدو ان هناك بيانات غير صالحه, الرجاء مراجعة البيانات المدخله و المحاوله مرة اخري";
                    return View(newUser); 

                }
            }
            //Debugging: Log ModelState errors if validation fails before creating the user
            var errorsFromModelState = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["Errors"] = errorsFromModelState;
            TempData["Error"] = "يبدو ان هناك بيانات غير صالحه, الرجاء مراجعة البيانات المدخله و المحاوله مرة اخري";
            return View(newUser); // Return the view with errors
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInVm user)
        {


            // model state hena malhash lazma 3shan el propperties fe el vm are nullable w an mhandle el vlaidation manually
            // laken ana hasebha 3shan momken ykon leha lazma tany an ma3rafhash
             if (ModelState.IsValid)
            {

               // mhandle el vlaidation manually
                if (user.UserName == null)
                {
                    TempData["Error"] = " الرجاء إدحال  إسم المستخدم.";
                    return View("HomePage", user);
                }
                var userModel = await _userManager.FindByNameAsync(user.UserName);

                if (userModel != null)
                {

                    // mhandle el vlaidation manually
                    if (user.Password == null)
                    {
                        TempData["Error"] = " الرجاء إدحال كلمة المرور.";
                        return View("HomePage", user);
                    }
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(userModel, user.Password, false, false);

                    //Adding customized cookie when user Sign in
                    {
                        string phoneNum = userModel.PhoneNumber;
                        List<Claim> claims = new List<Claim>();
                        //  claims.Add(new Claim("Phone", phoneNum));
                        claims.Add(new Claim("Role", userModel.Role));
                        //   claims.Add(new Claim("Department", userModel.Department));



                        await _signInManager.SignInWithClaimsAsync(userModel, new AuthenticationProperties
                        {
                            IsPersistent = false, // Ensures session-based authentication
                            AllowRefresh = true // Allows the session to be refreshed while the browser is open
                        }, claims);

                    }

                    if (result.Succeeded)
                    {
                            return RedirectToAction("Index", "Admin" , new { load = "Request/GetRequests" });          
                    }
                    else
                    {

                        TempData["Error"] =  "اسم المستخدم او كلمه المرور غير صحيحه";
                        return View("HomePage", user);

                    }


                }
                else
                {

                    TempData["Error"] = "اسم المستخدم او كلمه المرور غير صحيحه";
                    return View("HomePage" , user);
                }
            }
            else
            {
                TempData["Error"] = "ادخل بيانات صحيحه";
                return View("HomePage" , user);
            }
        }


        [HttpGet]
        public IActionResult ShowAll()
        {

            ViewBag.controllerName = "Auth";
            ViewBag.entity = "مستخدم";
            ViewBag.theEntity = "المستخدم";
            ViewBag.pluralEntity = "مستخدمين";
            ViewBag.thePluralEntity = "المستخدمين";
            ViewBag.placeHolder = "بدر أحمد";
            var users = _userManager.Users.ToList();
            return PartialView("_ShowAllUsers", users);
        }



        [HttpGet]

        public async Task<IActionResult> Details(string id)
        {
            // Safely retrieve the user ID from claims
            //var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            //if (claim == null || string.IsNullOrEmpty(claim.Value))
            //{
            //    return NotFound();
            //}

            //    var id = claim.Value;

            ViewBag.Roles = _roleManager.Roles.ToList();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "عفواو المستخدم غير موجود.";
             


                return RedirectToAction("Index", "Admin", new { load = "Auth/ShowAll" });

            }

            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return NotFound();

                var existingRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, existingRoles);
                user.UserName = model.UserName;
                user.FullName = model.FullName;
                user.NationalId = model.NationalId;
                user.PhoneNumber = model.PhoneNumber;
    
                user.Role = model.Role;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {

                    if (!string.IsNullOrEmpty(user.Role))
                    {
                        await _userManager.AddToRoleAsync(user, user.Role);
                    }
                    TempData["success"] = "تم تعديل بيانات الموظف بنجاح";

                    return RedirectToAction("Index", "Admin", new { load = "Auth/ShowAll" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            TempData["Error"] = "فشل التعديل,الرجاء مراجعه البيانات و المحاوله مره أخري";

            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(model);
        }


        // [HttpGet]
        //public async Task<IActionResult> Delete(string id)
        //{
        //	if (id == null) return NotFound();
        //	var user = await _userManager.FindByIdAsync(id);
        //	if (user == null) return NotFound();
        //	return View(user);
        //}

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["success"] = "تم حذف الموظف من قواعد البيانات بنجاح";

                    return RedirectToAction("Index", "Admin", new { load = "Auth/ShowAll" });

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            TempData["Error"] = "فشل الحذف,الرجاء مراجعه البيانات و المحاوله مره أخري";

            return RedirectToAction("Index", "Admin", new { load = "Auth/ShowAll" });

        }


        public async Task<IActionResult> Search(string keyword)
        {
            IEnumerable<Msar_Gded.Models.ApplicationUser> users;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                users = _userManager.Users.Where(u => u.FullName.Contains(keyword));
            }
            else
            {
                users = _userManager.Users;
            }
            return PartialView("_UserSearchResults", users.ToList());
        }

        public void TestAuth()
        {
            Console.WriteLine("Badr " + User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Console.WriteLine(User.Identity.IsAuthenticated);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            _signInManager.SignOutAsync();
            LogInVm vm = new LogInVm();
            return View("HomePage" , vm);
        }


        public IActionResult AccessDenied()
         {
            return View();
        
        }

    }
}
