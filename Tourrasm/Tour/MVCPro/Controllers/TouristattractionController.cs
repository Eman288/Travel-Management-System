using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPro.Data;
using MVCPro.Models;
using MVCPro.Shared;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Text.RegularExpressions;
using MVCPro.Migrations;
namespace MVCPro.Controllers
{
    public class TouristattractionController : Controller
    {
        [Obsolete]
        private readonly IHostingEnvironment _host;
        private AppDbContext _db;
        [Obsolete]
        public TouristattractionController(AppDbContext _db, IHostingEnvironment host)
        {
            this._db = _db;
            _host = host;
        }
        //public IActionResult Index()
        //{

        //    var tourists = _db.tourists.ToList();
        //    return View(tourists);

        //}
        public IActionResult Index(string searchby, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                ViewBag.SearchPerformed = false;
                return View(_db.tourists.ToList());
            }
            else
            {
                ViewBag.SearchPerformed = true;
                if (searchby == "Name")
                    return View(_db.tourists.Where(x => x.Name.StartsWith(search)).ToList());
                else
                    return View(_db.tourists.Where(x => x.City.StartsWith(search)).ToList());


            }
        }
        [HttpGet]
        public IActionResult add_Tourist_attraction()
        {
            return View(new TouristAttraction());
        }

        [HttpPost]
        public IActionResult add_Tourist_attraction(TouristAttraction TA)
        {

            if (string.IsNullOrEmpty(TA.Name) || string.IsNullOrEmpty(TA.City) || string.IsNullOrEmpty(TA.Description))
            {
                if (string.IsNullOrEmpty(TA.Name))
                {
                    ModelState.AddModelError("Name", "Enter Name");
                    return View(TA);
                }
                if (string.IsNullOrEmpty(TA.City))
                {
                    ModelState.AddModelError("City", "Enter city");
                    return View(TA);
                }
                if (string.IsNullOrEmpty(TA.Description))
                {
                    ModelState.AddModelError("Description", "Enter Description");
                    return View(TA);
                }
            }
            else
            {
                TouristAttraction ta0 = _db.tourists.FirstOrDefault(s => s.Name == TA.Name);
                if (TA == null)
                    return View();
                //if (global.currentTouristAttraction != null)
                //{
                //    return RedirectToAction("View_Tourist_attraction");
                //}
                if (ta0 != null)
                {
                    ModelState.AddModelError("Name", "This Tourist Attraction is already exited");
                    return View(TA);
                }
                else
                {
                    if (TA.TouristAttractionPicture != null && TA.TouristAttractionPicture.Length > 0)
                    {
                        if (!IsImageFile(TA.TouristAttractionPicture))
                        {
                            ModelState.AddModelError("TouristAttractionPicture", "Please upload an image with a JPG or PNG extension.");
                            return View(TA);
                        }
                    }
                    if (!validname(TA.Name) || !validname(TA.City))
                    {
                        if (!validname(TA.Name) && validname(TA.City))
                        {
                            ModelState.AddModelError("Name", "Invalid name!");
                            return View(TA);
                        }
                        else if (validname(TA.Name) && !validname(TA.City))
                        {
                            ModelState.AddModelError("City", "Invalid city!");
                            return View(TA);
                        }
                        else
                        {
                            ModelState.AddModelError("City", "Invalid city!");
                            ModelState.AddModelError("Name", "Invalid name!");
                            return View(TA);
                        }

                    }
                    else
                    {
                        if (TA.TouristAttractionPicture != null && TA.TouristAttractionPicture.Length > 0)
                        {
                            if (!IsImageFile(TA.TouristAttractionPicture))
                            {
                                ModelState.AddModelError("TouristAttractionPicture", "Please upload an image with a JPG or PNG extension.");
                                return View(TA);
                            }
                            // Generate unique filename for the image
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(TA.TouristAttractionPicture.FileName);

                            // Define the path to save the image
                            var filePath = Path.Combine(_host.WebRootPath, "images/TouristAttraction", fileName);

                            // Save the image to the specified path
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                TA.TouristAttractionPicture.CopyTo(stream);
                            }

                            // Set the ImageUser property to the filename
                            TA.Picture = fileName;
                        }
                        if (validname(TA.Name) && validname(TA.City))
                        {
                            _db.tourists.Add(TA);
                            _db.SaveChanges();
                            global.currentTouristAttraction = TA.Name;
                            ViewData["currentTouristAttraction"] = global.currentTouristAttraction;
                            return RedirectToAction("Index", TA);
                        }

                    }
                }
                return View("Index");
            }
            return View("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id != null)
            {
                TouristAttraction ta = _db.tourists.FirstOrDefault(s => s.Id == id);
                ViewData["currentTouristAttraction"] = ta;
                return View(ta);
            }
            
            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult Update(int id ,TouristAttraction ta, IFormFile TouristAttractionPicture)
        {
            var existingTouristAttraction = _db.tourists.FirstOrDefault(s => s.Id == id);
            var ex0 = _db.tourists.FirstOrDefault(s => s.Id == id);
            if (existingTouristAttraction != null && ex0 != null)
            {
                if (ta.TouristAttractionPicture != null && ta.TouristAttractionPicture.Length > 0)
                {
                    if (!IsImageFile(ta.TouristAttractionPicture))
                    {
                        ModelState.AddModelError("TouristAttractionPicture", "Please upload an image with a JPG or PNG extension.");
                        return View(ta);
                    }
                }
                if (!validname(ta.Name) || !validname(ta.City))
                {
                    if (!validname(ta.Name) && validname(ta.City))
                    {
                        ModelState.AddModelError("Name", "Invalid name!");
                        return View(existingTouristAttraction);
                    }
                    else if (validname(ta.Name) && !validname(ta.City))
                    {
                        ModelState.AddModelError("City", "Invalid city!");
                        return View(existingTouristAttraction);
                    }
                    else
                    {
                        ModelState.AddModelError("City", "Invalid city!");
                        ModelState.AddModelError("Name", "Invalid name!");
                        return View(existingTouristAttraction);
                    }
                }
                else
                {
                    if (ta.Name != null)
                    {
                        TouristAttraction ta0 = _db.tourists.FirstOrDefault(s => s.Name == ta.Name);
                        if (ta0 != null && ta0.Id != ta.Id)
                        {
                            ModelState.AddModelError("Name", "This Tourist Attraction is already exited");
                            return View(ta);
                        }
                        else
                            existingTouristAttraction.Name = ta.Name;
                    }
                    //if (string.IsNullOrEmpty(ta.Name))
                    //    existingTouristAttraction.Name = ex0.Name;
                    if (ta.Description != null)
                    {
                        existingTouristAttraction.Description = ta.Description;
                    }
                    //if (string.IsNullOrEmpty(ta.Description))
                    //    existingTouristAttraction.Description = ex0.Description;
                    if (ta.City != null)
                    {
                        existingTouristAttraction.City = ta.City;
                    }
                    //if (string.IsNullOrEmpty(ta.City))
                    //    existingTouristAttraction.City = ex0.City;
                    // Check if a new image file is uploaded
                    if (ta.TouristAttractionPicture != null && ta.TouristAttractionPicture.Length > 0)
                    {
                        if (existingTouristAttraction.Picture != null)
                        {
                            var existingImagePath = Path.Combine(_host.WebRootPath, "images/TouristAttraction", existingTouristAttraction.Picture);
                            if (System.IO.File.Exists(existingImagePath))
                            {
                                System.IO.File.Delete(existingImagePath);
                            }
                        }
                        if (!IsImageFile(ta.TouristAttractionPicture))
                        {
                            ModelState.AddModelError("TouristAttractionPicture", "Please upload an image with a JPG or PNG extension.");
                            return View(ta);
                        }
                        // Generate unique filename for the image
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ta.TouristAttractionPicture.FileName);

                        // Define the path to save the image
                        var filePath = Path.Combine(_host.WebRootPath, "images/TouristAttraction", fileName);

                        // Save the image to the specified path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ta.TouristAttractionPicture.CopyTo(stream);
                        }

                        // Set the ImageUser property to the filename
                        existingTouristAttraction.Picture = fileName;
                    }
                    _db.tourists.Update(existingTouristAttraction);
                    _db.SaveChanges();
                    
                    return RedirectToAction("Index"); // Redirect to a different action after successful update
                }
            }
            else
            {

                ModelState.AddModelError("", "Tourist Attraction not found.");
            }
            //}
            
