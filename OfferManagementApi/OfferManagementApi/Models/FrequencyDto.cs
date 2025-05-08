using System.ComponentModel.DataAnnotations;

namespace OfferManagementApi.Models
{
    public class FrequencyDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string? FrequencyValue { get; set; }

    }
}
