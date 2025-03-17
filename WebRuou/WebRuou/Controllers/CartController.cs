    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using WebRuou.Models;

    namespace WebRuou.Controllers
    {
        public class CartController : Controller
        {
            private DBRuouEntities db = new DBRuouEntities();

            // 🛒 Hiển thị giỏ hàng
            public ActionResult Index()
            {
                var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
                return View(cart);
            }

        // 🛍️ Thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int productID)
        {
            var product = db.Products.Find(productID);
            if (product == null)
            {
                return HttpNotFound("Sản phẩm không tồn tại!");
            }

            // Lấy giỏ hàng từ Session
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            var existingItem = cart.FirstOrDefault(item => item.ProductID == productID);
            if (existingItem != null)
            {
                existingItem.Quantity++; // Tăng số lượng nếu đã có
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.Name,
                    ProductPrice = product.Price ?? 0,
                    ImageURL = product.ImageURL,
                    Quantity = 1
                });
            }

            // Cập nhật giỏ hàng vào Session
            Session["Cart"] = cart;
            Session["CartCount"] = cart.Sum(i => i.Quantity); // 👈 Thêm dòng này

            return RedirectToAction("Index");
        }
        public JsonResult GetCartCount()
        {
            var cart = Session["Cart"] as List<CartItem>;
            int count = cart?.Sum(i => i.Quantity) ?? 0;
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        // 🗑️ Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int productID)
            {
                var cart = Session["Cart"] as List<CartItem>;
                if (cart != null)
                {
                    cart.RemoveAll(item => item.ProductID == productID);
                    Session["Cart"] = cart;
                }

                return RedirectToAction("Index");
            }

            // 🔄 Cập nhật số lượng sản phẩm
            [HttpPost]
            public ActionResult UpdateCart(int productID, int quantity)
            {
                var cart = Session["Cart"] as List<CartItem>;
                if (cart != null)
                {
                    var item = cart.FirstOrDefault(i => i.ProductID == productID);
                    if (item != null && quantity > 0)
                    {
                        item.Quantity = quantity;
                    }
                }

                Session["Cart"] = cart;
                return RedirectToAction("Index");
            }

            // 🛍️ Xóa toàn bộ giỏ hàng
            public ActionResult ClearCart()
            {
                Session["Cart"] = new List<CartItem>();
                return RedirectToAction("Index");
            }
        }
    }