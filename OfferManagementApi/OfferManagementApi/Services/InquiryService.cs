using Microsoft.EntityFrameworkCore;
using OfferManagementApi.Data;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;
using System;

namespace OfferManagementApi.Services
{
    public interface IInquiryService
    {
        Task<int> SaveInquiryAsync(InquiryViewModel model);
        Task<List<InquiryViewModel>> GetAllInquiriesAsync();
        Task<InquiryViewModel> GetInquiryByIdAsync(int id);
        Task UpdateInquiryAsync(InquiryViewModel model);
        Task DeleteInquiryAsync(int id);
        Task SaveAttchmentRecord(InquiryAttachmentsRecordsViewModel model);

        Task<InquiryAttachmentsRecordsViewModel> GetAttachmentByIdAsync(int attachmentId);
        Task<bool> DeleteAttachmentByIdAsync(int attachmentId, string fileStoragePath);
    }
    public class InquiryService : IInquiryService
    {
        private readonly MainDBContext _context;

        public InquiryService(MainDBContext context)
        {
            _context = context;
        }

        public async Task<int> SaveInquiryAsync(InquiryViewModel model)
        {
            var today = DateTime.UtcNow.Date;
            var datePart = today.ToString("dd-MM-yy");

            // Count how many inquiries already exist today
            var todayCount = await _context.Inquiries
              .Where(i => i.CreatedOn.HasValue && i.CreatedOn.Value.Date == today)
                .CountAsync();

            int sequenceNumber = todayCount + 1;
            string sequence = sequenceNumber.ToString("D2"); // Formats as 01, 02, 03...

            string enquiryNo = $"INQ-{datePart}-{sequence}";
            string rfqNo = $"RFQ-{datePart}-{sequence}";

            var inquiry = new Inquiry
            {
                CustomerType = model.CustomerType,
                CustomerName = model.CustomerName,
                CustomerId = model.CustomerId,
                CustPhoneNo = model.CustPhoneNo,
                CustAddress = model.CustAddress,
                CustEmail = model.CustEmail,
                Region = model.Region,
                City = model.City,
                State = model.State,
                Country = model.Country,
                Salutation = model.Salutation,
                CpfirstName = model.CpfirstName,
                CplastName = model.CplastName,
                EnquiryNo = enquiryNo,
                EnquiryDate = model.EnquiryDate,
                RfqNo = rfqNo,
                RfqDate = model.RfqDate,
                StdPaymentTerms = model.StdPaymentTerms,
                StdIncoTerms = model.StdIncoTerms,
                ListPrice = model.ListPrice,
                Discount = model.Discount,
                NetPriceWithoutGst = model.NetPriceWithoutGST,
                TotalPackage = model.TotalPackage,
                Status = model.Status,
                OfferStatus = model.OfferStatus,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = model.CreatedBy,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = model.UpdatedBy,
                TechnicalDetailsMappings = model.TechicalDetailsMapping?.Select(x => new TechnicalDetailsMapping
                {
                    MotorType = x.MotorType,
                    Kw = x.KW,
                    Hp = x.HP,
                    Phase = x.Phase,
                    Pole = x.Pole,
                    FrameSize = x.FrameSize,
                    Dop = x.DOP,
                    InsulationClass = x.InsulationClass,
                    Efficiency = x.Efficiency,
                    Voltage = x.Voltage,
                    Frequency = x.Frequency,
                    Quantity = x.Quantity,
                    Mounting = x.Mounting,
                    SafeAreaHazardousArea = x.SafeAreaHazardousArea,
                    Brand = x.Brand,
                    IfHazardousArea = x.IfHazardousArea,
                    TempClass = x.TempClass,
                    GasGroup = x.GasGroup,
                    Zone = x.Zone,
                    HardadousDescription = x.HardadousDescription,
                    Duty = x.Duty,
                    StartsPerHour = x.StartsPerHour,
                    Cdf = x.CDF,
                    AmbientTemp = x.AmbientTemp,
                    TempRise = x.TempRise,
                    Accessories = string.Join(",", x.Accessories),
                    Brake = x.Brake,
                    EncoderMounting = x.EncoderMounting,
                    EncoderMountingIfYes = x.EncoderMountingIfYes,
                    Application = x.Application,
                    Segment = x.Segment,
                    Narration = x.Narration,
                    Amount = x.Amount,
                    DeliveryTime = x.DeliveryTime
                }).ToList()
            };

            _context.Inquiries.Add(inquiry);
            await _context.SaveChangesAsync();
            return inquiry.InquiryId; // Assuming InquiryId is the primary key
        }

