using EmployeeLeaveManagementSys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeLeaveManagementSys.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
        public IActionResult TestError()
        {
            // Previously threw an exception for testing. Return the Error view instead
            // so the request doesn't produce an unhandled exception while debugging.
            return RedirectToAction("Error");
        }
    }
}