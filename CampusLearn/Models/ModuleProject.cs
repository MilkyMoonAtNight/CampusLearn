using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("moduleprojects")]
    public class ModuleProject
    {
        [Key]
        [Column("projectid")]
        public int ProjectID { get; set; }

        [Required]
        [Column("moduleid")]
        public int ModuleID { get; set; }

        [Required]
        [StringLength(255)]
        [Column("projecttitle")]
        public string ProjectTitle { get; set; } = string.Empty;

        [Column("projectpdfurl")]
        [StringLength(1024)]
        public string? ProjectPdfURL { get; set; }

        [Column("projectpdf")]
        public byte[]? ProjectPdf { get; set; }

        [Column("projectpdfmime")]
        [StringLength(100)]
        public string? ProjectPdfMime { get; set; }

        [Column("projectpdfsizebytes")]
        public long? ProjectPdfSizeBytes { get; set; }

        [Required]
        [Column("datedue")]
        public DateTime DateDue { get; set; }

        [Column("uploadpdfurl")]
        [StringLength(1024)]
        public string? UploadPdfURL { get; set; }

        [Column("uploadpdf")]
        public byte[]? UploadPdf { get; set; }

        [Column("uploadpdfmime")]
        [StringLength(100)]
        public string? UploadPdfMime { get; set; }

        [Column("uploadpdfsizebytes")]
        public long? UploadPdfSizeBytes { get; set; }

        // Navigation
        public TopicModule? Module { get; set; }
    }
}
