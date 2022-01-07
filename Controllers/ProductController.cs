using Market.Entities;
using Market.Repo.Abstract;
using Market.WebUI.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;

using System.IO;

using System.Linq;
using System.Threading.Tasks;

namespace Market.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICatagoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IProductRepository productRepository, ICatagoryRepository categoryRepository, IWebHostEnvironment hostEnvironment, AppDbContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;

            _context = context;

            this._hostEnvironment = hostEnvironment;
        }

        public ViewResult List(string category)
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
        public IActionResult ProductList()
        {

            return View(_productRepository.Products.ToList());
        }

        public IActionResult ProductList2()
        {

            return View(_productRepository.Products.ToList());
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.UploadImage.FileName);
                    string extension = Path.GetExtension(product.UploadImage.FileName);

                    product.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    string path = Path.Combine(wwwRootPath + "/img/", fileName);

                    using (var filestream = new FileStream(path, FileMode.Create))
                    {

                        await product.UploadImage.CopyToAsync(filestream);

                    }

                    _context.Products.Add(product);
                    _context.SaveChanges();

                    return RedirectToAction("ProductList2");
                }

            }
            catch (DataException)
            {

                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult UpdateProduct(int? id)
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");

            if (id == null)
            {
                return NotFound();
            }
            Product movie = _context.Products.Find(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }


        [HttpPost]
        public ActionResult UpdateProduct(Product movie, IFormFile file)
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    movie.ImageUrl = file.FileName;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                         file.CopyTo(stream);
                    }
                }

                _context.Entry(movie).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("ProductList2");
            }
            return View(movie);
        }

       
        public IActionResult Delete(int id)
        {
            var model = _productRepository.GetProductById(id);


            var ımagePath = Path.Combine(_hostEnvironment.WebRootPath, "img", model.ImageUrl);
             if (System.IO.File.Exists(ımagePath))
                 System.IO.File.Delete(ımagePath);

            if (model != null)
                {
                    _context.Entry(model).State = EntityState.Deleted; 
                    _context.SaveChanges();

                    return RedirectToAction("ProductList2");

            }

            return View(model);
          
        }

        
    }
}
