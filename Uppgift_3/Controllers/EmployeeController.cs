using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uppgift_3.Data;
using Uppgift_3.Models;

namespace Uppgift_3.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;

        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }

        // GET: Driver/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Driver/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Employee employee)
        {
            if (ModelState.IsValid) // Checks if the model passes validation rules
            {
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();

                // Store a success message in TempData
                TempData["SuccessMessage"] = $"User {employee.EmployeeName} was created successfully! IsAdmin = {employee.IsAdmin}";

                return RedirectToAction("Index", "Home"); // Redirect to the Index page after saving
            }

            // If validation fails, the form is shown again with error messages
            return View(employee);
        }
    }
}