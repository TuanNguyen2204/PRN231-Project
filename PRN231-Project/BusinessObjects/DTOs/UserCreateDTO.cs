using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class UserCreateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the UserName.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Please enter the Password.")]
        [MinLength(6)]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Please enter the Name.")]
        public string Name { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Please enter the Address.")]
        public string Address { get; set; } = null!;
        public bool IsSeller { get; set; }
    }
}
