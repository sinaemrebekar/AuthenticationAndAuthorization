using System.ComponentModel.DataAnnotations;

namespace WEB.Models.AccountViewModels
{
    public class EditUserVM
    {
        public required string Id { get; set; }

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MaxLength(120, ErrorMessage = "120 karakter sınırını geçtiniz!")]
        [MinLength(2, ErrorMessage = "En az 2 karakter girmelisiniz!")]
        [RegularExpression("^[a-zA-ZıİşŞğĞöÖüÜ ]+$", ErrorMessage = "Sadece harf ve boşluk girebilirsiniz!")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MaxLength(120, ErrorMessage = "120 karakter sınırını geçtiniz!")]
        [MinLength(2, ErrorMessage = "En az 2 karakter girmelisiniz!")]
        [RegularExpression("^[a-zA-ZıİşŞğĞöÖüÜ ]+$", ErrorMessage = "Sadece harf ve boşluk girebilirsiniz!")]
        public string LastName { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MaxLength(30, ErrorMessage = "30 karakter sınırını geçtiniz!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmelisiniz!")]
        [RegularExpression("^[a-zA-Z0-9 ıİşŞğĞöÖüÜ-]+$", ErrorMessage = "Sadece harf, sayı, boşluk ve \"-\" girebilirsiniz!")]
        public string UserName { get; set; }

        [Display(Name = "E-Mail")]
        [EmailAddress(ErrorMessage = "E-Mail formatında giriş yapınız!")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MaxLength(150, ErrorMessage = "150 karakter sınırını geçtiniz!")]
        public string Email { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [DataType(DataType.Date)]
        public DateOnly Birthdate { get; set; }
    }
}
