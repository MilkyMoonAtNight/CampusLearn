using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("weekcontent")]
    public class WeekContent
    {
        [Key]
        [Column("contentid")]
        public int ContentID { get; set; }

        [Required]
        [Column("weekid")]
        public int WeekID { get; set; }

        [Required]
        [Column("contenttitle")]
        [StringLength(255)]
        public string ContentTitle { get; set; } = string.Empty;

        // File path or URL for the stored PDF
        [Column("pdfurl")]
        [StringLength(1024)]
        public string? PdfURL { get; set; }

        // Optional physical PDF (nullable bytea in DB)
        [Column("pdffile")]
        public byte[]? PdfFile { get; set; }

        [Column("pdfmime")]
        [StringLength(100)]
        public string? PdfMime { get; set; }

        [Column("pdfsizebytes")]
        public long? PdfSizeBytes { get; set; }

        // Navigation
        public ModuleWeek? ModuleWeek { get; set; }
    }
}
