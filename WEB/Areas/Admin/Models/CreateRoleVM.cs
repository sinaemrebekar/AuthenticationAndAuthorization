using System.ComponentModel.DataAnnotations;

namespace WEB.Areas.Admin.Models
{
    public class CreateRoleVM
    {
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmelisiniz")]
        [MaxLength(120, ErrorMessage = "120 karakter sınırını geçtiniz!")]
        public required string Name { get; set; }
    }
}
