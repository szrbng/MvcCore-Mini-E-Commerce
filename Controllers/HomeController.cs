using Market.Entities;
using Market.Repo.Abstract;
using Market.WebUI.Models;
using Market.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Market.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IProductRepository _productRepository;
        private readonly ICatagoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICatagoryRepository categoryRepository)
        {
            _logger = logger;

            _productRepository = productRepository;

            _categoryRepository = categoryRepository;

        }

        /*public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                FeaturedProducts = _productRepository.FeaturedProducts
            };

            return View(homeViewModel);
        }*/

        public ViewResult Index(string category)
        {
            IEnumerable<Product> products;
            string currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                products = _productRepository.Products.OrderBy(p => p.Name);
                currentCategory = "All";
            }
            else
            {
                {
                    products = _productRepository.Products.Where(p => p.Category.CategoryName == category)
                      .OrderBy(p => p.Name);
                    currentCategory = _categoryRepository.GetCategories()
                      .FirstOrDefault(c => c.CategoryName == category)
                      ?.CategoryName;
                }
            }

            return View(new ProductListViewModel
            {
                Products = products,
                CurrentCategory = currentCategory
            });
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
