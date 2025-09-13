using System;
using System.ComponentModel.DataAnnotations;

namespace ST10241408_GiftOfTheGiversWebApp.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
