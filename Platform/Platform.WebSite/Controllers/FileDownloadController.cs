using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Platform.FileSystem;

namespace Platform.WebSite.Controllers
{
    public class FileDownloadController : Controller
    {
        private MediaFileManager _manager = new MediaFileManager();

        // GET: FileDownload/Preview/{id}
        public ActionResult Preview(Guid id)
        {
            var model = this._manager.GetMediaFile(id);
            if (model == null)
                return HttpNotFound();

            string path = _manager.GetMediaFilePath(model);
            string mime = model.MimeType;
            if (!System.IO.File.Exists(path))
                return HttpNotFound();

            return File(path, mime);
        }

        // GET: FileDownload/Download/{id}
        public ActionResult Download(Guid id)
        {
            var model = this._manager.GetMediaFile(id);
            if (model == null)
                return HttpNotFound();

            string path = _manager.GetMediaFilePath(model);
            string fileName = model.OutputFileName;
            string mime = model.MimeType;
            if (!System.IO.File.Exists(path))
                return HttpNotFound();

            return File(path, mime, fileName);
        }
    }
}
