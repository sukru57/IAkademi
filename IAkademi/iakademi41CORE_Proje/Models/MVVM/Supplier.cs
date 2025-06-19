using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace iakademi41CORE_Proje.Models.MVVM
{
    public class Supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }


        [StringLength(100)]
        [Required]
        [DisplayName("Marka Adı")]
      //  [RegularExpression(@"^[a-zA-Z]*$")]  //sadece harf
        public string? BrandName { get; set; }

        [DisplayName("Resim")]
        [Required]
        public string? PhotoPath { get; set; }


        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }

    }
}
