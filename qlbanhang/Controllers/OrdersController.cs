using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using qlbanhang.Helpers;
using qlbanhang.Model1s;
using qlbanhang.ViewModel;

namespace qlbanhang.Controllers
{
    public class OrdersController : Controller
    {
        // GET: OrdersController
        private readonly DNorthwindFdbContext _context;
        public OrdersController(DNorthwindFdbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var loginsession = HttpContext.Session.Get<string>("username");
            var myAcc = _context.Accounts.Where(p => p.Username == loginsession).FirstOrDefault();
            var listUserOrder = (
                from order in _context.Orders
                join orderdt in _context.OrderDetails
                on order.OrderId equals orderdt.OrderId
                join cus in _context.Customers
                on order.CustomerId equals cus.CustomerId
                join prod in _context.Products
                 on orderdt.ProductId equals prod.ProductId
                join acc in _context.Accounts
                on cus.AccountId equals acc.AccountId
                orderby order.OrderDate descending
                select new orderViewModelforadmin
                {
                    orderid = order.OrderId,
                    CustomerName = cus.CompanyName,
                    orderDate = Convert.ToDateTime(order.OrderDate),
                    ShipDate = Convert.ToDateTime(order.ShippedDate),
                    shipName = order.ShipName,
                    customerid = order.CustomerId,
                    accountid = cus.AccountId ?? 1
                }
                );
            return View(listUserOrder.Distinct());
        }
        public ActionResult Index2(int id , int id2)
        {
            var listProductByUser =
                         (from orderdt in _context.OrderDetails
                          join order in _context.Orders
                           on orderdt.OrderId equals order.OrderId
                          join prod in _context.Products
                           on orderdt.ProductId equals prod.ProductId
                          join cus in _context.Customers
                          on order.CustomerId equals cus.CustomerId
                          where cus.AccountId == id
                          where orderdt.OrderId == id2
                          orderby orderdt.OrderId descending
                          select new orderdetailViewModel
                          {
                              OrderId = orderdt.OrderId,
                              ProductId = orderdt.ProductId,
                              ProductName = prod.ProductName,
                              Quantity = orderdt.Quantity,
                              Discount = orderdt.Discount,
                              UnitPrice = (short)orderdt.UnitPrice,
                              Status = (short)orderdt.Status
                          });
            return View(listProductByUser.Distinct());
        }
        // GET: OrdersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrdersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
