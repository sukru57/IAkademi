using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace iakademi41CORE_Proje.Models.MVVM
{
    public class Category
    {
        //primary key , identity=yes
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int CategoryID { get; set; }


        [DisplayName("Kategori Adı")] //formda görünecek hali
        [Required(ErrorMessage = "Kategori Adı Zorunlu Alan")]
        [StringLength(50, ErrorMessage = "En fazla 50 Karakter")]
        public string? CategoryName { get; set; }


        [DisplayName("Üst Kategori")]
        public int? ParentID { get; set; } //üst ID


        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }

    }
}
