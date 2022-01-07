using Market.Entities;
using Market.Repo.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }

       /* [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }*/

       
        
        
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some products first!");
            }

            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            ViewBag.Message = "Siparişinizi onaylamak için lütfen giriş yapınız.";

            return View(order);
        }

       
        public IActionResult CheckoutComplete()
        {
            ViewBag.Message = "Siparişiniz Tamamlandı";
            return View();
        }
        public IActionResult OrderList()
        {

          

            return View();
        }

        
    }
}
