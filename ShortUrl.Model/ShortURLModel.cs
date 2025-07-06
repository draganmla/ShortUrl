using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortUrl.Model
{
    public class ShortURLModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR(50)")]
        public string ShortURL { get; set; }

        [Required]
        [MaxLength(2000)]
        [Column(TypeName = "VARCHAR(2000)")]
        [Url]
        public string LongURL { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastAccessed { get; set; }

        public int AccessCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime? ExpiresAt { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation properties can be added here for future expansion
        // public virtual ICollection<ShortUrlAnalytics> Analytics { get; set; }
    }
}
