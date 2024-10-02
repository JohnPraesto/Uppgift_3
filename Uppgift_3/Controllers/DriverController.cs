using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uppgift_3.Data;
using Uppgift_3.Models;

namespace Uppgift_3.Controllers
{
    public class DriverController : Controller
    {
        private readonly AppDbContext _db;

        public DriverController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult DriverIndex()
        {
            var drivers = _db.Drivers.ToList(); // Fetch all drivers from the database
            return View(drivers);
        }

        public IActionResult CreateDriver()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDriver(Driver driver)
        {
            if (ModelState.IsValid)
            {
                _db.Drivers.Add(driver);
                _db.SaveChanges();
                return RedirectToAction(nameof(DriverIndex));
            }
            return View(driver);
        }

        public IActionResult Delete(int? id)
        {
            var driver = _db.Drivers.Find(id);
            _db.Drivers.Remove(driver);
            _db.SaveChanges();
            return RedirectToAction("DriverIndex");
        }

        public IActionResult Details(int id)
        {
            var driver = _db.Drivers
                .Include(d => d.Incidents) // Include related incidents
                .FirstOrDefault(d => d.DriverID == id);

            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var driver = _db.Drivers.FirstOrDefault(d => d.DriverID == id);

            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Driver updatedDriver)
        {
            if (id != updatedDriver.DriverID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var driver = _db.Drivers.FirstOrDefault(d => d.DriverID == id);
                if (driver == null)
                {
                    return NotFound();
                }

                driver.DriverName = updatedDriver.DriverName; // Update only the name
                _db.SaveChanges();

                return RedirectToAction(nameof(DriverIndex));
            }

            return View(updatedDriver);
        }

        [HttpGet]
        public IActionResult AddIncident(int id)
        {
            var driver = _db.Drivers.FirstOrDefault(d => d.DriverID == id);

            if (driver == null)
            {
                return NotFound();
            }

            var incident = new Incident
            {
                DriverID = id, // Pre-fill the DriverID for the new incident
                TimeOfIncident = DateTime.Now // Set default time as current time (optional)
            };

            return View(incident);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddIncident(Incident newIncident)
        {
            if (ModelState.IsValid)
            {
                _db.Incidents.Add(newIncident);
                _db.SaveChanges();

                return RedirectToAction("Details", new { id = newIncident.DriverID });
            }

            return View(newIncident);
        }


    }
}
