using System.ComponentModel.DataAnnotations;

namespace OfferManagementApi.Models
{
    public class BrandDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string BrandName { get; set; } = null!;

        public bool IsActive { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
