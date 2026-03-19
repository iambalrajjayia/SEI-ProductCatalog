namespace ProductCatalog.DTOs
{
    /// <summary>
    ///Class to handle the product related attributes
    /// </summary>
    public class ProductAttributeDto
    {
        #region Public propertites

        /// <summary>
        /// Attribute identifier
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// Attribute name
        /// </summary>
        public string AttributeName { get; set; } = string.Empty;

        /// <summary>
        /// Attribute value
        /// </summary>
        public string AttributeValue { get; set; } = string.Empty;

        #endregion

    }
}
