using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLearn.Models
{
    [Table("moduleassignments")]
    public class ModuleAssignment
    {
        [Key]
        [Column("assignmentid")]
        public int AssignmentID { get; set; }

        [Required]
        [Column("moduleid")]
        public int ModuleID { get; set; }

        [Required]
        [StringLength(255)]
        [Column("assignmenttitle")]
        public string AssignmentTitle { get; set; } = string.Empty;

        [Column("assignmentpdfurl")]
        [StringLength(1024)]
        public string? AssignmentPdfURL { get; set; }

        [Column("assignmentpdf")]
        public byte[]? AssignmentPdf { get; set; }

        [Column("assignmentpdfmime")]
        [StringLength(100)]
        public string? AssignmentPdfMime { get; set; }

        [Column("assignmentpdfsizebytes")]
        public long? AssignmentPdfSizeBytes { get; set; }

        [Required]
        [Column("datedue")]
        public DateTime DateDue { get; set; }

        [Column("assignmentuploadpdfurl")]
        [StringLength(1024)]
        public string? AssignmentUploadPdfURL { get; set; }

        [Column("assignmentuploadpdf")]
        public byte[]? AssignmentUploadPdf { get; set; }

        [Column("assignmentuploadpdfmime")]
        [StringLength(100)]
        public string? AssignmentUploadPdfMime { get; set; }

        [Column("assignmentuploadpdfsizebytes")]
        public long? AssignmentUploadPdfSizeBytes { get; set; }

        // Navigation
        public TopicModule? Module { get; set; }
    }
}

