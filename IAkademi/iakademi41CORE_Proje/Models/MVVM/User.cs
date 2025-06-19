using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace iakademi41CORE_Proje.Models.MVVM
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [StringLength(100)]
        [Required]
        [DisplayName("Kullanıcı Adı")] //formda görünecek hali
        public string? NameSurname { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Email Zorunlu Alan")]
        [EmailAddress(ErrorMessage = "Doğru Email Adresi Girmediniz")]
        public string? Email { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Şifre Zorunlu Alan")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }  //d375af34cc08aba9a1cc9b6596a70c36

        public string? Telephone { get; set; }

        public string? InvoicesAddres { get; set; } //fatura adresi

        public bool IsAdmin { get; set; } //normal kullanıcımı? calısan personelmi?

        public bool Active { get; set; }
    }
}
