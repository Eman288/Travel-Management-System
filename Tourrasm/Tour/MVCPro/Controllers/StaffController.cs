using Microsoft.AspNetCore.Mvc;
using MVCPro.Data;
using MVCPro.Models;
using NuGet.Protocol.Plugins;
using System.Linq;
using System.Net.Mail;
using MVCPro.Shared;
using System.Runtime.Intrinsics.X86;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Text;
using System.Security.Cryptography;

namespace MVCPro.Controllers
{
    public class StaffController : Controller
    {
        private readonly IHostingEnvironment _host;
        private AppDbContext _db;

        public StaffController(AppDbContext _db, IHostingEnvironment host)
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
        public IActionResult LoginStaff()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginStaff(LoginDataStaff s)
        {
            if (s == null)
            {
                return RedirectToAction();
            }
            
            if (!validemail(s.Email))
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View(s);
            }

            Staff v = _db.staff.FirstOrDefault(f => f.Email == s.Email);

            if (v == null)
            {
                ModelState.AddModelError("Password", "user not found");
                return View(s);
            }

            //if (!ModelState.IsValid)
            //{
            //    return View(login);
            //}

            if (v.Password != s.Password)
            {
                ModelState.AddModelError("password", "incorrect pass");
                return View(s);
            }
            //else
            //{

            //    //return Content("Privacy1");
            //}
            if (v.StaffCode != s.StaffCode)
            {
                ModelState.AddModelError("StaffCode", "incorrect code");
                return View(s);
            }

            if (!ModelState.IsValid)
            {
                return View(s);
            }
            global.currentUser = v.NationalId;
            global.currentType = v.Type;
            ViewData["currentUser"] = global.currentUser;
            if (v.Type == "Staff")
            {
                var staff = _db.staff.FirstOrDefault(s => s.NationalId == global.currentUser);
                ViewData["currentStaff"] = staff;
                global.s = staff;
                return View("StaffMain");
            }
            else if (v.Type == "Tour Guide")
            {
                var staff = _db.staff.FirstOrDefault(s => s.NationalId == global.currentUser);
                ViewData["currentStaff"] = staff;
                global.s = staff;
                return View("TourGuideMain");
            }
            else
            {
                return new NotFoundResult();
            }

        }

        

        public IActionResult SignOut()
        {
            global.currentUser = null;
            global.currentType = null;
            return Redirect("https://localhost:44372/Home/Index");
        }

        public IActionResult GoToMainStaff()
        {
            Staff s = _db.staff.FirstOrDefault(n => n.NationalId == global.currentUser);
            ViewData["currentStaff"] = s;
            return View("StaffMain");
        }

        public IActionResult StaffMain()
        {
            return View();
        }

        public IActionResult TourGuideMain()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StaffCreation()
        {
            if (global.currentUser == "00000000000001")
            {
                ViewData["doneStaff"] = false;
                return View(new Staff());
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        public IActionResult StaffCreation(Staff st, IFormCollection re, IFormFile ProfilePicture)
        {

            ViewData["doneStaff"] = false;
            if (st == null)
            {
                return RedirectToAction();
            }
            if (global.currentUser == "00000000000001")
            {
                ViewData["doneStaff"] = false;
                string confirmPassword = re["ConfirmPassword"];
                Staff s1 = _db.staff.FirstOrDefault(s => s.NationalId == st.NationalId);
                Staff s2 = _db.staff.FirstOrDefault(s => s.StaffCode == st.StaffCode);
                Staff s = _db.staff.FirstOrDefault(s => s.Email == st.Email);

                if (s == null && s1 == null && s2 == null)
                {
                    if (st.Password != confirmPassword)
                    {
                        ModelState.AddModelError("Password", "Passwords do not match.");
                        return View(st);
                    }
                    

                    if (ModelState.IsValid)
                    {
                        if (st.ProfilePicture != null && st.ProfilePicture.Length > 0)
                        {
                            if (!IsImageFile(st.ProfilePicture))
                            {
                                ModelState.AddModelError("ProfilePicture", "Please upload an image with a JPG or PNG extension.");
                                return View(st);
                            }
                            // Generate unique filename for the image
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(st.ProfilePicture.FileName);

                            // Define the path to save the image
                           
                            if (st.Type == "Staff")
                            {
                                var p = "images/Staff";
                                var filePath = Path.Combine(_host.WebRootPath, p, fileName);
                                // Save the image to the specified path
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    st.ProfilePicture.CopyTo(stream);
                                }

                                // Set the ImageUser property to the filename
                                st.ImageUser = fileName;
                            }
                            else
                            {
                                var p = "images/TourGuide";
                                var filePath = Path.Combine(_host.WebRootPath, p, fileName);
                                // Save the image to the specified path
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    st.ProfilePicture.CopyTo(stream);
                                }
                                // Set the ImageUser property to the filename
                                st.ImageUser = fileName;
                            }
                        }
                        // Add user to the database
                        _db.staff.Add(st);
                        _db.SaveChanges();

                    ViewData["doneStaff"] = true;
                    return View();
                    }


                    return View(st);
                }
                else
                {
                    if (s1 != null && s == null)
                    {
                        ViewData["doneStaff"] = false;
                        ModelState.AddModelError("NationalId", "this National number is alredy exit");
                        return View(st);
                    }
                    else if (s != null && s1 == null)
                    {
                        ViewData["doneStaff"] = false;
                        ModelState.AddModelError("Email", "this email is alredy exit");
                        return View(st);
                    }
                    else if (s2 != null)
                    {
                        ViewData["doneStaff"] = false;
                        ModelState.AddModelError("StaffCode", "There is already a staff with that name");
                        return View(st);
                    }
                    else
                    {
                        ViewData["doneStaff"] = false;
                        ModelState.AddModelError("Email", "this email is alredy exit");
                        ModelState.AddModelError("NationalId", "this National number is alredy exit");
                        return View(st);
                    }

                }
            }
            else
            {
                return new NotFoundResult();
            }
        }

