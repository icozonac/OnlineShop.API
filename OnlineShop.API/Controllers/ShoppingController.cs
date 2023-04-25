using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Controllers.DataAccess;
using OnlineShop.API.Models;


namespace OnlineShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        readonly IDataAccess dataAccess;
        private readonly string DateFormat;



        public ShoppingController(IDataAccess dataAccess, IConfiguration configuration)
        {
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }

        [HttpGet("GetProductCategories")]
        public IActionResult GetProductCategories()
        {
            var productCategories = dataAccess.GetProductCategories();
            return Ok(productCategories);
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts(string category, string subcategory, int count)
        {
            var products = dataAccess.GetProducts(category, subcategory, count);
            return Ok(products);
        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = dataAccess.GetProduct(id);
            return Ok(product);
        }

        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser([FromBody] User user)
        {
          user.CreatedDate = DateTime.Now.ToString(DateFormat);
          user.ModifiedDate = DateTime.Now.ToString(DateFormat);

          var result = dataAccess.InsertUser(user);


          string? message;
          if (result) message = "inserted";
          else message = "email not available";
          return Ok(message);
            
        }
    }
}
