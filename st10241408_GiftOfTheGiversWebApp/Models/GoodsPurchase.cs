// Import necessary namespaces for data validation and database operations
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Define the namespace for the GoodsPurchase model
namespace ST10241408_GiftOfTheGiversWebApp.Models
{
    // Define the GoodsPurchase model
    public class GoodsPurchase
    {
        // Define the primary key for the GoodsPurchase model
        [Key]
        public int GoodsPurchaseID { get; set; }

        // Define the price of the item, which is required
        [Required]
        [Display(Name = "Item Price")]
        public decimal GoodsPurchasePrice { get; set; }

        // Define the number of items, which is required and must be greater than or equal to 0
        [Required]
        [Display(Name = "Number of items")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public int ITEM_COUNT { get; set; }

        // Define the total price of the goods
        public decimal GoodsTotalPrice { get; set; }

        // Define the category of the goods, which is optional
        [Display(Name = "Category")]
        public String? CATEGORY { get; set; }

        // Define a non-mapped property for the goods inventory ID
        // This property is not persisted to the database
        [NotMapped]
        public int GOODS_INVENTORY_ID { get; set; }
    }
}