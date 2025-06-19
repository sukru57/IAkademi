using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace iakademi41CORE_Proje.Models.MVVM
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")] //formda görünecek hali
        public int StatusID { get; set; }

        [DisplayName("Statü Adı")] //formda görünecek hali
        [StringLength(100)]
        [Required]
        public string? StatusName { get; set; }

        [DisplayName("Aktif/Pasif")] //formda görünecek hali
        public bool Active { get; set; }
    }
}
