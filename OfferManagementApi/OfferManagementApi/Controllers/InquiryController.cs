using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfferManagementApi.Models;
using OfferManagementApi.Services;
using DinkToPdf.Contracts;
using SelectPdf;
using System.Text;
using Microsoft.SqlServer.Server;


namespace OfferManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquiryController : ControllerBase
    {
        private readonly IInquiryService _inquiryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<InquiryController> _logger;


        public InquiryController(IInquiryService inquiryService, IWebHostEnvironment webHostEnvironment, ILogger<InquiryController> logger)
        {
            _inquiryService = inquiryService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        //[HttpPost]
        //public async Task<IActionResult> SaveInquiry([FromBody] InquiryViewModel model)
        //{
        //    await _inquiryService.SaveInquiryAsync(model);
        //    return Ok(new { message = "Inquiry saved successfully." });
        //}
        [HttpPost]
        public async Task<IActionResult> SaveInquiry()
        {
            try
            {
                var form = HttpContext.Request.Form;
                InquiryViewModel model = null;
                model = JsonConvert.DeserializeObject<InquiryViewModel>(HttpContext.Request.Form["model"]);
                var inquiry = await _inquiryService.SaveInquiryAsync(model);

                // Create upload directory if not exists
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Loop through all uploaded files
                foreach (var file in form.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        string fileExtension = Path.GetExtension(file.FileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        var filePath = Path.Combine(uploadDir, uniqueFileName);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);
                        var attachment = new InquiryAttachmentsRecordsViewModel
                        {
                            InquiryId = inquiry,
                            OriginalFileName = file.FileName,
                            UniqueFileName = uniqueFileName,
                        };
                        await _inquiryService.SaveAttchmentRecord(attachment);
                    }
                }

                return Ok(new { message = "Inquiry saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", detail = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllInquiries()
        {
            var inquiries = await _inquiryService.GetAllInquiriesAsync();
            return Ok(inquiries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInquiryById(int id)
        {
            var inquiry = await _inquiryService.GetInquiryByIdAsync(id);
            if (inquiry == null)
                return NotFound();

            return Ok(inquiry);
        }

        [HttpPut]
        //public async Task<IActionResult> UpdateInquiry([FromBody] InquiryViewModel model)
        //{
        //    await _inquiryService.UpdateInquiryAsync(model);
        //    return Ok(new { message = "Inquiry updated successfully." });
        //}

        [HttpPut]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateInquiry()
        {
            try
            {
                var form = HttpContext.Request.Form;

                // Deserialize the model from form
                var model = JsonConvert.DeserializeObject<InquiryViewModel>(form["model"]);

                // Create upload directory if not exists
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Loop through all uploaded files
                foreach (var file in form.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        string fileExtension = Path.GetExtension(file.FileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        var filePath = Path.Combine(uploadDir, uniqueFileName);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);
                        var attachment = new InquiryAttachmentsRecordsViewModel
                        {
                            InquiryId = model.InquiryId,
                            OriginalFileName = file.FileName,
                            UniqueFileName = uniqueFileName,
                        };
                        await _inquiryService.SaveAttchmentRecord(attachment);
                    }
                }

                await _inquiryService.UpdateInquiryAsync(model);
                return Ok(new { message = "Inquiry updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", detail = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteInquiry(int id)
        {
            await _inquiryService.DeleteInquiryAsync(id);
            return Ok(new { message = "Inquiry deleted successfully." });
        }

        [HttpGet("getFileById/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {

                var attachment = await _inquiryService.GetAttachmentByIdAsync(id);

                if (attachment == null)
                {
                    return NotFound("Attachment not found.");
                }
                var _fileStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                var filePath = Path.Combine(_fileStoragePath, attachment.UniqueFileName);

                // Check if the file exists
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File not found.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Set the content type for the response (you can change it based on your file type)
                var contentType = "application/octet-stream"; // You can also use specific types like "application/pdf" for PDFs.

                // Set the file name for the download
                var fileDownloadName = attachment.OriginalFileName;

                // Return the file as a downloadable attachment
                return File(fileBytes, contentType, fileDownloadName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("deleteFile/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            // Delete the file from the storage
            var _fileStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            var result = await _inquiryService.DeleteAttachmentByIdAsync(id, _fileStoragePath);

            if (!result)
                return NotFound("File not found.");

            return Ok("File deleted successfully.");
        }




        [HttpPost("downloadPdf")]
        public async Task<IActionResult> DownloadPdf()
        {
            try
            {
                _logger.LogInformation("Hello");
                var form = HttpContext.Request.Form;
                var model = JsonConvert.DeserializeObject<InquiryViewModel>(form["model"]);
                string dir = Directory.GetCurrentDirectory();
                string templatePath = Path.Combine(dir, "samplePdf", "samplePdf.html");
                string saveAs = $"Compliance_Certificate_{DateTime.Now:MMddyyyy_HHmmss}.pdf";
                string outputPath = Path.Combine(_webHostEnvironment.ContentRootPath, "outputPdf", saveAs);

                // Load HTML template
                string htmlBody;
                using (StreamReader sr = new StreamReader(templatePath))
                {
                    htmlBody = await sr.ReadToEndAsync();
                }

                // Inject the technical details table into the placeholder
                string technicalTableHtml = BuildTechnicalDetailsHtml(model.TechicalDetailsMapping);
                htmlBody = htmlBody.Replace("#enquiryNo#", model.EnquiryNo);
                htmlBody = htmlBody.Replace("#date#", DateTime.Now.ToString("dd/MMM/yyyy"));
                htmlBody = htmlBody.Replace("#customerName#", model.CustomerName);
                htmlBody = htmlBody.Replace("#cpName#", model.Salutation + " " + model.CpfirstName + " " + model.CplastName);
                htmlBody = htmlBody.Replace("#enquiryDate#", model.EnquiryDate.ToString("dd/MMM/yyyy"));
                htmlBody = htmlBody.Replace("{{technicalDetails}}", technicalTableHtml);

                SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.Custom;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.MarginBottom = 30;
                converter.Options.MarginTop = 30;
                converter.Options.MarginLeft = 30;
                converter.Options.MarginRight = 30;
                Console.WriteLine(htmlBody);
                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlBody);
                byte[] pdf = doc.Save();
                doc.Close();
                // Assuming you have the path to the generated PDF file
                string pdfFilePath = outputPath; // Replace this with the actual path

                string mimeType = "application/pdf";
                byte[] pdfBytes = pdf;
                System.IO.File.WriteAllBytes(pdfFilePath, pdfBytes);
                string fileName = System.IO.Path.GetFileName(pdfFilePath);
                // Optionally, you can delete the file after reading its content
                System.IO.File.Delete(pdfFilePath);

                return File(pdfBytes, mimeType, fileName);

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                // Optionally log the exception
                ModelState.AddModelError("ERROR", "Error while generating PDF: " + ex.Message);
                return BadRequest(ModelState);
            }
        }

        private string BuildTechnicalDetailsHtml(List<TechnicalDetailsMappingViewModel> details)
        {
            if (details == null || !details.Any()) return "";

            var sb = new StringBuilder();

            sb.AppendLine("<div style='min-width: 1000px;  font-family: Arial, sans-serif;'>");

            // Header row
            sb.AppendLine(@"
    <div class='technical-header' style='display: flex; font-weight: bold;  border-top: 1px solid #ccc; background-color: #f0f0f0;'>
        <div style='width: 100px; padding: 8px;'>Sr.No.</div>
        <div style='width: 300px; padding: 8px;'>Narration</div>
        <div style='width: 300px; padding: 8px;'>Delivery Time</div>
        <div style='width: 100px; padding: 6px;'>Quantity</div>
        <div style='width: 100px; padding: 6px;'>Unit Price (INR)</div>
        <div style='width: 100px; padding: 6px;'>Total Amount (INR)</div>
    </div>");

            // Data rows
            foreach (var item in details)
            {
                sb.AppendLine($@"
        <div class='technical-row' style='display: flex;'>
            <div style='width: 100px; padding: 8px;'>{item.RowIndex}</div>
            <div style='width: 300px; padding: 8px;'>{item.Narration}</div>
            <div style='width: 300px; padding: 8px;'>{item.DeliveryTime}</div>
            <div style='width: 100px; padding: 6px;'>{item.Quantity}</div>
            <div style='width: 100px; padding: 6px;'>{item.Amount:C}</div>
            <div style='width: 100px; padding: 6px;'>{item.TotalAmount:C}</div>
        </div>");
            }

            sb.AppendLine("</div>");
            return sb.ToString();
        }


    }
}

