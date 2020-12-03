using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using System.IO;

namespace MRDB.Controllers
{
    public class FileController : Controller
    {
        // GET: FileController
        public ActionResult FileDownload()
        {
            ManagerFile managerFile = new ManagerFile();
            var fileCont = managerFile.GetFileConten("EbGu3", "Ivannita", "prueba1.txt");
            var OriginalnameFile = "prueba1.txt";
            var NewNameFile = OriginalnameFile.Substring(0, OriginalnameFile.Length - 4);
            var fileVirtualPath = $"{managerFile.FileAcction(NewNameFile, fileCont)}";
            byte[] content = System.IO.File.ReadAllBytes(fileVirtualPath);
            var name = Path.GetFileName(fileVirtualPath);
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }
}
