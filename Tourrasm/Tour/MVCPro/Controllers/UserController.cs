using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPro.Data;
using MVCPro.Models;
using System.Net.Mail;
using MVCPro.Shared;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
namespace MVCPro.Controllers
{
    public class UserController : Controller
    {
        private readonly IHostingEnvironment _host;
        private AppDbContext _db;

        public UserController(AppDbContext _db, IHostingEnvironment host)
        {
            this._db = _db;
            _host = host;
        }
        public bool validemail(string email)
        {
            //bool isvalide;
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }


        [HttpGet]
        public IActionResult LoginUser()
        {

            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(LoginData login)
        {
            if (login == null)
            {
                return RedirectToAction();
            }
            if (global.currentUser != null)
            {
                return RedirectToAction("Profile");
            }
            if (!validemail(login.Email))
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View(login);
            }

            User v = _db.Users.FirstOrDefault(s => s.Email == login.Email);

            if (v == null)
            {
                ModelState.AddModelError("Password", "user not found");
                return View(login);
            }

            //if (!ModelState.IsValid)
            //{
            //    return View(login);
            //}

            if (login.Password != v.Password)
            {
                ModelState.AddModelError("password", "incorrect pass");
                return View(login);
            }
            //else
            //{

            //    //return Content("Privacy1");
            //}

            if (!ModelState.IsValid)
            {
                return View(login);
            }
            global.currentUser = v.NationalId;
            global.currentType = "User";
            ViewData["currentUser"] = global.currentUser;
            global.u = v;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View(new User());
        }


        [HttpPost]
        public IActionResult Register(User u, IFormCollection re, IFormFile ProfilePicture)
        {
            global.ispic = true;
            if (u == null)
            {
                return RedirectToAction();
            }
            if (global.currentUser != null)
            {
                return RedirectToAction("Profile");
            }
            string confirmPassword = re["ConfirmPassword"];
            User user1 = _db.Users.FirstOrDefault(s => s.NationalId == u.NationalId);
            User user = _db.Users.FirstOrDefault(s => s.Email == u.Email);
            if (user == null && user1 == null)
            {
                if (u.Password != confirmPassword)
                {
                    ModelState.AddModelError("Password", "Passwords do not match.");
                    return View(u);
                }
                if (string.IsNullOrWhiteSpace(u.Job))
                {
                    // If it's empty, clear the model state errors for "Job"
                    u.Job = "none";
                }

                if (ModelState.IsValid)
                {


                    if (ProfilePicture != null)
                    {
                        var photoExtension = Path.GetExtension(ProfilePicture.FileName).ToLower();
                        if (photoExtension != ".jpg" && photoExtension != ".png" && photoExtension != ".jpeg")
                        {
                            global.ispic = false;
                            ModelState.AddModelError("ImageUser", "Invalid Photo");
                            return View("Register", u);
                        }


                        string path = Path.Combine(_host.WebRootPath, "images/User");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);

                        path = Path.Combine(path, fileName);
                        using (var Stream = new FileStream(path, FileMode.Create))
                        {
                            ProfilePicture.CopyTo(Stream);
                            u.ImageUser = fileName;
                        }
                    }
                    //بتعمل اكسبشن
                    //u.Password = HashPassword(u.Password);
                    u.Password = u.Password;
                    // Add user to the database
                    _db.Users.Add(u);
                    _db.SaveChanges();


                    return RedirectToAction("LoginUser");
                }


                return View(u);
            }
            else
            {
                if (user1 != null && user == null)
                {
                    ModelState.AddModelError("NationalId", "this National number is alredy exit");
                    return View(u);
                }
                else if (user != null && user1 == null)
                {
                    ModelState.AddModelError("Email", "this email is alredy exit");
                    return View(u);
                }
                else
                {
                    ModelState.AddModelError("Email", "this email is alredy exit");
                    ModelState.AddModelError("NationalId", "this National number is alredy exit");
                    return View(u);
                }

            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult select()
        {
            ViewBag.op = "";
            return View();
        }

        [HttpPost]
        public RedirectToActionResult select(IFormCollection req)
        {
            string op = req["op"];
            global.currentType = op;
            ViewData["userType"] = global.currentType;
            if (op == "User")
                return RedirectToAction("LoginUser");
            else if (op == "Staff")
                return RedirectToAction("LoginStaff", "Staff");
            else
                return RedirectToAction("select");
        }



        public IActionResult Profile()
        {
            if (global.currentUser != null)
            {
                if (global.currentType == "User")
                {
                    User u = _db.Users.FirstOrDefault(s => s.NationalId == global.currentUser);
                    ViewData["currentUser"] = u;
                    ViewBag.EmailError = TempData["EmailError"];
                    ViewBag.UserNameError = TempData["UserNameError"];
                    ViewBag.NationalityError = TempData["NationalityError"];
                    ViewBag.PasswordError = TempData["PasswordError"];
                    ViewBag.ProfilePictureError = TempData["ProfilePictureError"];
                    return View();
                }
                else if (global.currentType == "Staff")
                {
                    Staff s = _db.staff.FirstOrDefault(s => s.NationalId == global.currentUser);
                    ViewData["currentUser"] = s;
                    return Redirect("https://localhost:44372/Staff/StaffMain");
                }
                else if (global.currentType == "Tour Guide")
                {
                    Staff t = _db.staff.FirstOrDefault(s => s.NationalId == global.currentUser);
                    ViewData["currentUser"] = t;
                    return Redirect("https://localhost:44372/Staff/TourGuideMain");
                }

            }

            return RedirectToAction("LoginUser");

        }

        // a function to validate the profile picture


        public IActionResult SignOut()
        {
            global.currentUser = null;
            return RedirectToAction("Index", "Home");
        }

        // delete user
        public IActionResult DeleteAccount()
        {
            User user = _db.Users.FirstOrDefault(s => s.NationalId == global.currentUser);

            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                global.currentUser = null;
            }

            return RedirectToAction("Index", "Home");
        }

