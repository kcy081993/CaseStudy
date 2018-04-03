using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagement.Controllers
{
    public class StockController : Controller
    {
        private readonly ProductDBEntities _db = new ProductDBEntities();

        public ActionResult Index()
        {
            return View(_db.OrderStatus.Where(o => o.Orders.IsStored).ToList());
        }
    }
}