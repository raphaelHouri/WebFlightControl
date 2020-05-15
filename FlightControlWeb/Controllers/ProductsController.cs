/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
       //injection - we should get it in the constructor not new
        private IProductManager productManager = new ProductsManger();
        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            return productManager.GetAllProducts();
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Products
        [HttpPost]
        public Product Post([FromBody] Product p)
        {
            p.Id = 5;
            // return CreatedAtAction(actionName: "GetItem", new { id = p.Id }, p);
            return p;
        }
      *//*  public Product AddProduct(Product p)
        {
            productManager.AddProduct(p);
            return p;
        }*/
            /*// POST: api/Products
            [HttpPost]
            public void Post([FromBody] string value)
            {
            }
    *//*
            // PUT: api/Products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


    }
}
*/