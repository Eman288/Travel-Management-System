using Microsoft.AspNetCore.Mvc;
using MVCPro.Data;
using MVCPro.Models;
using NuGet.Protocol.Plugins;
using System.Linq;
using System.Net.Mail;
using MVCPro.Shared;
using System.Runtime.Intrinsics.X86;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.EntityFrameworkCore;
using MVCPro.Migrations;
using System.Collections.Generic;

namespace MVCPro.Controllers
{
    public class TripController : Controller
    {
        private readonly IHostingEnvironment _host;
        private AppDbContext _db;
        public TripController (AppDbContext _db, IHostingEnvironment host)
        {
            this._db = _db;
            _host = host;
        }

        //public IActionResult Index()
        //{
        //    var trips = _db.trips.Where(t => t.staff == global.currentUser).ToList();
        //    return View(trips);
        //}

        public IActionResult Index(string searchby, string search, DateTime? searchDate)
        {
            ViewBag.SearchPerformed = false;
            ViewBag.SelectedSearchBy = searchby;
            if (string.IsNullOrEmpty(search) && searchby != "Today" && searchby != "months")
            {
                if (global.isIndex == true)
                {
                    return View(_db.trips.Where(t => t.staff == global.currentUser).ToList());
                }
                else
                {
                    ViewData["Trips"] = _db.trips.ToList();
                    return RedirectToAction(nameof(DisplayTrips));
                }
            }

            ViewBag.SearchPerformed = true;
            if (searchby == "City" && !string.IsNullOrEmpty(search))
            {
                if (global.isIndex == true)
                {
                    return View(_db.trips.Where(x => x.City.StartsWith(search) && x.staff == global.currentUser).ToList());
                }
                else
                {
                    ViewData["Trips"] = _db.trips.Where(x => x.City.StartsWith(search)).ToList();
                    return RedirectToAction(nameof(DisplayTrips));
                }
                
            }
            else if (searchby == "StartDate")
            {
                DateTime date;
                if (DateTime.TryParseExact(search, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out date))
                {
                    if (global.isIndex == true)
                    {
                        return View(_db.trips.Where(x => x.StartDate.Date == date.Date && x.staff == global.currentUser).ToList());
                    }
                    else
                    {
                        ViewData["Trips"] = _db.trips.Where(x => x.StartDate.Date == date.Date).ToList();
                        return RedirectToAction(nameof(DisplayTrips));
                    }
                }

                if (global.isIndex == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //ViewData["Trips"] = _db.trips.Where(x => x.StartDate.Date == date.Date && x.staff == global.currentUser).ToList();
                    return RedirectToAction(nameof(DisplayTrips));
                }
                
            }
            else if (searchby == "Today")
            {
                if (global.isIndex == true)
                {
                    return View(_db.trips.Where(x => x.StartDate.Date == DateTime.Now.Date && x.staff == global.currentUser).ToList());
                }
                else
                {
                    ViewData["Trips"] = _db.trips.Where(x => x.StartDate.Date == DateTime.Now.Date).ToList();
                    return RedirectToAction(nameof(DisplayTrips));
                }
                
            }
            else if (searchby == "months")
            {
                if (global.isIndex == true)
                {
                    return View(_db.trips.Where(x => x.StartDate.Month == DateTime.Now.Month && x.staff == global.currentUser).ToList());
                }
                else
                {
                    ViewData["Trips"] = _db.trips.Where(x => x.StartDate.Month == DateTime.Now.Month).ToList();
                    return RedirectToAction(nameof(DisplayTrips));
                }
            }
            if (global.isIndex == true)
            {
                return View(_db.trips.Where(a => a.staff == global.currentUser).ToList());
            }
            else
            {
                ViewData["Trips"] = _db.trips.ToList();
                return RedirectToAction(nameof(DisplayTrips));
            }
            
        }

        [HttpGet]
        public IActionResult CreateTrip() 
        {
            return View(new Trip());
        }

        [HttpPost]
        public IActionResult CreateTrip(Trip t, IFormCollection re, IFormFile ProfilePicture)
        {
            if (t == null)
            {
                return RedirectToAction();
            }
            if (global.currentUser == null)
            {
                return new NotFoundResult();
            }
            Staff s = _db.staff.FirstOrDefault(n => n.NationalId == global.currentUser);
            t.staff = global.currentUser;
            t.Staff = s;
            //if (ModelState.IsValid)
            //{
            if (t.ProfilePicture != null && t.ProfilePicture.Length > 0)
            {
                if (!IsImageFile(t.ProfilePicture))
                {
                    ModelState.AddModelError("ProfilePicture", "Please upload an image with a JPG or PNG extension.");
                    return View(t);
                }
                // Generate unique filename for the image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(t.ProfilePicture.FileName);

                // Define the path to save the image
                var filePath = Path.Combine(_host.WebRootPath, "images/Trip", fileName);

                // Save the image to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    t.ProfilePicture.CopyTo(stream);
                }

                // Set the ImageUser property to the filename
                t.ImageUser = fileName;
            }

                _db.trips.Add(t);
                _db.SaveChanges();
            //int i = 7;
            //while (i != 0)
            //{
            //    TripAtt ta = new TripAtt();
            //    ta.TripId = t.TripId;
            //    ta.Trip = t;
            //    ta.Order = i;
            //    i--;
            //}
            ViewData["currentStaff"] = s;
            return Redirect("https://localhost:44372/Staff/StaffMain");
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

