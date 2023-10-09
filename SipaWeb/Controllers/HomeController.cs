using Microsoft.AspNetCore.Mvc;
using SipaWeb.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SipaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async  Task<IActionResult> IndexAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:44391/api/Lugares");

            if (response.IsSuccessStatusCode)
            {
                var lugaresData = await response.Content.ReadAsAsync<List<Lugares>>(); // Reemplaza "LugaresData" con el tipo de tu modelo de datos
                return View(lugaresData);
            }

            return View(); // Manejar el caso de error aquí
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}