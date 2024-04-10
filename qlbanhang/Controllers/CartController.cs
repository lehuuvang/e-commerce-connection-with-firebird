using Microsoft.AspNetCore.Mvc;
using qlbanhang.Model1s;
using qlbanhang.ViewModel;
using qlbanhang.Helpers;
using System.Runtime.CompilerServices;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace qlbanhang.Controllers
{
	public class CartController : Controller
	{
        public IConfiguration Configuration { get; set; }
        private readonly DNorthwindFdbContext _dbContext;
		private readonly INotyfService _notfy;
		public CartController(DNorthwindFdbContext _Context, INotyfService notfy, IConfiguration configuration)
		{
			_dbContext = _Context;
			_notfy = notfy;
            this.Configuration = configuration;
        }
		const string CART_KEY = "MYCART";
		public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
		public IActionResult Index()
		{
			return View(Cart);
		}
		public IActionResult addToCart(int id, int? quanlity = 1)//int? quanlity = 1
		{
			var cartCheck = Cart;
			var item = cartCheck.SingleOrDefault(p => p.ProductId == id);
			if (item == null)
			{
				var products = _dbContext.Products.SingleOrDefault(p => p.ProductId == id);
				if (products == null)
				{
					TempData["Message"] = $"Not found product";
					return Redirect("/404");
				}
				item = new CartItem
				{
					ProductId = products.ProductId,
					ProductName = products.ProductName,
					Picture = products.Picture,
					UnitPriceSS = (short?)products.UnitPrice,
					quantity = (short?)quanlity
				};
				cartCheck.Add(item);
			}
			else
			{
				item.quantity += (short?)quanlity;
			}
			HttpContext.Session.Set(CART_KEY, cartCheck);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult Checkout()
		{
			if (Cart.Count == 0)
			{
				return Redirect("/");
			}
			return View(Cart);
		}
		public Boolean Checkinput(string name , string address)
		{
			if(name == "" || address == "")
			{
				_notfy.Warning("Please fill information before ordering item!");
				return false;
			}
			return true;
		}
		[HttpPost]
		public ActionResult Checkout(orderViewModel model, string mail)
		{
            string host = this.Configuration.GetValue<string>("Smtp:Server");
            int port = this.Configuration.GetValue<int>("Smtp:Port");
            bool enableSsl = this.Configuration.GetValue<bool>("Smtp:EnableSsl");
            bool defaultCredentials = this.Configuration.GetValue<bool>("Smtp:DefaultCredentials");
            string from = this.Configuration.GetValue<string>("Smtp:From");
            string userName = this.Configuration.GetValue<string>("Smtp:Username");
            string password = this.Configuration.GetValue<string>("Smtp:Password");
            if (ModelState.IsValid)
			{
				if (HttpContext.Session.Get<string>("username") != null)
				{
					if (Checkinput(model.shipName, model.ShipAddress))
					{
						//default is user with account user123 because login don't have session
						var myaccid = _dbContext.Accounts.Where(p => p.Username == HttpContext.Session.Get<string>("username")).FirstOrDefault();
						var mycusid = _dbContext.Customers.Where(p => p.AccountId == myaccid.AccountId).FirstOrDefault();
						var order = new Order
						{					
							CustomerId = mycusid.CustomerId,
							OrderDate = DateTime.Now,
							ShipName = model.shipName ?? "Unknown",
							ShipAddress = model.ShipAddress ?? "Unknown",
							//ShipVia = model.ShipVia, errror , beacause the shipvia is primaty key
							ShippedDate = Convert.ToDateTime(model.ShipDate)
						};
                        #region
                        // mail block
                        //using (MailMessage mm = new MailMessage(from, mail))
                        //{
                        //    mm.Subject = "Thank for buy my store";
                        //    mm.Body = "this is a body";
                        //    mm.IsBodyHtml = false;
                        //    using (SmtpClient smtp = new SmtpClient())
                        //    {
                        //        smtp.Host = host;
                        //        smtp.EnableSsl = enableSsl;
                        //        NetworkCredential networkCred = new NetworkCredential(userName, password);
                        //        smtp.UseDefaultCredentials = defaultCredentials;
                        //        smtp.Credentials = networkCred;
                        //        smtp.Port = port;
                        //        smtp.Send(mm);
                        //    }
                        //}
                        #endregion
                        _dbContext.Database.BeginTransaction();
						try
						{
							_dbContext.Database.CommitTransaction();
							_dbContext.Add(order);
							_dbContext.SaveChanges();

							var OrderDetail = new List<OrderDetail>();
							foreach (var item in Cart)
							{
								OrderDetail.Add(new OrderDetail
								{
									OrderId = order.OrderId,
									Quantity = item.quantity ?? 0,
									UnitPrice = item.UnitPriceSS,
									Discount = (float)(item.UnitPriceSS * item.quantity),
									ProductId = item.ProductId,
									Status = 1 
									// user status default is "1" is processing  and admin is going to confirm include , "2 Finished" , "0" is reject
								});
							}                          
                            _dbContext.AddRange(OrderDetail);
							_dbContext.SaveChanges();                          
                            HttpContext.Session.Set<List<CartItem>>(CART_KEY, new List<CartItem>());
							_notfy.Success("Buying item success, please check your order");
							return Redirect("./success");
						}
						catch
						{
							_dbContext.Database.RollbackTransactionAsync();
						}
					}
				}
				else
				{
					_notfy.Error("Please login before buying item!");
					return Redirect("../Products/listProduct");
				}
			}
			return View(Cart);
		}
		public IActionResult success()
		{
			return View();
		}
	}
}
