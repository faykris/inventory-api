using InventoryAPI.Data;
using InventoryAPI.Models.Dtos;
using InventoryAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ProductController(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            List<Product> allProducts = dbContext.Products.ToList();

            return Ok(allProducts);
            
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsByName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Ok(dbContext.Products.ToList());
            }
            String upperCaseName = name.ToUpper();

            List<Product> products = await dbContext.Products
                .Where(p => EF.Functions.Like(p.Name.ToUpper(), $"%{upperCaseName}%"))
                .ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        public IActionResult AddProduct(AddProductDto addProductDto)
        {
            List<Product> productList = [];
      
            for (int i = 0; i < addProductDto.Quantity; i++)
            {
                productList.Add(new Product {
                    Name = addProductDto.Name,
                    Price = addProductDto.Price,
                    Elaboration = addProductDto.Elaboration,
                    ImageUrl = addProductDto.ImageUrl,
                    Status = 1
                });
            }

            dbContext.Products.AddRange(productList);
            dbContext.SaveChanges();

            return Ok(productList);
        }

        [HttpPut]
        [Route("info/{id:int}")]
        public IActionResult UpdateInfoProduct(int id, UpdateInfoProductDto updateInfoProductDto)
        {
            Product? product = dbContext.Products.Find(id);

            if (product is null)
            {
                return NotFound();
            }

            product.Name = updateInfoProductDto.Name;
            product.Price = updateInfoProductDto.Price;
            product.Elaboration = updateInfoProductDto.Elaboration;
            product.ImageUrl = updateInfoProductDto.ImageUrl;
            product.Updated = DateTime.Now;

            dbContext.SaveChanges();

            return Ok(product);
        }

        [HttpPut]
        [Route("defectives")]
        public IActionResult MarkAsDefectiveProducts(MarkedIdsProductDto markedIdsProductDto)
        {
            List<int> productIds = markedIdsProductDto.ProductIdList;

            var products = dbContext.Products.ToList()
                .Where(product => productIds.Contains(product.Id));


            foreach (Product product in products)
            {
                product.Status = 2;
                product.Updated = DateTime.Now;
            }

            dbContext.SaveChanges();

            return Ok(products);
        }

        [HttpPut]
        [Route("shipping")]
        public IActionResult ShippingProducts(MarkedIdsProductDto markedIdsProductDto)
        {
            List<int> productIds = markedIdsProductDto.ProductIdList;

            var products = dbContext.Products.ToList()
                .Where(product => productIds.Contains(product.Id));


            foreach (Product product in products)
            {
                product.Status = 3;
                product.Shipped = DateTime.Now;
                product.Updated = DateTime.Now;
            }

            dbContext.SaveChanges();

            return Ok(products);
        }

    }
}
