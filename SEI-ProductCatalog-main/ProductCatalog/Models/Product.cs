using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Models
{
    /// <summary>
    /// Product model entity class to handle the product details
    /// </summary>
    public class Product
    {
        #region Public properties

        /// <summary>
        /// Product identifier. Primary key for the Product entity
        /// </summary>
        [Key]
        public int ProductId { get; set; }

        /// <summary>
        /// Product name 
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Product price per item
        /// </summary>
        public decimal PricePerItem { get; set; }

        /// <summary>
        /// Product average customer rating in range of 1 to 10
        /// </summary>
        public double AverageCustomerRating { get; set; }

        /// <summary>
        /// A collection of product attributes associated with the product, such as color, size, material, etc.
        /// </summary>
        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();

        #endregion

    }
}
