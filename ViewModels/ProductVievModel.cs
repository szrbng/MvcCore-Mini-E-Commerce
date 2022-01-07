using Market.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Market.WebUI.ViewModels
{
    public class ProductVievModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

      

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [NotMapped]
        public IFormFile UploadImage { get; set; }
    }
}
