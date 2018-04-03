
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly ProductDBEntities _db = new ProductDBEntities();

        [HttpGet]
        public ActionResult OrderProduct()
        {
            return View(_db.OrderStatus.Where(s => s.Status).ToList());
        }

     
        public ActionResult AddOrder(int id)
        {
            try
            {
                Orders order = new Orders();
                order.ProductID = id;
                order.OrderTime = DateTime.Now;
                order.IsStored = false;

                _db.Orders.Add(order);
                _db.SaveChanges();

                OrderStatus os = new OrderStatus();
                os.OrderID = order.OrderID;
                os.Status = true;

                _db.OrderStatus.Add(os);
                _db.SaveChanges();

                ViewBag.Message ="<div class='alert alert-success'><button type='button'class='close'data-dismiss='alert'>×</button><strong>Success!</strong>Ordered</div>";
            }
            catch
            {
                ViewBag.Message = "<div class='alert alert-alert'><button type='button'class='close'data-dismiss='alert'>×</button><strong>Error!</strong> Error.</div><br>";
            }
            return RedirectToAction("OrderProduct", "Order");
        }
        
        public ActionResult CancelOrder(int id)
        {
            try
            {
                var orderStatus = _db.OrderStatus.FirstOrDefault(o => o.OrderID == id);
                var order = _db.Orders.Find(id);
                
                if (orderStatus != null)
                {
                    if(order.IsStored == true)
                    {
                        ViewBag.Message =
                        "<div class='alert alert-alert'><button type='button'class='close'data-dismiss='alert'>×</button><strong>HATA!</strong>You cant cancel your product because It is in the store process</div><br>";
                        return RedirectToAction("OrderProduct", "Order");
                    }

                    orderStatus.Status = false;
                    _db.OrderStatus.Attach(orderStatus);
                    _db.Entry(orderStatus).State = EntityState.Modified;
                    _db.SaveChanges();

                    ViewBag.Message =
                        "<div class='alert alert-success'><button type='button'class='close'data-dismiss='alert'>×</button><strong>SUCCES!</strong> Order has been Cancelled.</div>";
                }
            }
            catch
            {
                ViewBag.Message =
                    "<div class='alert alert-alert'><button type='button'class='close'data-dismiss='alert'>×</button><strong>ERROR!</strong>You cant cancel your product because It is in the store process</div><br>";
                return RedirectToAction("OrderProduct", "Order");
            }
            return RedirectToAction("OrderProduct", "Order");
        }

        public ActionResult StoreAllOrder()
        {
            try
            {
                var orders = _db.Orders.ToList();

                foreach (var order in orders)
                {
                    if(order.IsStored != true)
                    {
                        order.IsStored = true;
                        _db.Orders.Attach(order);
                        _db.Entry(order).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                }
            }
            catch
            {
                ViewBag.Message =
                    "<div class='alert alert-alert'><button type='button'class='close'data-dismiss='alert'>×</button><strong>HATA!</strong>You cant cancel your product because It is in the store process</div><br>";
            }
            return RedirectToAction("Index", "Stock");
        }




    }
    }
