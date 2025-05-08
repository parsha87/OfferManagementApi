using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

[Table("Inquiry")]
[Index("CustomerName", Name = "IX_Inquiry_CustomerName")]
[Index("DateOfEnq", Name = "IX_Inquiry_DateOfEnq")]
[Index("EnqNumber", Name = "IX_Inquiry_EnqNumber")]
[Index("Rfqnumber", Name = "IX_Inquiry_RFQNumber")]
[Index("Status", Name = "IX_Inquiry_Status")]
public partial class Inquiry1
{
    [Key]
    public int Id { get; set; }

    [Column("KW")]
    [StringLength(50)]
    public string? Kw { get; set; }

    [Column("HP")]
    [StringLength(50)]
    public string? Hp { get; set; }

    [StringLength(50)]
    public string? Pole { get; set; }

    [StringLength(50)]
    public string? FrameSize { get; set; }

    [Column("DOP")]
    [StringLength(50)]
    public string? Dop { get; set; }

    [Column("IP")]
    [StringLength(50)]
    public string? Ip { get; set; }

    [StringLength(50)]
    public string? Efficiency { get; set; }

    [StringLength(50)]
    public string? Voltage { get; set; }

    [StringLength(50)]
    public string? Frequency { get; set; }

    public int? Quantity { get; set; }

    [StringLength(100)]
    public string? Mounting { get; set; }

    [StringLength(100)]
    public string? SafeAreaHazardousArea { get; set; }

    [StringLength(100)]
    public string? Brand { get; set; }

    public bool? IfHazardousArea { get; set; }

    public string? Narration { get; set; }

    [StringLength(100)]
    public string? CustomerName { get; set; }

    [StringLength(100)]
    public string? Region { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? EnqNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateOfEnq { get; set; }

    [StringLength(255)]
    public string? StdPaymentTerms { get; set; }

    [StringLength(255)]
    public string? StdIncoTerms { get; set; }

    [Column("RFQNumber")]
    [StringLength(100)]
    public string? Rfqnumber { get; set; }

    [Column("RFQDate", TypeName = "datetime")]
    public DateTime? Rfqdate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ListPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }

    [Column("NetPriceWithoutGST", TypeName = "decimal(18, 2)")]
    public decimal? NetPriceWithoutGst { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPackage { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(450)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    [StringLength(450)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedOn { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ClosedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OfferSentDate { get; set; }

    [StringLength(255)]
    public string? Zone { get; set; }

    [StringLength(255)]
    public string? TempClass { get; set; }

    [StringLength(255)]
    public string? GasGroup { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Inquiry1CreatedByNavigations")]
    public virtual AspNetUser? CreatedByNavigation { get; set; }

    [InverseProperty("Inquiry")]
    public virtual ICollection<InquiryBrandMapping> InquiryBrandMappings { get; set; } = new List<InquiryBrandMapping>();

    [ForeignKey("UpdatedBy")]
    [InverseProperty("Inquiry1UpdatedByNavigations")]
    public virtual AspNetUser? UpdatedByNavigation { get; set; }
}
