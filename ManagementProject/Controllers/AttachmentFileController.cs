using ManagementProject.Interfaces;
using ManagementProject.UtilityServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;

namespace ManagementProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentFileController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttachmentFileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{filePath}")]
        public IActionResult DownloadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path is required.");
            }

            var fullPath = HandleFile.DecodeBase64(filePath);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("File not found.");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", Path.GetFileName(fullPath));
        }

        [HttpGet("all/{workId}")]
        public async Task<IActionResult> DownloadAllFiles(int workId)
        {
            var listFileAttachment = await _unitOfWork.AttachmentFiles.GetAllFileByWork(workId);
            if (listFileAttachment.IsNullOrEmpty())
            {
                return NotFound();
            }

            var zipMemory = new MemoryStream();

            using (var archive = new ZipArchive(zipMemory, ZipArchiveMode.Create, true))
            {
                foreach (var fileAttachment in listFileAttachment)
                {
                    var fullPath = HandleFile.DecodeBase64(fileAttachment.FilePath); // Giả sử FilePath là một chuỗi mã hóa Base64 của đường dẫn

                    if (System.IO.File.Exists(fullPath))
                    {
                        var fileInfo = new FileInfo(fullPath);
                        var entry = archive.CreateEntry(fileInfo.Name, CompressionLevel.Optimal);

                        using (var entryStream = entry.Open())
                        using (var fileStream = new FileStream(fullPath, FileMode.Open))
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }
            }

            zipMemory.Position = 0;

            return File(zipMemory, "application/octet-stream", "AllFiles.zip");
        }
    }
}
