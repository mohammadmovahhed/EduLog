using Microsoft.AspNetCore.Http;

namespace EduLog.CoreLayer.Services.FileManager
{
    public interface IFileManager
    {
        string SaveFileAndReturnName(IFormFile file, string savePath);
        void DeleteFile(string fileName, string path);
    }
}