            return RedirectToAction("Index");
        }
        public IActionResult View_Tourist_attraction(TouristAttraction ta)
        {
            //if (global.currentTouristAttraction != null)
            //{
            //    TouristAttraction ta = _db.tourists.FirstOrDefault(s => s.Name == global.currentTouristAttraction);
            //    ViewData["currentTouristAttraction"] = ta;
            //    return View();
            //}

            //return RedirectToAction("Index");
            
            ViewData["currentTouristAttraction"] = ta;
            return View(ta);

        }

        public IActionResult delete_Tourist_attraction(int id)
        {
            
            TouristAttraction ta = _db.tourists.FirstOrDefault(s => s.Id == id);
            var tripsAtt = _db.tripatt.Where(s => s.TouristAttractionId == id).ToList();
            if (tripsAtt != null || tripsAtt.Count != 0)
            {
                var tripsiDs = tripsAtt.Select(a => a.TripId).ToList();
                var usertrips = _db.usertrips.Where(u => tripsiDs.Contains(u.TripId)).ToList();
                if (usertrips == null || usertrips.Count == 0)
                {
                    foreach (TripAtt i in tripsAtt)
                    {
                        _db.tripatt.Remove(i);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    global.checkIndex = false;
                    RedirectToAction("Index");
                }
            }
            if (ta != null)
            {
                _db.tourists.Remove(ta);
                _db.SaveChanges();
                global.currentTouristAttraction = null;
            }

            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public IActionResult Update(TouristAttraction ta) 
        //{
        //    _db.Users.Update(ta);
        //    _db.SaveChanges();

        //    return RedirectToAction("Index");
        //}
        [NonAction]
        private bool IsImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (file.Length > 0 && allowedExtensions.Contains(fileExtension))
            {
                //if (file.ContentType.StartsWith("image"))
                //{
                return true;
                //}
            }
            return false;
        }
        [NonAction]
        public bool validname(string name)
        {
            //isvalid = false;
            String regex = @"^[a-z' 'A-Z]+$";
            Regex rgex = new Regex(regex);
            if (rgex.IsMatch(name))
            {
                return true;
            }
            else
                return false;
        }
    }
}
