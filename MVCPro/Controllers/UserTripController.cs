using Microsoft.AspNetCore.Mvc;
using MVCPro.Models;
using Microsoft.EntityFrameworkCore;
using MVCPro.Data;
using System.Threading.Tasks;
using MVCPro.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCPro.Controllers
{
    public class UserTripController : Controller
    {
        private readonly AppDbContext _db;

        public UserTripController(AppDbContext _db)
        {
            this._db = _db;
        }

        // Index action to display user trips
        public async Task<IActionResult> Index()
        {
            var userTrips = await _db.usertrips
                .Include(ut => ut.Trip)
                .Include(ut => ut.User)
                .ToListAsync();

            return View(userTrips);
        }

        public async Task<IActionResult> Delete(string userId, int? tripId)
        {
            if (userId == null || tripId == null)
            {
                return NotFound();
            }

            var userTrip = await _db.usertrips
                .Include(ut => ut.Trip)
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TripId == tripId);

            if (userTrip == null)
            {
                return NotFound();
            }

            return View(userTrip);
        }

        // POST: UserTrip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId, int tripId)
        {
            var userTrip = await _db.usertrips
                .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TripId == tripId);

            if (userTrip == null)
            {
                return NotFound();
            }

            _db.usertrips.Remove(userTrip);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    var trip = _db.trips.Where(a => a.TripId == global.currentTrip).FirstOrDefault();
        //    ViewData["trip"] = trip;
        //    return View();
        //}
        // GET: UserTrip/Create
        //[//HttpPost]
        public IActionResult Create()
        {
            global.i += 1;
            //var uid = global.currentUser;
            //var tid = global.currentTrip;
            ViewData["wrongUser"] = true;
            global.checkIndex = true;
            var uid = global.currentUser;
            var tid = global.currentTrip;

            if (uid == null)
            {
                ViewData["wrongUser"] = false;
                global.checkIndex = false;
                //return new NotFoundResult();
                return RedirectToAction("ViewTrip", "Trip", new { id = tid });
            }
            var user = _db.Users.Where(a => a.NationalId == uid).FirstOrDefault();
            if (user == null)
            {
                ViewData["wrongUser"] = false;
                global.checkIndex = false;
                //return new EmptyResult();
                return RedirectToAction("ViewTrip", "Trip", new { id = tid });
            }
            //if (ModelState.IsValid)
            //{
                var userTrip = new UserTrip
                {
                    UserId = uid,
                    TripId = tid,
                };

                _db.usertrips.Add(userTrip);
                _db.SaveChanges();
                return RedirectToAction("BookingDone");
            //}

           // ViewData["wrongUser"] = false;
            //return RedirectToAction("ViewTrip", "Trip", new { id = tid });

        }

        public IActionResult BookingDone()
        {
            return View();
        }

    }
}
