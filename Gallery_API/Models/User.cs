using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gallery_API.Model
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public int Role { get; set; }

    // Ensure you're referencing the correct column name (case-sensitive)
       [Column(TypeName = "timestamp")]
        public DateTime? Created_At { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? Updated_At { get; set; }
    }
}
