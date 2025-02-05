using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopBackEnd_1.Models;

namespace WebShopBackEnd_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("AllProducts")]
        [Authorize(Roles = "User, Admin, Moderator")]
        public IActionResult AllProducts()
        {
            using (var context = new Webshop1Context())
            {
                try
                {
                    return Ok(context.Products.ToList());
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpGet("Product")]
        public IActionResult Product(int id)
        {
            using (var context = new Webshop1Context())
            {
                try
                {
                    return Ok(context.Products.ToList().Find(x => x.Id == id));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpPost("ProductPost")]
        public IActionResult ProductPost(Product product)
        {
            using (var context = new Webshop1Context())
            {
                try
                {
                    context.Products.Add(product);
                    context.SaveChanges();

                    return Ok("Product added!");
                    
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpDelete("ProductRemove")]
        public IActionResult ProductRemove(int id)
        {
            using (var context = new Webshop1Context())
            {
                try
                {
                    context.Products.Remove(context.Products.ToList().Find(x=> x.Id == id));
                    context.SaveChanges();

                    return Ok("Product added!");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
