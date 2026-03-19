namespace ProductCatalog.DTOs
{
    /// <summary>
    /// Class to handle the product details
    /// </summary>
    public class ProductDto
    {
        #region Public properties

        /// <summary>
        /// Product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Proudct price per item
        /// </summary>
        public decimal PricePerItem { get; set; }

        /// <summary>
        /// Product average customer rating
        /// </summary>
        public double AverageCustomerRating { get; set; }

        #endregion

    }
}
