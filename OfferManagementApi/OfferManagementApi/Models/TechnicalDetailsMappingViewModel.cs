namespace OfferManagementApi.Models
{
    public class TechnicalDetailsMappingViewModel
    {
        public int Id { get; set; }
        public int InquiryId { get; set; }
        public string? MotorType { get; set; }
        public string? KW { get; set; }
        public string? HP { get; set; }
        public string? Phase { get; set; }
        public string? Pole { get; set; }
        public string? FrameSize { get; set; }
        public string? DOP { get; set; }
        public string? InsulationClass { get; set; }
        public string? Efficiency { get; set; }
        public string? Voltage { get; set; }
        public string? Frequency { get; set; }
        public string? Quantity { get; set; }
        public string? Mounting { get; set; }
        public string? SafeAreaHazardousArea { get; set; }
        public string? Brand { get; set; }
        public string? IfHazardousArea { get; set; }
        public string? TempClass { get; set; }
        public string? GasGroup { get; set; }
        public string? Zone { get; set; }
        public string? HardadousDescription { get; set; }
        public string? Duty { get; set; }
        public string? StartsPerHour { get; set; }
        public string? CDF { get; set; }
        public string? AmbientTemp { get; set; }
        public string? TempRise { get; set; }
        public List<string> Accessories { get; set; } = new List<string>();
        public string? Brake { get; set; }
        public string? EncoderMounting { get; set; }
        public string? EncoderMountingIfYes { get; set; }
        public string? Application { get; set; }
        public string? Segment { get; set; }
        public string? Narration { get; set; }
        public decimal? Amount { get; set; }
        public string DeliveryTime { get; set; }
    }
}
