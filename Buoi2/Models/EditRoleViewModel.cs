using System.ComponentModel.DataAnnotations;

namespace Buoi2.Models
{
    public class EditRoleViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage ="Điền tên")]
        public string RoleName { get; set; }
    }
}
