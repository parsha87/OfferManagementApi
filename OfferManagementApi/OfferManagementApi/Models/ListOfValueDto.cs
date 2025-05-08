using System.ComponentModel.DataAnnotations;

namespace OfferManagementApi.Models
{
    public class ListOfValueDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Value { get; set; } = null!;

        [StringLength(500)]
        public string Type { get; set; }
    }

    public enum ListOfValueType
    {
        Brand,
        Frequency,
        Voltage,
        StartsPerHour,
        AmbientTemp,
        Accessories,
        Application,
        Segment,
        StdPaymentTerms
    }
}
