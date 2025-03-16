using System.Linq;
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
    }
}


