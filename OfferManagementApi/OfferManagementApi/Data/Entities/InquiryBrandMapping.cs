using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

[Table("InquiryBrandMapping")]
public partial class InquiryBrandMapping
{
    [Key]
    public int Id { get; set; }

    public int InquiryId { get; set; }

    [StringLength(100)]
    public string? InquiryEnqNumber { get; set; }

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

    [StringLength(50)]
    public string? Zone { get; set; }

    [StringLength(50)]
    public string? TempClass { get; set; }

    [StringLength(50)]
    public string? GasGroup { get; set; }

    [ForeignKey("InquiryId")]
    [InverseProperty("InquiryBrandMappings")]
    public virtual Inquiry1 Inquiry { get; set; } = null!;
}
