using Microsoft.EntityFrameworkCore;
using ProductCatalog.Models;

namespace ProductCatalog.Data
{
    /// <summary>
    /// Product DB context class to handle the DB Context of entity framework
    /// </summary>
    public class ProductDbContext: DbContext
    { 
        
        #region Public properties

        /// <summary>
        /// DB set for the Products
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// DB set for the product attributes
        /// </summary>
        public DbSet<ProductAttribute> ProductAttributes { get; set; } = null!;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to invoke base class options
        /// </summary>
        /// <param name="options">Options</param>
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Override method for on model createing method to create product and attributes models
        /// </summary>
        /// <param name="modelBuilder">Modfel builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Set up relationships between models
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Attributes)
                .WithOne(a => a.Product)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            //Precision for money columns
            modelBuilder.Entity<Product>()
                .Property(p => p.PricePerItem)
                .HasPrecision(18, 2);

            //Create Product data
            modelBuilder.Entity<Product>().HasData(
                //New Products TV, Mouse, Phone, Book
                new Product { ProductId = 1, ProductName = "TV", PricePerItem = 399.99m, AverageCustomerRating = 3.0 },
                new Product { ProductId = 2, ProductName = "Mouse", PricePerItem = 29.99m, AverageCustomerRating = 3.5 },
                new Product { ProductId = 3, ProductName = "Phone", PricePerItem = 799.99m, AverageCustomerRating = 3.0 },
                new Product { ProductId = 4, ProductName = "Book", PricePerItem = 19.99m, AverageCustomerRating = 4.5 }
            );

            //Create Product attributes data
            modelBuilder.Entity<ProductAttribute>().HasData(

                //TV product attributes
                new ProductAttribute { AttributeId = 1, ProductId = 1, AttributeName = "Color", AttributeValue = "White" },
                new ProductAttribute { AttributeId = 2, ProductId = 1, AttributeName = "Width", AttributeValue = "30" },
                new ProductAttribute { AttributeId = 3, ProductId = 1, AttributeName = "Height", AttributeValue = "45" },

                //Mouse product attributes
                new ProductAttribute { AttributeId = 4, ProductId = 2, AttributeName = "Color", AttributeValue = "Black" },
                new ProductAttribute { AttributeId = 5, ProductId = 2, AttributeName = "Hand", AttributeValue = "Right Handed" },
                new ProductAttribute { AttributeId = 6, ProductId = 2, AttributeName = "Battery", AttributeValue = "AAA" },

                //Phone product attributes
                new ProductAttribute { AttributeId = 7, ProductId = 3, AttributeName = "Type", AttributeValue = "I-Phone" },
                new ProductAttribute { AttributeId = 8, ProductId = 3, AttributeName = "Model", AttributeValue = "17 Pro" },
                new ProductAttribute { AttributeId = 9, ProductId = 3, AttributeName = "Width", AttributeValue = "10" },
                new ProductAttribute { AttributeId = 10, ProductId = 3, AttributeName = "Height", AttributeValue = "45" },

                //Book attributes
                new ProductAttribute { AttributeId = 4, ProductId = 3, AttributeName = "Color", AttributeValue = "Orange" },
                new ProductAttribute { AttributeId = 5, ProductId = 3, AttributeName = "Language", AttributeValue = "English" },
                new ProductAttribute { AttributeId = 6, ProductId = 3, AttributeName = "Author", AttributeValue = "Balraj" }

            );
        }

        #endregion

    }
}
