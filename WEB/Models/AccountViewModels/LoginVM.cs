using System.ComponentModel.DataAnnotations;

namespace WEB.Models.AccountViewModels
{
    public class LoginVM
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MaxLength(30, ErrorMessage = "30 karakter sınırını geçtiniz!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmelisiniz!")]
        [RegularExpression("^[a-zA-Z0-9 ıİşŞğĞöÖüÜ-]+$", ErrorMessage = "Sadece harf, sayı, boşluk ve \"-\" girebilirsiniz!")]
        public string UserName { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MaxLength(10, ErrorMessage = "10 karakter sınırını geçtiniz!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmelisiniz!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
