using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SipaWeb.Models;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
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

        public async Task<IActionResult> IndexAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:44391/api/Lugares");

            if (response.IsSuccessStatusCode)
            {
                var lugaresData = await response.Content.ReadAsAsync<List<Lugares>>();
                return View(lugaresData);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Clientes()
        {
            return View();
        }

        public IActionResult ClientesDescripcion()
        {
            return View();
        }

        public IActionResult Inicio()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ProcesarFormulario(Clientes viewModel)
        {
            if (ModelState.IsValid)
            {
                // Crear una nueva instancia de la entidad Clientes y asignar valores desde el ViewModel
                var cliente = new Clientes
                {
                    Ciudad = viewModel.Ciudad,
                    Dias = viewModel.Dias,
                    Presupuesto = viewModel.Presupuesto,
                    Adultos =viewModel.Adultos,
                    Ninos= viewModel.Ninos,
                    Clima = viewModel.Clima,
                  //  Intereses= viewModel.Intereses,

                    // Asigna otras propiedades según corresponda
                };

                string[] interesesSeleccionados = viewModel.InteresesSeleccionados;
                cliente.Intereses = string.Join(", ", interesesSeleccionados);


                if (viewModel.Intereses != null && viewModel.Intereses.Length > 0)
                {
                    cliente.Intereses = Request.Form["InteresesConcatenados"];
                }

                // Realizar una solicitud POST para almacenar el cliente en el endpoint de la API
                using (var httpClient = new HttpClient())
                {
                    // Especifica la URL del endpoint
                    var apiUrl = "https://localhost:44391/api/Clientes";

                    // Realiza la solicitud POST enviando el objeto cliente
                    var response = await httpClient.PostAsJsonAsync(apiUrl, cliente);

                    if (response.IsSuccessStatusCode)
                    {

                        // La solicitud fue exitosa, puedes redirigir a una vista de confirmación
                        //   return RedirectToAction("Confirmacion");
                    }
                    else
                    {
                        // La solicitud no fue exitosa, maneja el error según sea necesario
                        // Por ejemplo, muestra un mensaje de error al usuario
                    }
                }
            }

            // Si la validación falla, vuelve a mostrar el formulario con errores
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ProcesarClienteDescripcion(Clientes viewModel)
        {

            if (ModelState.IsValid)
            {
                // Crear una nueva instancia de la entidad Clientes y asignar valores desde el ViewModel
                var cliente = new Clientes
                {
                    Descripcion = viewModel.Descripcion,

                };

                // Realizar una solicitud POST para almacenar el cliente en el endpoint de la API
                using (var httpClient = new HttpClient())
                {
                    // Especifica la URL del endpoint
                    var apiUrl = "https://localhost:44391/api/Clientes";

                    // Realiza la solicitud POST enviando el objeto cliente
                    var response = await httpClient.PostAsJsonAsync(apiUrl, cliente);

                    if (response.IsSuccessStatusCode)
                    {
                        string objetoSerializado = Newtonsoft.Json.JsonConvert.SerializeObject(cliente);
                        var apiRecomendacion = "http://127.0.0.1:5000/obtener_objeto_de_api_externa";
                        string urlConParametro = $"{apiRecomendacion}?parametro={objetoSerializado}";
                       
                        var responseRecomendacion = await httpClient.GetAsync(urlConParametro);

                        if (responseRecomendacion.IsSuccessStatusCode)
                        {
                            string responseBody = await responseRecomendacion.Content.ReadAsStringAsync();


                            if (responseBody.StartsWith("["))
                            {
                                var objetosRecibidos = JsonConvert.DeserializeObject<List<Lugares>>(responseBody);
                                return View("LugarRecomendado", objetosRecibidos);
                                // Trabajar con la lista de objetos
                            }
                            else
                            {
                                var objetosRecibido = JsonConvert.DeserializeObject<Lugares>(responseBody);
                                return View("LugarRecomendado", objetosRecibido);
                                // Trabajar con el objeto individual
                            }
                        }

                        // La solicitud fue exitosa, puedes redirigir a una vista de confirmación
                        //   return RedirectToAction("Confirmacion");
                    }
                    else
                    {
                        // La solicitud no fue exitosa, maneja el error según sea necesario
                        // Por ejemplo, muestra un mensaje de error al usuario
                    }
                }
            }

            // Si la validación falla, vuelve a mostrar el formulario con errores
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}