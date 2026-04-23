using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBConnect.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; } = null!;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;

        // In real app store a hashed password
        [Required]
        public string PasswordHash { get; set; } = null!;

        public string? FullName { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<Cart>? Carts { get; set; }
    }
}