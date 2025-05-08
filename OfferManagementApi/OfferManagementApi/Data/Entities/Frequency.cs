using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OfferManagementApi.Data.Entities;

[Table("Frequency")]
public partial class Frequency
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string? FrequencyValue { get; set; }
}
