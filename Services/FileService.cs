using System.Diagnostics;

namespace student_resource_hub.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string uploadFolder, long maxFileSize = 50 * 1024 * 1024);
        Task<bool> DeleteFileAsync(string filePath);
        Task<FileStream> GetFileStreamAsync(string filePath);
        bool ValidateFileUpload(IFormFile file, long maxFileSize = 50 * 1024 * 1024);
        string GetFileExtension(string fileName);
        string GetMimeType(string fileExtension);
        bool IsAllowedFileType(string fileExtension);
    }

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileService> _logger;
        private readonly string[] _allowedExtensions =
        {
            ".pdf", ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt", ".txt",
            ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".m4v", ".mov", ".avi",
            ".wmv", ".mkv", ".webm", ".flv", ".f4v", ".3gp", ".3g2", ".ogv",
            ".ts", ".mts", ".m2ts", ".vob", ".asf"
        };

        public FileService(IWebHostEnvironment env, ILogger<FileService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public bool ValidateFileUpload(IFormFile file, long maxFileSize = 50 * 1024 * 1024)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File validation failed: File is null or empty.");
                return false;
            }

            if (file.Length > maxFileSize)
            {
                _logger.LogWarning($"File validation failed: File size {file.Length} exceeds max size {maxFileSize}.");
                return false;
            }

            string extension = GetFileExtension(file.FileName);
            if (!IsAllowedFileType(extension))
            {
                _logger.LogWarning($"File validation failed: File type {extension} is not allowed.");
                return false;
            }

            return true;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string uploadFolder, long maxFileSize = 50 * 1024 * 1024)
        {
            try
            {
                if (!ValidateFileUpload(file, maxFileSize))
                {
                    throw new InvalidOperationException("File validation failed.");
                }

                // Create upload folder if it doesn't exist
                string uploadPath = Path.Combine(_env.WebRootPath, uploadFolder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Generate unique filename
                string fileExtension = GetFileExtension(file.FileName);
                string uniqueFileName = $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMdd_HHmmss}{fileExtension}";
                string filePath = Path.Combine(uploadPath, uniqueFileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation($"File uploaded successfully: {uniqueFileName}");
                return Path.Combine(uploadFolder, uniqueFileName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return false;
                }

                string fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/').Replace("/", "\\"));

                if (!File.Exists(fullPath))
                {
                    _logger.LogWarning($"File not found for deletion: {fullPath}");
                    return false;
                }

                File.Delete(fullPath);
                _logger.LogInformation($"File deleted successfully: {filePath}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting file: {ex.Message}");
                return false;
            }
        }

        public async Task<FileStream> GetFileStreamAsync(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/').Replace("/", "\\"));

                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }

                return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting file stream: {ex.Message}");
                throw;
            }
        }

        public string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName).ToLowerInvariant();
        }

        public string GetMimeType(string fileExtension)
        {
            return fileExtension.ToLowerInvariant() switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".mp4" => "video/mp4",
                ".m4v" => "video/x-m4v",
                ".mov" => "video/quicktime",
                ".avi" => "video/x-msvideo",
                ".wmv" => "video/x-ms-wmv",
                ".mkv" => "video/x-matroska",
                ".webm" => "video/webm",
                ".flv" => "video/x-flv",
                ".f4v" => "video/x-f4v",
                ".3gp" => "video/3gpp",
                ".3g2" => "video/3gpp2",
                ".ogv" => "video/ogg",
                ".ts" => "video/mp2t",
                ".mts" => "video/mp2t",
                ".m2ts" => "video/mp2t",
                ".vob" => "video/dvd",
                ".asf" => "video/x-ms-asf",
                _ => "application/octet-stream"
            };
        }

        public bool IsAllowedFileType(string fileExtension)
        {
            return _allowedExtensions.Contains(fileExtension.ToLowerInvariant());
        }
    }
}
