using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using VaultixAdmin.ServerAPIs;

namespace VaultixAdmin.Controllers
{
    public class HomeController : Controller
    {
        public class Train
        {
            public List<IFormFile> Files { get; set; }
            public string Classifier { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> UploadFiles(Train input)
        {
            long size = input.Files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in input.Files)
            {
                if (formFile.Length > 0)
                {
                    foreach (IFormFile file in input.Files)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            // copy the file data into the memory stream that was created
                            await file.CopyToAsync(memoryStream);
                            // Convert memory stream to array and parse it into the uploaded image byte array 
                            byte[] uploadedImage = memoryStream.ToArray();
                            // Convert the byte array as a Base64 String
                            string base64String = Convert.ToBase64String(uploadedImage);
                            var url = await API.HttpTrainNeuralNetwork_MethodAsync(input.Classifier, base64String);
                        }
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new {size, filePath });
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
