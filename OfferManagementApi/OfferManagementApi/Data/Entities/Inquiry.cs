using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

public partial class Inquiry
{
    [Key]
    public int InquiryId { get; set; }

    [StringLength(100)]
    public string? CustomerType { get; set; }

    [StringLength(200)]
    public string? CustomerName { get; set; }

    public int? CustomerId { get; set; }

    [StringLength(100)]
    public string? Region { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? EnquiryNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EnquiryDate { get; set; }

    [StringLength(100)]
    public string? RfqNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RfqDate { get; set; }

    [StringLength(200)]
    public string? StdPaymentTerms { get; set; }

    [StringLength(200)]
    public string? StdIncoTerms { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ListPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }

    [Column("NetPriceWithoutGST", TypeName = "decimal(18, 2)")]
    public decimal? NetPriceWithoutGst { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPackage { get; set; }

    [StringLength(100)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedOn { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    [InverseProperty("Inquiry")]
    public virtual ICollection<InquiryAttachmentsRecord> InquiryAttachmentsRecords { get; set; } = new List<InquiryAttachmentsRecord>();

    [InverseProperty("Inquiry")]
    public virtual ICollection<TechnicalDetailsMapping> TechnicalDetailsMappings { get; set; } = new List<TechnicalDetailsMapping>();
}
