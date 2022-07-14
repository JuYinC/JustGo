using JustGo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JustGo.Controllers
{
    public class MapController : Controller
    {
        private readonly IPlaceWeatherRepostiory _pwr;

        public MapController( IPlaceWeatherRepostiory pwr)
        {
            _pwr = pwr;
        }

        public IActionResult Index()
        {
            string[] a = new string[] { "a" };
            string[] b = new string[] { "高雄市", "澎湖縣" };
            string[] c = new string[] { };
            int[] d = new int[] { 1,3,4,5 };
            //ViewBag.Json = JsonConvert.SerializeObject(_pwr.getPlaceFilter(a, b, c, d));
            ViewBag.Json = JsonConvert.SerializeObject(_pwr.getPlace(0,20000));
            //ViewBag.Json = Json(_pwr.getPlaceFilter(a, b, c, d));
            return View();
        }
    }
}
