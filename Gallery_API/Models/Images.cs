using Gallery_API.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gallery_API.Models
{
    [Table("images")]
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int User_Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Filename { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime Created_At { get; set; } = DateTime.UtcNow;

        public byte[] Image_Data { get; set; } // Byte array to store image data

        // Navigation property to link back to user
        [ForeignKey("User_Id")]
        public virtual User User { get; set; }
    }
}
