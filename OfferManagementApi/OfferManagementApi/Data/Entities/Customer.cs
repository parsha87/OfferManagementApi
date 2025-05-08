using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

[Table("Customer")]
public partial class Customer
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    public string CustomerName { get; set; } = null!;

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(500)]
    public string? Region { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? PhoneNo { get; set; }
}
