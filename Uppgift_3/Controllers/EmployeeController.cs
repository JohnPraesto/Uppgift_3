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

        public IActionResult EmployeeIndex(string searchString)
        {
            var employee = _db.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                employee = employee.Where(d => d.EmployeeName.Contains(searchString));
            }

            return View(employee.ToList());
        }

        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(employee);
                _db.SaveChanges();
                return RedirectToAction(nameof(EmployeeIndex));
            }
            return View(employee);
        }

        public IActionResult Details(int id)
        {
            var employee = _db.Employees.FirstOrDefault(d => d.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _db.Employees.FirstOrDefault(d => d.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee updatedEmployee)
        {
            if (id != updatedEmployee.EmployeeId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var employee = _db.Employees.FirstOrDefault(d => d.EmployeeId == id);
                if (employee == null)
                {
                    return NotFound();
                }

                employee.EmployeeName = updatedEmployee.EmployeeName;
                employee.Password = updatedEmployee.Password;
                employee.IsAdmin = updatedEmployee.IsAdmin;

                _db.SaveChanges();

                return RedirectToAction(nameof(EmployeeIndex));
            }

            return View(updatedEmployee);
        }

        public IActionResult Delete(int? id)
        {
            var employee = _db.Employees.Find(id);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
            return RedirectToAction("EmployeeIndex");
        }
    }
}