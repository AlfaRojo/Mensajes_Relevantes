using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using System.IO;
using Mensajes_Relevantes.Models;

namespace MRDB.Controllers
{
    public class FileController : Controller
    {
        // GET: FileController
        public ActionResult FileDownload(Message messages)
        {
            ManagerFile managerFile = new ManagerFile();
            var fileCont = managerFile.GetFileConten(messages.emisor, messages.receptor, messages.file_Name);
            var fileVirtualPath = $"{managerFile.FileAcction(messages.file_Name, fileCont)}";
            byte[] content = System.IO.File.ReadAllBytes(fileVirtualPath);
            var name = Path.GetFileName(fileVirtualPath);
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }
}
