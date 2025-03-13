using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebRuou.Models;

namespace WebRuou.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        DBRuouEntities db = new DBRuouEntities();
        // GET: Admin/Order
        public ActionResult Index( int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);

            var orders = db.Orders.ToList().ToPagedList(pageNum, pageSize);
            return View(orders);
        }
        public ActionResult UpdateStatus(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            if (order.Status == "Đang giao")
            {
                order.Status = "Hoàn thành";
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }
        public ActionResult Edit(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var existingOrder = db.Orders.Find(order.OrderID);
                if (existingOrder == null)
                {
                    return HttpNotFound();
                }

                // Không cho phép chỉnh sửa nếu đơn hàng đã hoàn thành
                if (existingOrder.Status == "Hoàn thành")
                {
                    ModelState.AddModelError("", "Không thể chỉnh sửa đơn hàng đã hoàn thành.");
                    return View(order);
                }

                // Cập nhật trạng thái đơn hàng
                existingOrder.Status = order.Status;
                existingOrder.TotalAmount = order.TotalAmount;
                existingOrder.OrderDate = order.OrderDate;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            if (order.Status == "Đang giao")
            {
                db.Orders.Remove(order);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}