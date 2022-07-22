using JustGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustGo.Controllers
{
    public class MapController : Controller
    {
        // GET: MapController
        readonly IPlaceWeatherRepostiory _pwr;
        public MapController(IPlaceWeatherRepostiory rwp)
        {
            _pwr = rwp;
        }

        public ActionResult Index()
        {
            //string[] a = new string[]();
            //string[] a = new string[] { "a", "a" };
            //string[] b = new string[] { "高雄市" };
            //string[] c = new string[] { };
            //int[] d = new int[] { 1, 2, 3 };

            return View();
        }

        // GET: MapController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MapController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MapController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MapController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MapController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MapController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MapController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
