using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10241408_GiftOfTheGiversWebApp.Models
{
    // Define a class to represent a Disaster entity
    public class Disaster
    {
        // Primary key and required field to uniquely identify a disaster
        [Key]
        [Required]
        public int DISTATER_ID { get; set; }

        // Username associated with the disaster (not required)
        public string USERNAME { get; set; }

        // Start date of the disaster (required, only store the date part)
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")] // Store only the date part
        [Display(Name = "Start Date")] // Display name for the UI
        public DateTime? STARTDATE { get; set; }

        // End date of the disaster (required, only store the date part)
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")] // Store only the date part
        [Display(Name = "End Date")] // Display name for the UI
        public DateTime? ENDDATE { get; set; }

        // Location of the disaster (required)
        [Required]
        [Display(Name = "Location")] // Display name for the UI
        public string? LOCATION { get; set; }

        // Type of aid required for the disaster (required)
        [Required]
        [Display(Name = "Aid Type")] // Display name for the UI
        public string? AID_TYPE { get; set; }

        // Flag to indicate if the disaster is active (not required)
        [Display(Name = "Is Active")] // Display name for the UI
        public int IsActive { get; set; }
    }
}