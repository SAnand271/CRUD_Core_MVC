using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }


        // GET: EmployeeController
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }




        // Employee Details
        public async Task<IActionResult> Details(int? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }




        //AddOrEdit Get Method
        public async Task<IActionResult> AddOrEdit(int? employeeId)
        {
            ViewBag.PageName = employeeId == null ? "Create Employee" : "Edit Employee";
            ViewBag.IsEdit = employeeId == null ? false : true;
            if (employeeId == null)
            {
                return View();
            }
            else
            {
                var employee = await _context.Employees.FindAsync(employeeId);

                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
        }

        //AddOrEdit Post Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int employeeId, [Bind("EmployeeId,Name,Designation,Address,Salary,JoiningDate")]
Employees employeeData)
        {
            bool IsEmployeeExist = false;

            Employees employee = await _context.Employees.FindAsync(employeeId);

            if (employee != null)
            {
                IsEmployeeExist = true;
            }
            else
            {
                employee = new Employees();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employee.Name = employeeData.Name;
                    employee.Designation = employeeData.Designation;
                    employee.Address = employeeData.Address;
                    employee.Salary = employeeData.Salary;
                    employee.JoiningDate = employeeData.JoiningDate;

                    if (IsEmployeeExist)
                    {
                        _context.Update(employee);
                    }
                    else
                    {
                        _context.Add(employee);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeData);
        }




        //// GET: EmployeeController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: EmployeeController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: EmployeeController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: EmployeeController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: EmployeeController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: EmployeeController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}



        // GET: Employees/Delete/1
        public async Task<IActionResult> Delete(int? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == employeeId);

            return View(employee);
        }

        // POST: Employees/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
