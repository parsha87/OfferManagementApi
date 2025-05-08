using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

[Table("InquiryAttachmentsRecord")]
public partial class InquiryAttachmentsRecord
{
    [Key]
    public int AttachmentId { get; set; }

    public int InquiryId { get; set; }

    [StringLength(255)]
    public string? OriginalFileName { get; set; }

    [StringLength(255)]
    public string? UniqueFileName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UploadedOn { get; set; }

    [ForeignKey("InquiryId")]
    [InverseProperty("InquiryAttachmentsRecords")]
    public virtual Inquiry Inquiry { get; set; } = null!;
}