        // GET: Trip/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = _db.trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var trip = _db.trips.Where(t => t.TripId == id).FirstOrDefault();
            var users = _db.usertrips.Where(a => a.TripId == id).ToList();
            if (users.Count == 0)
            {
                if (trip == null)
                {
                    return NotFound();
                }
                List<TripAtt> tours = _db.tripatt.Where(a => a.TripId == id).ToList();
                if (tours != null)
                {
                    for (int i = 0; i < tours.Count; i++)
                    {
                        _db.tripatt.Remove(tours[i]);
                        _db.SaveChanges();
                    }
                }
                _db.trips.Remove(trip);
                _db.SaveChanges();
                global.currentTrip = 0;
                
                return RedirectToAction(nameof(Index));
            }
            global.checkIndex = false;
            return RedirectToAction(nameof(Index));

        }


        // GET: Trip/Edit/5
        public IActionResult Edit(int id)
        {
            var trip = _db.trips.Find(id);
            ViewBag.Buses = _db.buses.Select(b => b.BusID).ToList();
            if (trip == null)
            {
                return NotFound();
            }
            return View(trip);
        }

        // POST: Trip/Edit/5
        // POST: Trip/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Trip t, int id, [Bind("TripId,Name,FromLocation,StartDate,EndDate,City,Price,Description,BusID,staff")] Trip trip, IFormFile ProfilePicture)
        {
            var existing = _db.trips.FirstOrDefault(s => s.TripId == global.currentTrip);

            if (id != trip.TripId)
            {
                return NotFound();
            }
            string path = Path.Combine(_host.WebRootPath, "images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (ProfilePicture != null)
            {

                var fileExtension = Path.GetExtension(ProfilePicture.FileName).ToLower();
                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
                {
                    ModelState.AddModelError("ImageUser", " InValid Photo ");
                    return View("Profile", t);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);

                path = Path.Combine(path, fileName);
                using (var Stream = new FileStream(path, FileMode.Create))
                {
                    ProfilePicture.CopyTo(Stream);
                    t.ImageUser = fileName;
                    existing.ImageUser = t.ImageUser;

                }
            }
            else
            {
                t.ImageUser = t.ImageUser ?? "default.png";
            }

            try
            {
                _db.Entry(trip).State = EntityState.Modified;
                _db.trips.Update(existing);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(trip.TripId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return View(trip);
        }

        private bool TripExists(int tripId)
        {
            throw new NotImplementedException();
        }

        //display trips user mode
        public IActionResult DisplayTrips()
        {
            List<Trip> t;
                t = _db.trips.ToList();
            
            ViewData["tripsall"] = t;
            return View();
        }


        // Attach a tourist attraction to a trip

        [HttpGet]
        public IActionResult AttachToTourist(int id)
        {
            var tripatt = _db.tripatt.Where(a => a.TripId == id).Select(a => a.TouristAttractionId).ToList();
            var tours = _db.tourists.Where(a => !tripatt.Contains(a.Id)).ToList();

            global.currentTrip = id;
            ViewData["tours"] = tours;
            return View(tours);
        }

        [HttpPost]
        public IActionResult AttachToTourist(int? selectedTourIds)
        {
            if (selectedTourIds == null)
            {
                return new NotFoundResult();
            }

            var tripatt = _db.tripatt.Where(a => a.TripId == global.currentTrip)
                               .OrderByDescending(a => a.Order)
                               .FirstOrDefault(); global.currentTrip = global.currentTrip;

            var u = _db.tourists.Where(a => a.Id == selectedTourIds).FirstOrDefault();

            if (u == null)
            {
                return new NotFoundResult();
            }
            var t = _db.trips.Where(a => a.TripId == global.currentTrip).FirstOrDefault();

            TripAtt ta = new TripAtt();
            ta.TripId = t.TripId;
            ta.Trip = t;
            ta.TouristAttractionId = u.Id;
            ta.TouristAttraction = u;
            if (tripatt == null)
            {
                ta.Order = 1;
            }
            else
            {
                ta.Order = tripatt.Order + 1;
            }
            _db.tripatt.Add(ta);
            _db.SaveChanges();

            return RedirectToAction(nameof(AttachToTourist), new { id = global.currentTrip });
        }

        // show tourists attractions for the trip
        [HttpGet]
        public IActionResult TouristsForTrip(int? id)
        {
            if (id == null && global.currentTrip != null)
            {
                id = global.currentTrip;
            }
            var tripatt = _db.tripatt.Where(a => a.TripId == id).Select(a => a.TouristAttractionId).ToList();
            var tours = _db.tourists.Where(a => tripatt.Contains(a.Id)).ToList();
            return View(tours);
        }

        //Redirect to another action in another controller
        public IActionResult EditTouristGo(int idt)
        {
            return RedirectToAction("Update", "Touristattraction", new { id = idt });
        }

        // remove the connection between this trip and the tourist attraction
        public IActionResult removeFromTrip(int id)
        {
            var u = _db.tripatt.Where(a => a.TouristAttractionId == id && a.TripId == global.currentTrip).FirstOrDefault();
            if (u != null)
            {
                _db.tripatt.Remove(u);
                _db.SaveChanges();
            }
            return RedirectToAction("TouristsForTrip");
        }

        // View Trip // user mode
        public IActionResult ViewTrip(int id)
        {
            ViewData["wrongUser"] = true;
            global.currentTrip = id;
            var tripatt = _db.tripatt.Where(a => a.TripId == id).Select(a => a.TouristAttractionId).ToList();
            var tours = _db.tourists.Where(a => tripatt.Contains(a.Id)).ToList();
            ViewData["TripId"] = id;
            ViewData["tourists"] = tours;
            return View();
        }
    }
}
