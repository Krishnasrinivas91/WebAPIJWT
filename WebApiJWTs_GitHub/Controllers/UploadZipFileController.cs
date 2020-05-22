using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace WebApiJWTs_GitHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadZipFileController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UploadZipFileAsync()
        {
            if (Request != null && Request.Form != null)
            {
                var file = Request.Form.Files[0];
                string PathToSave = @"C:\Users\Seenu\Pictures\WriteFromVS2017\" + Request.Form.Files[0].FileName;
                if (file.Length > 0)
                {
                    using (var stream = new FileStream(PathToSave, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        return Ok("File Uploaded");
                    }

                }
            }
            return BadRequest("No File Uploaded");
        }
    }
}