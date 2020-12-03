using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using System.IO;

namespace MRDB.Controllers
{
    public class FileController : Controller
    {
        // GET: FileController
        public ActionResult FileDownload(string emisor, string receptor, string file_Name)
        {
            ManagerFile managerFile = new ManagerFile();
            var fileCont = managerFile.GetFileConten(emisor, receptor, file_Name);
            var fileVirtualPath = $"{managerFile.FileAcction(file_Name, fileCont)}";
            byte[] content = System.IO.File.ReadAllBytes(fileVirtualPath);
            var name = Path.GetFileName(fileVirtualPath);
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }
}
