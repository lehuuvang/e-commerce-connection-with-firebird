using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qlbanhang.Model1s;
using qlbanhang.Models;
using System.Collections.Immutable;
using System.Diagnostics;

namespace qlbanhang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DNorthwindFdbContext _employeeFdbContext;

        public HomeController(ILogger<HomeController> logger, DNorthwindFdbContext employeeFdbContext)
        {
            _logger = logger;
            _employeeFdbContext = employeeFdbContext;
        }

        public IActionResult Index()
        {
            return View(_employeeFdbContext.Categories.ToListAsync()    );
        }
            
        public IActionResult Privacy()
        {
            var list = from Country in _employeeFdbContext.Categories select Country.CategoryId;
            return View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}