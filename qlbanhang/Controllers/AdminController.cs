using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using qlbanhang.Model1s;
using qlbanhang.ViewModel;

namespace qlbanhang.Controllers
{
    public class AdminController : Controller
    {
        private readonly DNorthwindFdbContext _context;

        public AdminController(DNorthwindFdbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var viewModel = new AdminStatisticsViewModel
            {
                AccountCount = _context.Accounts.Count(),
                CategoriesCount = _context.Categories.Count(),
                SuppliersCount = _context.Suppliers.Count(),
                ProductCount = _context.Products.Count(),
            };
            return View(viewModel);
        }
    }
}
