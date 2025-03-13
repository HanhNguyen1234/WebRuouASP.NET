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
        DBRuouEntities db = new DBRuouEntities();

        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            int userId = 1; // Giả định user đăng nhập, sau này thay bằng Session hoặc Identity
            var cartItems = db.Carts.Where(c => c.UserID == userId).ToList();
            return View(cartItems);
        }
      /*  public int GetCartCount()
        {
            // Kiểm tra nếu User chưa đăng nhập, trả về 0
            if (Session["UserID"] == null)
                return 0;

            int userId = (int)Session["UserID"]; // Lấy UserID từ Session
            return db.Carts.Where(c => c.UserID == userId).Sum(c => (int?)c.Quantity) ?? 0;
        }*/


        // Thêm sản phẩm vào giỏ
        public ActionResult AddToCart(int productId)
        {
            int userId = 1; // Thay bằng user đăng nhập

            var cartItem = db.Carts.FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
            }
            else
            {
                db.Carts.Add(new Cart { UserID = userId, ProductID = productId, Quantity = 1 });
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng sản phẩm
        [HttpPost]
        public ActionResult UpdateCart(int cartId, int quantity)
        {
            var cartItem = db.Carts.Find(cartId);
            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ
        public ActionResult RemoveFromCart(int cartId)
        {
            var cartItem = db.Carts.Find(cartId);
            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Chuyển giỏ hàng thành đơn hàng
        public ActionResult Checkout()
        {
            int userId = 1; // Thay bằng user đăng nhập
            var cartItems = db.Carts.Where(c => c.UserID == userId).ToList();

            if (cartItems.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn trống!";
                return RedirectToAction("Index");
            }

            var newOrder = new Order
            {
                UserID = userId,
                OrderDate = System.DateTime.Now,
                Status = "Chờ xác nhận",
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                OrderDetails = cartItems.Select(c => new OrderDetail
                {
                    ProductID = c.ProductID,
                    Quantity = c.Quantity,
                    Price = c.Product.Price
                }).ToList()
            };

            db.Orders.Add(newOrder);
            db.Carts.RemoveRange(cartItems);
            db.SaveChanges();

            TempData["Success"] = "Đơn hàng của bạn đã được tạo!";
            return RedirectToAction("Index", "Order");
        }
    }
}