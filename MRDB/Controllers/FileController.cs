using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MRDB.Controllers
{
    public class FileController : Controller
    {
        // GET: FileController
        public ActionResult FileDownload()
        {
            var fileVirtualPath = string.Empty;
            ManagerFile managerFile = new ManagerFile();
            var fileCont = managerFile.GetFileConten("EbGu3", "Ivannita", "prueba1.txt");
            var OriginalnameFile = "prueba1.txt";
            var NewNameFile = OriginalnameFile.Substring(0, OriginalnameFile.Length - 4);
            fileVirtualPath = $"~/{managerFile.FileAcction(NewNameFile, fileCont)}";
            return File(fileVirtualPath, "application / force-download", Path.GetFileName(fileVirtualPath));
            
        }

        
    }
}
