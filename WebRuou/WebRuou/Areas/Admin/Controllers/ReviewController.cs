using System;
using System.Linq;
using System.Web.Mvc;
using WebRuou.Models;
using PagedList;

namespace WebRuou.Areas.Admin.Controllers
{
    public class ReviewController : Controller
    {
        DBRuouEntities db = new DBRuouEntities();

        public ActionResult Index(int? page)
        {
            int pageSize = 10; // Số đánh giá trên mỗi trang
            int pageNumber = (page ?? 1);

            var reviews = db.Reviews
                            .OrderByDescending(r => r.ReviewDate)
                            .ToPagedList(pageNumber, pageSize);

            return View(reviews);
        }
    }
}
