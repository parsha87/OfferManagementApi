using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

[Table("VisitSection")]
public partial class VisitSection
{
    [Key]
    public int VisitSectionId { get; set; }

    public int InquiryId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime VisitDate { get; set; }

    [StringLength(255)]
    public string? VisitReason { get; set; }

    public string? VisitKeyPoints { get; set; }

    [ForeignKey("InquiryId")]
    [InverseProperty("VisitSections")]
    public virtual Inquiry Inquiry { get; set; } = null!;
}
