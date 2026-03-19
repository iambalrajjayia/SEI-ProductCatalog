using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;

namespace ProductCatalog.Controllers
{
    /// <summary>
    /// Controller class to handle product related functionalities
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region Private variables

        /// <summary>
        /// Logger object
        /// </summary>
        private readonly ILogger<ProductController> _logger;

        /// <summary>
        /// DB context
        /// </summary>
        private readonly ProductDbContext _productDbContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="productDbContext">Db context</param>
        public ProductController(ILogger<ProductController> logger, ProductDbContext productDbContext)
        {
            _logger = logger;
            _productDbContext = productDbContext;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get collection of all the products
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet(Name = AppResources.API_GET_PRODUCTS)]
        public async Task<IActionResult> GetProducts()
        {
            //Write log
            _logger.LogInformation(AppResources.API_GET_PRODUCTS);

            try
            {
                //Select products for db context to generate list of products with descending order of average customer rating
                var products = await _productDbContext.Products
                    .OrderByDescending(p => p.AverageCustomerRating)
                    .Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        PricePerItem = p.PricePerItem,
                        AverageCustomerRating = p.AverageCustomerRating
                    })
                    .ToListAsync();
                //Return OK with list of products as success code 200
                return Ok(products);
            }
            catch (Exception ex)
            {
                //Log execption
                _logger.LogError(ex, $"{AppResources.API_GET_PRODUCTS} {AppResources.MESSAGE_EXCEPTION}");
                //Retun status code 500 with error message 
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppResources.MESSAGE_UNHANDLED_EXCEPTION });
            }
        }

        /// <summary>
        /// Get a product based on a product identifier
        /// </summary>
        /// <param name="productId">Unique product identifier</param>
        /// <returns>A product</returns>
        [HttpGet("{productId:int}", Name = "GetProduct")]
        public async Task<ActionResult> GetProduct(int productId)
        {
            //Write log information
            _logger.LogInformation("GetProduct with id = {ProductId}", productId);

            try
            {
                //Select a first product from db context based on the product id.
                //If product exist based on the id then get the proudct else get default value as null
                var product = await _productDbContext.Products
                    .Where(p => p.ProductId == productId)
                    .Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        PricePerItem = p.PricePerItem,
                        AverageCustomerRating = p.AverageCustomerRating
                    })
                    .FirstOrDefaultAsync();

                //Verify if product is null then write into a log file, product not found.
                if (product is null)
                {
                    _logger.LogWarning("GetProduct: product id = {ProductId} not found", productId);
                    return NotFound(new { message = $"Product with id {productId} was not found." });
                }
                //Return OK with a product as success code 200
                return Ok(product);
            }
            catch (Exception ex)
            {
                //Log execption
                _logger.LogError(ex, $"{AppResources.API_GET_PRODUCT} {AppResources.MESSAGE_EXCEPTION} for product Id = {productId}");
                //Retun status code 500 with error message 
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppResources.MESSAGE_UNHANDLED_EXCEPTION });
            }

        }

        /// <summary>
        /// Get product attributes for a given product based on id
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>List of product attributes a given product</returns>
        [HttpGet("{productId:int}/attributes", Name = "GetProductAttributes")]
        public async Task<IActionResult> GetProductAttributes(int productId)
        {
            //Write log
            _logger.LogInformation("GetProductAttributes with id = {ProductId}", productId);

            try
            {
               
                //Verify whether product exist or not.
                bool isProductExists = await _productDbContext.Products
                    .AnyAsync(p => p.ProductId == productId);

                //If product does not exist then return product not found, else find attributes for the product
                if (!isProductExists)
                {
                    _logger.LogWarning("GetProductAttributes: product id={ProductId} not found", productId);
                    return NotFound(new { message = $"Product with id {productId} was not found." });
                }

                //If product exist then get a list of product attributes for the product based on id
                var attributes = await _productDbContext.ProductAttributes
                    .Where(a => a.ProductId == productId)
                    .Select(a => new ProductAttributeDto
                    {
                        AttributeId = a.AttributeId,
                        AttributeName = a.AttributeName,
                        AttributeValue = a.AttributeValue
                    })
                    .ToListAsync();

                //If product does not have attributes
                if (attributes is null)
                {
                    _logger.LogWarning("GetProductAttributes: product id={ProductId} does not have attributes", productId);
                    return NotFound(new { message = $"Product with id {productId} does not have attributes." });
                }

                //Return OK with list of product attributes for the product
                return Ok(attributes);
            }
            catch (Exception ex)
            {
                //Log execption
                _logger.LogError(ex, $"{AppResources.API_GET_PRODUCT_ATTRIBUTES} {AppResources.MESSAGE_EXCEPTION} for product Id = {productId}");
                //Retun status code 500 with error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = AppResources.MESSAGE_UNHANDLED_EXCEPTION });
            }
        }

        #endregion

    }

}

