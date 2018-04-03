using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagement.Controllers
{
    public class ProductController : Controller
    {

        private readonly ProductDBEntities _db = new ProductDBEntities();

        
        public ActionResult Index()
        {
            return View(_db.Products.Where(p => p.IsActive && p.IsDeleted == false).ToList());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Products product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (product != null)
                    {

                        var checkProduct = _db.Products.Where(p => p.IsActive && p.IsDeleted == false).FirstOrDefault(p => p.ProductName == product.ProductName);
                        if (checkProduct != null)
                        {
                            ViewBag.Message =
                                "<div class='alert alert-danger'><button type='button'class='close'data-dismiss='alert'>×</button><strong>ERROR!</strong> Product has already exist!</div>";
                            return View(product);
                        }

                        product.IsActive = true;
                        product.IsDeleted = false;

                        _db.Products.Add(product);
                        _db.SaveChanges();
                        ViewBag.Message =
                            "<div class='alert alert-success'><button type='button'class='close'data-dismiss='alert'>×</button><strong>SUCCESS!</strong> Product has succesfully added</div>";
                    }

                }
                return View(product);

            }
            catch
            {
                ViewBag.Message = "<div class='alert alert-alert'><button type='button'class='close'data-dismiss='alert'>×</button><strong>ERROR!</strong> Error.</div><br>";
            }
            return View();
        }

        public ActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _db.Products.Find(id);
                if (product != null)
                {
                    product.IsActive = false;
                    product.IsDeleted = true;

                    _db.Products.Attach(product);
                    _db.Entry(product).State = EntityState.Modified;
                    _db.SaveChanges();

                    ViewBag.Message =
                        "<div class='alert alert-success'><button type='button'class='close'data-dismiss='alert'>×</button><strong>SUCCES!</strong> Product has been deleted.</div>";
                }
            }
            catch
            {
                ViewBag.Message =
                    "<div class='alert alert-alert'><button type='button'class='close'data-dismiss='alert'>×</button><strong>Error!</strong></div><br>";
                return RedirectToAction("Index", "Product");
            }
            return RedirectToAction("Index", "Product");
        }
    }
}