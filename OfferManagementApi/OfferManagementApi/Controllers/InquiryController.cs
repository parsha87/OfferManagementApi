using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfferManagementApi.Models;
using OfferManagementApi.Services;

namespace OfferManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquiryController : ControllerBase
    {
        private readonly IInquiryService _inquiryService;

        public InquiryController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
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


    }
}

