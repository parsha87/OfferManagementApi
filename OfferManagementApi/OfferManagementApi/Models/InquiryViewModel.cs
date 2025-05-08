namespace OfferManagementApi.Models
{
    public class InquiryViewModel
    {
        public int InquiryId { get; set; }
        public string CustomerType { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string? RfqNo { get; set; }
        public DateTime RfqDate { get; set; }
        public string? StdPaymentTerms { get; set; }
        public string? StdIncoTerms { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal NetPriceWithoutGST { get; set; }
        public decimal TotalPackage { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public List<TechnicalDetailsMappingViewModel> TechicalDetailsMapping { get; set; }
    }
}
