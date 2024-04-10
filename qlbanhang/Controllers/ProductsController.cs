using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using qlbanhang.Helpers;
using qlbanhang.Model1s;
using qlbanhang.ViewModel;
using SelectPdf;

namespace qlbanhang.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DNorthwindFdbContext _context;

        public ProductsController(DNorthwindFdbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var dNorthwindFdbContext = _context.Products.Include(p => p.Category).Include(p => p.Supplier);
            return View(await dNorthwindFdbContext.ToListAsync());
        }
		// Get: Products/listProduct
		public IActionResult listProduct()
		{
            var products = _context.Products.AsQueryable();
			var result = products.Select(p => new ProductViewModel
			{
				ProductID = p.ProductId,
				ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
				CategoryName = p.Category.CategoryName,
                Picture = p.Picture,
				Description = p.Category.Description
			});
			return View(result);
		}
        public IActionResult ShowProductWasBought()
        {
            var loginsession = HttpContext.Session.Get<string>("username");
            var myAcc = _context.Accounts.Where(p => p.Username == loginsession).FirstOrDefault();
            var listProductByUser =
             (from orderdt in _context.OrderDetails
              join order in _context.Orders
               on orderdt.OrderId equals order.OrderId
              join prod in _context.Products
               on orderdt.ProductId equals prod.ProductId
              join cus in _context.Customers
              on order.CustomerId equals cus.CustomerId
              join acc in _context.Accounts
              on cus.AccountId equals myAcc.AccountId
              orderby orderdt.OrderId descending
              select new orderdetailViewModel
              {
                  OrderId = orderdt.OrderId,
                  ProductId = orderdt.ProductId,
                  ProductName = prod.ProductName,
                  customerName = cus.CompanyName,
                  Quantity = orderdt.Quantity,
                  Discount = orderdt.Discount,
                  UnitPrice = (short)orderdt.UnitPrice,
                  Status = (short)orderdt.Status
              });
            #region
            // my code is fixing not work
            //var CheckOrder = _context.Orders.Any(p => p.CustomerId == myCus.CustomerId);
            //if (CheckOrder)
            //{
            //    var Order = _context.Orders.Where(p => p.CustomerId == myCus.CustomerId).FirstOrDefault();
            //    var listOrder = _context.OrderDetails.Where(p => p.OrderId == Order.OrderId).AsQueryable();
            //    if (listOrder != null)
            //    {
            //        TempData["myOrder"] = listOrder;
            //        var result = listOrder.Select(p => new orderdetailViewModel
            //        {
            //            OrderId = p.OrderId,
            //            ProductId = p.ProductId,
            //            Quantity = p.Quantity,
            //            Discount = p.Discount,
            //            UnitPrice = (short)p.UnitPrice,
            //            Status = (short)p.Status
            //        });
            //        return View(result);
            //    }
            //}
            //else
            //{
            //    TempData["myOrder"] = null;
            //}
            //return View();
            //         var prodBought = _context.OrderDetails.AsQueryable();
            //var result = prodBought.Select(p => new orderdetailViewModel
            //{
            //	OrderId = p.OrderId,
            //             ProductId = p.ProductId,
            //             Quantity = p.Quantity,
            //             Discount = p.Discount,
            //             UnitPrice = (short)p.UnitPrice,
            //             Status = (short)p.Status
            //});
            //return View(result);
            #endregion
            TempData["myOrder"] = listProductByUser.Distinct();
            return View(listProductByUser.Distinct());
        }
        [HttpPost]
        public IActionResult GeneratePDF(string html)
        {
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");
            HtmlToPdf oHtmlToPdf = new HtmlToPdf();
            PdfDocument opdfDocument = oHtmlToPdf.ConvertHtmlString(html);
            byte[] pdf = opdfDocument.Save();
            opdfDocument.Close();

            return File(
                    pdf,
                    "application/pdf",
                    "CustomerBill.pdf"
                );
        }
		//Product/searchProduct
		public IActionResult searchProduct(string? query)
		{
			var products = _context.Products.AsQueryable();
			if (query != null)
			{
				products = products.Where(p => p.ProductName.Contains(query));
			}
			var result = products.Select(p => new ProductViewModel
			{
				ProductID = p.ProductId,
				ProductName = p.ProductName,
				UnitPrice = p.UnitPrice,
				CategoryName = p.Category.CategoryName,
				Picture = p.Picture,
				Description = p.Category.Description
			});
			return View(result);
		}
		public IActionResult detailProduct(int id)
		{
			var detailProduct = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
			if (detailProduct == null)
			{
				return Redirect("/404");
			}
			var result = new DetailProductViewModel
			{
				ProductID = detailProduct.ProductId,
				ProductName = detailProduct.ProductName,
                UnitPrice = detailProduct.UnitPrice,
                Picture = detailProduct.Picture,
				QuantityUnit = detailProduct.QuantityPerUnit,
				CategoryName = detailProduct.Category.CategoryName,
				Description = detailProduct.Category.Description
			};
			return View(result);
		}
		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var suppliers = _context.Suppliers.Select(s => new SelectListItem
            {
                Value = s.SupplierId.ToString(),
                Text = s.CompanyName
            }).ToList();
            ViewBag.Suppliers = suppliers;

            var categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();
            ViewBag.Categories = categories;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DNorthwindFdbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
