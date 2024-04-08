using ManagementProject.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ManagementProject.UtilityServices
{
    public class HandleFile
    {
        public static async Task<List<AttachmentFile>> SaveFilesAsync(List<IFormFile> files, string folderPath)
        {
            var savedFiles = new List<AttachmentFile>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                //var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                AttachmentFile attachmentFile = new AttachmentFile()
                {
                    FileName = file.FileName,
                    FileType = "application/octet-stream",
                    FilePath = EncodeBase64(Path.Combine(folderPath, uniqueFileName))
                };

                savedFiles.Add(attachmentFile);
            }

            return savedFiles;
        }
        public static string EncodeBase64(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodeBase64(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
    
}
