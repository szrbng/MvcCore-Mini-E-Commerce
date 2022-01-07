using Market.Entities;
using Market.Repo.Abstract;
using Market.WebUI.Compenents;
using Market.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Market.WebUI.Controllers
{
    public class ShoppingCartController : Controller 
    {
        private readonly IProductRepository _productRepository;

        private readonly ShoppingCart _shoppingCart;

        private readonly AppDbContext _appDbContext;


        public ShoppingCartController(IProductRepository productRepository, ShoppingCart shoppingCart, AppDbContext appDbContext)
        {
            _productRepository = productRepository;
            _shoppingCart = shoppingCart;

            _appDbContext = appDbContext;
        }

        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppinCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }

        
        public IActionResult AddToShoppingCart(int productId)
        {
            var selectedProduct = _productRepository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (selectedProduct != null)
            {
                _shoppingCart.AddToCart(selectedProduct, 1);
            }
          
           return RedirectToAction("Index");
        }

        public IActionResult ClearShoppingCart()
        {
            _shoppingCart.ClearCart();

            

            return RedirectToAction("Index");
        }

        public IActionResult RemoveShoppingCartItem(int productId)
        {
            var selectedProduct = _appDbContext.Products.FirstOrDefault(p => p.ProductId == productId);

            if (selectedProduct != null)
            {
                _shoppingCart.RemoveFromCart(selectedProduct);
            }
            return RedirectToAction("Index");
        }

        
    }
}
