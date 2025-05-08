using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

public partial class TechnicalDetailsMapping
{
    [Key]
    public int Id { get; set; }

    public int? InquiryId { get; set; }

    [StringLength(100)]
    public string? MotorType { get; set; }

    [Column("KW")]
    [StringLength(50)]
    public string? Kw { get; set; }

    [Column("HP")]
    [StringLength(50)]
    public string? Hp { get; set; }

    [StringLength(50)]
    public string? Phase { get; set; }

    [StringLength(50)]
    public string? Pole { get; set; }

    [StringLength(50)]
    public string? FrameSize { get; set; }

    [Column("DOP")]
    [StringLength(50)]
    public string? Dop { get; set; }

    [StringLength(50)]
    public string? InsulationClass { get; set; }

    [StringLength(50)]
    public string? Efficiency { get; set; }

    [StringLength(50)]
    public string? Voltage { get; set; }

    [StringLength(50)]
    public string? Frequency { get; set; }

    [StringLength(50)]
    public string? Quantity { get; set; }

    [StringLength(50)]
    public string? Mounting { get; set; }

    [StringLength(100)]
    public string? SafeAreaHazardousArea { get; set; }

    [StringLength(100)]
    public string? Brand { get; set; }

    [StringLength(50)]
    public string? IfHazardousArea { get; set; }

    [StringLength(50)]
    public string? TempClass { get; set; }

    [StringLength(50)]
    public string? GasGroup { get; set; }

    [StringLength(50)]
    public string? Zone { get; set; }

    [StringLength(500)]
    public string? HardadousDescription { get; set; }

    [StringLength(100)]
    public string? Duty { get; set; }

    [StringLength(50)]
    public string? StartsPerHour { get; set; }

    [Column("CDF")]
    [StringLength(50)]
    public string? Cdf { get; set; }

    [StringLength(50)]
    public string? AmbientTemp { get; set; }

    [StringLength(50)]
    public string? TempRise { get; set; }

    [StringLength(200)]
    public string? Accessories { get; set; }

    [StringLength(100)]
    public string? Brake { get; set; }

    [StringLength(100)]
    public string? EncoderMounting { get; set; }

    [StringLength(100)]
    public string? EncoderMountingIfYes { get; set; }

    [StringLength(200)]
    public string? Application { get; set; }

    [StringLength(100)]
    public string? Segment { get; set; }

    public string? Narration { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [ForeignKey("InquiryId")]
    [InverseProperty("TechnicalDetailsMappings")]
    public virtual Inquiry? Inquiry { get; set; }
}
