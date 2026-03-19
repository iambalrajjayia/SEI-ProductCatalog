using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Models
{
    /// <summary>
    /// Product attribute model entity class to handle the product attribute
    /// </summary>
    public class ProductAttribute
    {
        #region Public properties

        /// <summary>
        /// Product attribute identifier. Primary key for the product attribute entity
        /// </summary>
        [Key]
        public int AttributeId { get; set; }

        /// <summary>
        /// Product identifier. Foreign key to the Product entity
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Attribute name such as "Color", "Width", "Height", etc.
        /// </summary>
        public string AttributeName { get; set; } = string.Empty;

        /// <summary>
        /// Attribute value such as "Green", 12, 16, etc.
        /// </summary>
        public string AttributeValue { get; set; } = string.Empty;

        /// <summary>
        /// Product details as reference for navigation purposes
        /// </summary>
        public Product? Product { get; set; }

        #endregion

    }
}