        public IActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete()
        {
            ViewData["deleteStaff"] = false;

            if (global.currentUser == "00000000000001")
            {
                return View(_db.staff.Where(a=> a.NationalId != "00000000000001").ToList());
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        public IActionResult Delete(string t)
        {
            if (global.currentUser == "00000000000001")
            {
                var staffToDelete = _db.staff.FirstOrDefault(s => s.StaffCode == t);
                var trips = _db.trips.Where(a => a.staff == global.currentUser).ToList();
                var s = _db.staff.Where(a => a.NationalId == "00000000000001").FirstOrDefault();
                if (staffToDelete != null)
                {
                    
                        if (trips != null || trips.Count != 0)
                        {
                            foreach (Trip i in trips)
                            {
                                i.staff = "00000000000001";
                                i.Staff = s;
                                _db.trips.Update(i);
                                _db.SaveChanges();
                            }
                        }
                        _db.staff.Remove(staffToDelete);
                        _db.SaveChanges();
                        //global.currentUser = null;
                        ViewData["deleteStaff"] = true;
                        return RedirectToAction(nameof(GoToMainStaff));
                }
                else
                {
                    //ModelState.AddModelError("StaffCode", "There is no Staff or Tour guide with this code");
                    return new NotFoundResult();
                }
            }
            else
            {
                return new NotFoundResult();
            }

        }


        // update user
        [HttpGet]
        public ActionResult UpdateStaff()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult UpdateStaff(Staff s, IFormFile profilePicture)
        {
            //if (ModelState.IsValid)
            //{
            // Perform update logic here
            var ex = _db.staff.FirstOrDefault(s => s.NationalId == global.currentUser); // Get the existing user from your database or storage
            if (ex != null)
            {
                if (s.Name != null)
                {
                    ex.Name = s.Name;
                }
                if (s.Email != null)
                {
                    ex.Email = s.Email;
                }

                if (s.Password != null)
                {
                    ex.Password = s.Password;
                }
                if (s.StaffCode != null)
                {
                    s.StaffCode = s.StaffCode;
                }
                string path;
                if (global.currentType == "Staff")
                {
                    // Check if a new image file is uploaded
                    path = Path.Combine(_host.WebRootPath, "images/Staff");
                }
                else
                {
                    path = Path.Combine(_host.WebRootPath, "images/TourGuide");
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (profilePicture != null)
                {

                    var fileExtension = Path.GetExtension(profilePicture.FileName).ToLower();
                    if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
                    {
                        ModelState.AddModelError("ImageUser", " InValid Photo ");
                        return View("StaffMain", s);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(s.ProfilePicture.FileName);
                    path = Path.Combine(path, fileName);
                    using (var Stream = new FileStream(path, FileMode.Create))
                    {
                        profilePicture.CopyTo(Stream);
                        s.ImageUser = fileName;
                        ex.ImageUser = s.ImageUser;

                    }
                }
                else
                {
                    s.ImageUser = s.ImageUser ?? "default.png";
                }


                s.NationalId = ex.NationalId;


                _db.staff.Update(ex);
                _db.SaveChanges();

                return RedirectToAction("StaffMain"); // Redirect to a different action after successful update



            }
            else
            {

                ModelState.AddModelError("", "Staff not found.");
            }
            //}

            return View("StaffMain");
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



        private bool IsImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (file.Length > 0 && allowedExtensions.Contains(fileExtension))
            {
                if (file.ContentType.StartsWith("image"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
