using Gedik.ImageClassification.Service.Models;
using Gedik_ImageClassificationML.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gedik.ImageClassification.Service.Controllers
{
    [ApiController]
    public class ClassificationController : Controller
    {
        private IHostingEnvironment _env;

        public ClassificationController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Route("v1/classification/image")]
        [HttpPost]
        public ServiceResult<ModelOutput> Index(ClassificationRequestModel request)
        {
            try
            {
                string base64 = request.ImageBase64;
                string filename = Guid.NewGuid().ToString() + ".jpg";
                SaveImage(base64, filename);

                var input = new ModelInput();
                input.ImageSource = System.IO.Path.Combine(_env.ContentRootPath, "Images") + "\\" + filename;
                ModelOutput result = ConsumeModel.Predict(input);
                DeleteImage(filename);
                return new ServiceResult<ModelOutput>() { 
                    Data = result,  
                    Message = "", 
                    Status = true 
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<ModelOutput>()
                {
                    Data = null,
                    Message = ex.Message,
                    Status = false
                };
            }

        }

        private void SaveImage(string base64img, string outputImgFilename = "image.jpg")
        {
            var folderPath = System.IO.Path.Combine(_env.ContentRootPath, "Images");
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            System.IO.File.WriteAllBytes(Path.Combine(folderPath, outputImgFilename), Convert.FromBase64String(base64img));
        }

        private void DeleteImage(string outputImgFilename = "image.jpg")
        {
            var folderPath = System.IO.Path.Combine(_env.ContentRootPath, "Images");
            if (System.IO.Directory.Exists(folderPath))
            {
                System.IO.File.Delete(Path.Combine(folderPath, outputImgFilename));
            }
        }


    }
}
