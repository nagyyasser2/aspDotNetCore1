using aspDotNetCore.Authorization;
using aspDotNetCore.Data;
using aspDotNetCore.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace aspDotNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ProductsController(ApplicationDbContext dbContext, ILogger<ProductsController> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        // Create a new product
        [HttpPost]
        [Route("")]
        public ActionResult<int> CreateProduct(Product product)
        {
            _dbContext.Set<Product>().Add(product);
            _dbContext.SaveChanges();
            return Ok(product.Id);
        }

        // Get all products
        [HttpGet]
        [Route("")]
        [CheckPermission(Permission.ReadProducts)]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _dbContext.Set<Product>().ToList();
            return Ok(products);
        }

        // Get a product by ID
        [HttpGet]
        [Route("{key}")]
        [LogSensitiveAction]
        public ActionResult<Product> GetProduct([FromRoute(Name = "key")] int id)
        {
            _logger.LogDebug("trying  to get product with id#{id} ", id);
            var product = _dbContext.Set<Product>().Find(id);
            if (product == null)
            {
                _logger.LogWarning("Product #{id}, Not Found!", id);
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }

        // Update a product
        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var product = _dbContext.Set<Product>().Find(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            product.Name = updatedProduct.Name;
            product.Sku = updatedProduct.Sku;

            _dbContext.SaveChanges();
            return NoContent();
        }

        // Delete a product
        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _dbContext.Set<Product>().Find(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            _dbContext.Set<Product>().Remove(product);
            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}
