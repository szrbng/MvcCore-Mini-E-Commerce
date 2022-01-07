using Market.Entities;
using Market.Repo.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public APIController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpGet]
        public JsonResult GetProduct()
        {
            return new JsonResult(_productRepository.Products.ToList());
        }


        [HttpPost]
        public IActionResult CreateProduct(Product model)
        {
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    _productRepository.CreateProduct(model);

                }
            }

            return Ok();
        }
    }
}
