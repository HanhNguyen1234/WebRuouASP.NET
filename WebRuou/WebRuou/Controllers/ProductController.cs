﻿using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using WebRuou.Models;

namespace WebRuou.Controllers
{
    public class ProductController : Controller
    {
        private DBRuouEntities db = new DBRuouEntities();

        public ActionResult Index()
        {
            // Lấy danh sách danh mục kèm theo số lượng sản phẩm
            var categories = db.Categories
                .Include(c => c.Products)
                .ToList();

            // Lấy danh sách sản phẩm
            var products = db.Products
                .Include(p => p.Category)
                .ToList();

            // Lưu vào ViewBag để hiển thị trên View
            ViewBag.Categories = categories;
            ViewBag.Products = products;

            return View();
        }
        public ActionResult PartialSameProduct(int categoryId)
        {
            var relatedProducts = db.Products
                .Where(p => p.CategoryID == categoryId)
                .ToList();

            return PartialView(relatedProducts);
        }

        public ActionResult ProductDetail(int id)
        {
            // Retrieve the product by ID
            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound(); // Return 404 if product not found
            }

            return View(product); // Pass the product to the view
        }
    }
}


