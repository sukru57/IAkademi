using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace iakademi41CORE_Proje.Models.MVVM
{
    public class Message
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }
        public int UserID { get; set; }
        public string? Content { get; set; }
    }
}
