using EduLog.CoreLayer.Services.FileManager;
using EduLog.CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EduLog.Web.Areas.AdminPanel.Controllers
{
    public class UploadController : Controller
    {
        private readonly IFileManager fileManager;
        public UploadController(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        [Route("/Upload/Article")]
        public IActionResult UploadArticelImage(IFormFile upload)
        {
            if (upload == null)
                return BadRequest();

            var imageName = fileManager.SaveFileAndReturnName(upload, Directories.PostContentImage);
            return Json(new
            {
                Uploaded = true,
                url = Directories.GetPostContentImage(imageName)
            });
        }
    }
}