        // update user
        [HttpGet]
        public ActionResult UpdateUser()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult UpdateUser(User user, IFormFile ProfilePicture, IFormCollection re)
        {
            bool f = true;
            var existingUser = _db.Users.FirstOrDefault(s => s.NationalId == global.currentUser);
            if (existingUser != null)
            {
                if (user.UserName != null)
                {
                    if (!Regex.IsMatch(user.UserName, @"^[a-zA-Z\s]+$"))
                    {
                        f = false;
                        TempData["UserNameError"] = "Invalid name.";

                    }
                    existingUser.UserName = user.UserName;
                }
                if (user.Email != null)
                {
                    if (!Regex.IsMatch(user.Email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)$"))
                    {
                        f = false;
                        TempData["EmailError"] = "Invalid email format. Please enter a valid email address.";

                    }
                    existingUser.Email = user.Email;
                }
                if (user.Nationality != null)
                {
                    if (!Regex.IsMatch(user.Nationality, @"^[a-zA-Z\s]+$"))
                    {
                        f = false;
                        TempData["NationalityError"] = "Invalid Nationality.";

                    }
                    existingUser.Nationality = user.Nationality;
                }
                if (user.Job != null)
                {
                    existingUser.Job = user.Job;
                }
                if (user.Password != null)
                {
                    string confirmPassword = re["ConfirmPassword"];
                    if (user.Password != confirmPassword)
                    {
                        f = false;
                        TempData["PasswordError"] = "paswords do not match";

                    }
                    existingUser.Password = user.Password;
                }

                // Check if a new image file is uploaded
                string path = Path.Combine(_host.WebRootPath, "images/User");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (ProfilePicture != null)
                {

                    var fileExtension = Path.GetExtension(ProfilePicture.FileName).ToLower();
                    if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
                    {
                        f = false;
                        TempData["ProfilePictureError"] = "Invalid photo.";
                        //ModelState.AddModelError("ImageUser", " InValid Photo ");
                        //return View("Profile", user);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);

                    path = Path.Combine(path, fileName);
                    using (var Stream = new FileStream(path, FileMode.Create))
                    {
                        ProfilePicture.CopyTo(Stream);
                        user.ImageUser = fileName;
                        existingUser.ImageUser = user.ImageUser;

                    }
                }
                else
                {
                    user.ImageUser = user.ImageUser ?? "default.png";
                }
                if (!f)
                {
                    return RedirectToAction("Profile");
                }
                _db.Users.Update(existingUser);
                _db.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to Index action after successful update
            }
            else
            {
                ModelState.AddModelError("", "User not found.");
            }

            return RedirectToAction("Index", "Home", user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return HashPassword(inputPassword) == hashedPassword;
        }


    }
}