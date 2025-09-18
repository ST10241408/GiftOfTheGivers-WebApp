using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10241408_GiftOfTheGiversWebApp.Models
{
    // This class represents a goods donation
    public class GoodsDonation
    {
        // Primary key and required field for the goods donation ID
        [Key]
        [Required]
        public int GOODS_DONATION_ID { get; set; }

        // The username of the person making the donation
        public string USERNAME { get; set; }

        // Required field for the date of the donation
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Donation Date")]
        public DateTime? DATE { get; set; }

        // Required field for the number of items being donated
        [Required]
        [Display(Name = "Number of items")]
        public int ITEM_COUNT { get; set; }

        // Required field for the category of the donated goods
        [Required]
        [Display(Name = "Category")]
        public String? CATEGORY { get; set; }

        // Required field for the description of the donated goods
        [Required]
        [Display(Name = "Description")]
        public String? DESCRIPTION { get; set; }

        // Optional field for the name of the donor
        [Display(Name = "Donor")]
        public String? DONOR { get; set; }
    }
}