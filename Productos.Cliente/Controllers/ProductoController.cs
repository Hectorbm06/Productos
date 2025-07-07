using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Productos.Cliente.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Productos.Cliente.Controllers
{
    public class ProductoController : Controller
    {
        private readonly HttpClient _httpClient;
        public ProductoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7248/api");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Productos/lista");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var productos = JsonConvert.DeserializeObject<IEnumerable<ProductoViewModel>>(content);
                return View("Index", productos);
            }
            return View(new List<ProductoViewModel>());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Producto producto)
        {
            if(ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Productos", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error al crear el producto");
            }
            return View(producto);
        }
    }
}
