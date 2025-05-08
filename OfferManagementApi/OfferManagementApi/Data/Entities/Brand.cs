using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

public partial class Brand
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string BrandName { get; set; } = null!;

    public bool IsActive { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }
}
