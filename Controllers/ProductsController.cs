using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.DTO;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // private static readonly string[] Products={
        //     "samsung s6","samsung s7","samsung s8"
        // };






        private readonly SocialContext _socialcontext;
        public ProductsController(SocialContext socialContext)
        {
            _socialcontext = socialContext;
        }
        [AllowAnonymous]

        [HttpGet]
        public async Task<ActionResult> GetProducts()  //async method
        {
            var products = await
            _socialcontext.
            Products.
            // Select(s=>new ProductDTO()
            // {
            //     ProductId=s.ProductId,
            //     Name=s.Name,
            //     Price=s.Price,
            //     IsActive=s.IsActive
            // }).
            Select(p => ProductToDTO(p)).
            ToListAsync(); //await sayesinde contextten veri gelemden alt satıra geçmez
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            //     if(Products.Length-1<id)
            //     return "";
            // return Products[id];

            var d = await _socialcontext.Products.FirstOrDefaultAsync(s => s.ProductId == id);
            //var p = await _socialcontext
            //    .Products
            //    // .Select(s=>new ProductDTO()
            //    // {
            //    //     ProductId=s.ProductId,
            //    //     Name=s.Name,
            //    //     Price=s.Price,
            //    //     IsActive=s.IsActive
            //    // })
            //    .Select(p => ProductToDTO(p))
            //    .FirstOrDefaultAsync(i => i.ProductId == id);
            if (d == null)
                return NotFound();

            return Ok(d);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {

            _socialcontext.Add(entity);
            await _socialcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = entity.ProductId }, ProductToDTO(entity));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product entity)
        {
            if (id != entity.ProductId)
            {
                return BadRequest();
            }
            var product = await _socialcontext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Name = entity.Name;
            product.Price = entity.Price;

            try
            {
                await _socialcontext.SaveChangesAsync();

            }
            catch (Exception e)
            {

                return NotFound();
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _socialcontext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _socialcontext.Products.Remove(product);
            await _socialcontext.SaveChangesAsync();
            return NoContent();

        }
        private static ProductDTO ProductToDTO(Product p)
        {

            return new ProductDTO()
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                IsActive = p.IsActive
            };
        }
    }
}