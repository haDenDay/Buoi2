using System.ComponentModel.DataAnnotations;

namespace Buoi2.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
