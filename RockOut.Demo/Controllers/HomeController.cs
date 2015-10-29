using System.Collections.Generic;
using System.Web.Mvc;
using RockOut.Demo.Models;

namespace RockOut.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = BuildModel(2);

            return View(model);
        }

        public JsonResult Async()
        {
            var model = BuildModel(4);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private ViewModel BuildModel(int numBullet)
        {
            var model = new ViewModel();

            model.Title = "Page title";

            model.BulletPoints = new List<ListElementModel>();

            for (int i = 0; i < numBullet; i++)
            {
                model.BulletPoints.Add(new ListElementModel { Label = "Element " + i, Value = i });
            };

            return model;
        }
    }
}