        public async Task SaveAttchmentRecord(InquiryAttachmentsRecordsViewModel model)
        {
            var inquiry = new InquiryAttachmentsRecord
            {
                InquiryId = model.InquiryId,
                OriginalFileName = model.OriginalFileName,
                UniqueFileName = model.UniqueFileName,
                UploadedOn = DateTime.UtcNow
            };

            _context.InquiryAttachmentsRecords.Add(inquiry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InquiryAttachmentsRecord>> GetAttachmentsByInquiryId(int inquiryId)
        {
            return await _context.InquiryAttachmentsRecords
                                 .Where(record => record.InquiryId == inquiryId)
                                 .ToListAsync();
        }

        public async Task<List<InquiryViewModel>> GetAllInquiriesAsync()
        {
            var inquiries = await _context.Inquiries
      .Include(x => x.TechnicalDetailsMappings)
      .ToListAsync(); // Get the data from the database

            var result = inquiries.Select(x => new InquiryViewModel
            {
                InquiryId = x.InquiryId,
                CustomerType = x.CustomerType,
                CustomerName = x.CustomerName,
                CustomerId = (int)x.CustomerId,
                CustPhoneNo = x.CustPhoneNo,
                CustAddress = x.CustAddress,
                CustEmail = x.CustEmail,
                Region = x.Region,
                City = x.City,
                State = x.State,
                Country = x.Country,
                Salutation = x.Salutation,
                CpfirstName = x.CpfirstName,
                CplastName = x.CplastName,
                EnquiryNo = x.EnquiryNo,
                EnquiryDate = (DateTime)x.EnquiryDate,
                RfqNo = x.RfqNo,
                RfqDate = (DateTime)x.RfqDate,
                StdPaymentTerms = x.StdPaymentTerms,
                StdIncoTerms = x.StdIncoTerms,
                ListPrice = (decimal)x.ListPrice,
                Discount = (decimal)x.Discount,
                NetPriceWithoutGST = (decimal)x.NetPriceWithoutGst,
                TotalPackage = (decimal)x.TotalPackage,
                OfferStatus = x.OfferStatus,
                Status = x.Status,
                CreatedOn = (DateTime)x.CreatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedOn = (DateTime)x.UpdatedOn,
                UpdatedBy = x.UpdatedBy,
                TechicalDetailsMapping = x.TechnicalDetailsMappings.Select(td => new TechnicalDetailsMappingViewModel
                {
                    Id = td.Id,
                    InquiryId = (int)td.InquiryId,
                    MotorType = td.MotorType,
                    KW = td.Kw,
                    HP = td.Hp,
                    Phase = td.Phase,
                    Pole = td.Pole,
                    FrameSize = td.FrameSize,
                    DOP = td.Dop,
                    InsulationClass = td.InsulationClass,
                    Efficiency = td.Efficiency,
                    Voltage = td.Voltage,
                    Frequency = td.Frequency,
                    Quantity = td.Quantity,
                    Mounting = td.Mounting,
                    SafeAreaHazardousArea = td.SafeAreaHazardousArea,
                    Brand = td.Brand,
                    IfHazardousArea = td.IfHazardousArea,
                    TempClass = td.TempClass,
                    GasGroup = td.GasGroup,
                    Zone = td.Zone,
                    HardadousDescription = td.HardadousDescription,
                    Duty = td.Duty,
                    StartsPerHour = td.StartsPerHour,
                    CDF = td.Cdf,
                    AmbientTemp = td.AmbientTemp,
                    TempRise = td.TempRise,
                    Accessories = string.IsNullOrWhiteSpace(td.Accessories) ? new List<string>() : td.Accessories.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .ToList(),
                    Brake = td.Brake,
                    EncoderMounting = td.EncoderMounting,
                    EncoderMountingIfYes = td.EncoderMountingIfYes,
                    Application = td.Application,
                    Segment = td.Segment,
                    Narration = td.Narration,
                    Amount = td.Amount.HasValue ? td.Amount.Value : 0,
                    DeliveryTime = td.DeliveryTime
                }).ToList()
            }).ToList();
            return result;
        }

        public async Task<InquiryViewModel> GetInquiryByIdAsync(int id)
        {
            var inquiry = await _context.Inquiries
                .Include(x => x.TechnicalDetailsMappings)
                .FirstOrDefaultAsync(x => x.InquiryId == id);

            // Get attachments
            var attachments = await _context.InquiryAttachmentsRecords
                .Where(a => a.InquiryId == id)
                .ToListAsync();

            var uploadedFiles = attachments.Select(a => new InquiryAttachmentsRecordsViewModel
            {
                AttachmentId = a.AttachmentId,
                InquiryId = a.InquiryId,
                OriginalFileName = a.OriginalFileName,
                UniqueFileName = a.UniqueFileName,
                UploadedOn = (DateTime)a.UploadedOn
            }).ToList();

            if (inquiry == null) return null;

            return new InquiryViewModel
            {
                InquiryId = inquiry.InquiryId,
                CustomerType = inquiry.CustomerType,
                CustomerName = inquiry.CustomerName,
                CustomerId = (int)inquiry.CustomerId,
                CustPhoneNo = inquiry.CustPhoneNo,
                CustAddress = inquiry.CustAddress,
                CustEmail = inquiry.CustEmail,
                Region = inquiry.Region,
                City = inquiry.City,
                State = inquiry.State,
                Country = inquiry.Country,
                Salutation = inquiry.Salutation,
                CpfirstName = inquiry.CpfirstName,
                CplastName = inquiry.CplastName,
                EnquiryNo = inquiry.EnquiryNo,
                EnquiryDate = (DateTime)inquiry.EnquiryDate,
                RfqNo = inquiry.RfqNo,
                RfqDate = (DateTime)inquiry.RfqDate,
                StdPaymentTerms = inquiry.StdPaymentTerms,
                StdIncoTerms = inquiry.StdIncoTerms,
                ListPrice = (decimal)inquiry.ListPrice,
                Discount = (decimal)inquiry.Discount,
                NetPriceWithoutGST = (decimal)inquiry.NetPriceWithoutGst,
                TotalPackage = (decimal)inquiry.TotalPackage,
                Status = inquiry.Status,
                OfferStatus = inquiry.OfferStatus,
                CreatedOn = (DateTime)inquiry.CreatedOn,
                CreatedBy = inquiry.CreatedBy,
                UpdatedOn = (DateTime)inquiry.UpdatedOn,
                UpdatedBy = inquiry.UpdatedBy,
                TechicalDetailsMapping = inquiry.TechnicalDetailsMappings.Select(td => new TechnicalDetailsMappingViewModel
                {
                    Id = td.Id,
                    InquiryId = (int)td.InquiryId,
                    MotorType = td.MotorType,
                    KW = td.Kw,
                    HP = td.Hp,
                    Phase = td.Phase,
                    Pole = td.Pole,
                    FrameSize = td.FrameSize,
                    DOP = td.Dop,
                    InsulationClass = td.InsulationClass,
                    Efficiency = td.Efficiency,
                    Voltage = td.Voltage,
                    Frequency = td.Frequency,
                    Quantity = td.Quantity,
                    Mounting = td.Mounting,
                    SafeAreaHazardousArea = td.SafeAreaHazardousArea,
                    Brand = td.Brand,
                    IfHazardousArea = td.IfHazardousArea,
                    TempClass = td.TempClass,
                    GasGroup = td.GasGroup,
                    Zone = td.Zone,
                    HardadousDescription = td.HardadousDescription,
                    Duty = td.Duty,
                    StartsPerHour = td.StartsPerHour,
                    CDF = td.Cdf,
                    AmbientTemp = td.AmbientTemp,
                    TempRise = td.TempRise,
                    Accessories = td.Accessories.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToList(),
                    Brake = td.Brake,
                    EncoderMounting = td.EncoderMounting,
                    EncoderMountingIfYes = td.EncoderMountingIfYes,
                    Application = td.Application,
                    Segment = td.Segment,
                    Narration = td.Narration,
                    Amount = td.Amount.HasValue ? td.Amount.Value : 0,
                    DeliveryTime = td.DeliveryTime
                }).ToList(),
                uploadedFiles = uploadedFiles
            };
        }

        public async Task UpdateInquiryAsync(InquiryViewModel model)
        {
            var inquiry = await _context.Inquiries
                .Include(x => x.TechnicalDetailsMappings)
                .FirstOrDefaultAsync(x => x.InquiryId == model.InquiryId);

            if (inquiry == null)
                throw new Exception("Inquiry not found");

            inquiry.CustomerType = model.CustomerType;
            inquiry.CustomerName = model.CustomerName;
            inquiry.CustomerId = model.CustomerId;
            inquiry.CustPhoneNo = model.CustPhoneNo;
            inquiry.CustAddress = model.CustAddress;
            inquiry.CustEmail = model.CustEmail;
            inquiry.Region = model.Region;
            inquiry.City = model.City;
            inquiry.State = model.State;
            inquiry.Country = model.Country;
            inquiry.Salutation = model.Salutation;
            inquiry.CpfirstName = model.CpfirstName;
            inquiry.CplastName = model.CplastName;
            inquiry.EnquiryNo = model.EnquiryNo;
            inquiry.EnquiryDate = model.EnquiryDate;
            inquiry.RfqNo = model.RfqNo;
            inquiry.RfqDate = model.RfqDate;
            inquiry.StdPaymentTerms = model.StdPaymentTerms;
            inquiry.StdIncoTerms = model.StdIncoTerms;
            inquiry.ListPrice = model.ListPrice;
            inquiry.Discount = model.Discount;
            inquiry.NetPriceWithoutGst = model.NetPriceWithoutGST;
            inquiry.TotalPackage = model.TotalPackage;
            inquiry.Status = model.Status;
            inquiry.OfferStatus = model.OfferStatus;
            inquiry.UpdatedOn = DateTime.UtcNow;
            inquiry.UpdatedBy = model.UpdatedBy;

            // Remove old child records
            _context.TechnicalDetailsMappings.RemoveRange(inquiry.TechnicalDetailsMappings);

            // Add new child records
            inquiry.TechnicalDetailsMappings = model.TechicalDetailsMapping.Select(x => new TechnicalDetailsMapping
            {
                MotorType = x.MotorType,
                Kw = x.KW,
                Hp = x.HP,
                Phase = x.Phase,
                Pole = x.Pole,
                FrameSize = x.FrameSize,
                Dop = x.DOP,
                InsulationClass = x.InsulationClass,
                Efficiency = x.Efficiency,
                Voltage = x.Voltage,
                Frequency = x.Frequency,
                Quantity = x.Quantity,
                Mounting = x.Mounting,
                SafeAreaHazardousArea = x.SafeAreaHazardousArea,
                Brand = x.Brand,
                IfHazardousArea = x.IfHazardousArea,
                TempClass = x.TempClass,
                GasGroup = x.GasGroup,
                Zone = x.Zone,
                HardadousDescription = x.HardadousDescription,
                Duty = x.Duty,
                StartsPerHour = x.StartsPerHour,
                Cdf = x.CDF,
                AmbientTemp = x.AmbientTemp,
                TempRise = x.TempRise,
                Accessories = string.Join(",", x.Accessories),
                Brake = x.Brake,
                EncoderMounting = x.EncoderMounting,
                EncoderMountingIfYes = x.EncoderMountingIfYes,
                Application = x.Application,
                Segment = x.Segment,
                Narration = x.Narration,
                Amount = x.Amount,
                DeliveryTime = x.DeliveryTime,
            }).ToList();

            await _context.SaveChangesAsync();
        }

        public async Task DeleteInquiryAsync(int id)
        {
            var inquiry = await _context.Inquiries
                .Include(x => x.TechnicalDetailsMappings)
                .FirstOrDefaultAsync(x => x.InquiryId == id);

            if (inquiry == null)
                throw new Exception("Inquiry not found");

            _context.TechnicalDetailsMappings.RemoveRange(inquiry.TechnicalDetailsMappings);
            _context.Inquiries.Remove(inquiry);

            await _context.SaveChangesAsync();
        }
        // Service method to get a single file by its AttachmentId
        public async Task<InquiryAttachmentsRecordsViewModel> GetAttachmentByIdAsync(int attachmentId)
        {
            var attachment = await _context.InquiryAttachmentsRecords
                .Where(a => a.AttachmentId == attachmentId)
                .FirstOrDefaultAsync();

            if (attachment == null)
            {
                return null; // Or handle the case where the file is not found
            }

            return new InquiryAttachmentsRecordsViewModel
            {
                AttachmentId = attachment.AttachmentId,
                InquiryId = attachment.InquiryId,
                OriginalFileName = attachment.OriginalFileName,
                UniqueFileName = attachment.UniqueFileName,
                UploadedOn = (DateTime)attachment.UploadedOn
            };
        }
        public async Task<bool> DeleteAttachmentByIdAsync(int attachmentId, string fileStoragePath)
        {
            var fileRecord = await _context.InquiryAttachmentsRecords
                                           .FirstOrDefaultAsync(x => x.AttachmentId == attachmentId);

            if (fileRecord == null)
                return false;

            // Delete file from disk
            var fullFilePath = Path.Combine(fileStoragePath, fileRecord.UniqueFileName);
            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }

            // Remove record from database
            _context.InquiryAttachmentsRecords.Remove(fileRecord);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
