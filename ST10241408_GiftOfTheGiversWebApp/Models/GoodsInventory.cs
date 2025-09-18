using System.ComponentModel.DataAnnotations;

namespace ST10241408_GiftOfTheGiversWebApp.Models
{
    // This class represents a Goods Inventory entity
    public class GoodsInventory
    {
        // Primary key of the Goods Inventory table
        [Key]
        public int GOODS_INVENTORY_ID { get; set; }

        // Category of the goods (e.g. electronics, clothing, etc.)
        public string CATEGORY { get; set; }

        // The current count of items in the inventory (can be null if not set)
        public int? ITEM_COUNT { get; set; }
    }
}