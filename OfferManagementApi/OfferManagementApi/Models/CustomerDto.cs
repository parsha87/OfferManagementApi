using System.ComponentModel.DataAnnotations;

namespace OfferManagementApi.Models
{
    public class CustomerDto
    {
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
}
