using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cush.Models
{
    [Table("transactions")]
    public class Cush
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "NUMERIC(10, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Kind { get; set; }

        [Required]
        [Column(TypeName = "TIMESTAMP WITH TIME ZONE")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "TIMESTAMP WITH TIME ZONE")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}