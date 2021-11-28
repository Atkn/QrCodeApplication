using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QrCodeApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;

namespace QrCodeApplication.Controllers
{
    public class FormModel
    {
        public string Url { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FormModel model)
        {
            string qrCodeString = model.Url;
            var writter = new QRCodeWriter();
            var resultBit = writter.encode(qrCodeString, BarcodeFormat.QR_CODE,200, 200);
            var matrix = resultBit;
            int scale = 2;
            Bitmap result = new Bitmap(matrix.Width * scale, matrix.Height * scale);
            for (int x = 0; x < matrix.Height; x++)
            {
                for (int y = 0; y < matrix.Width; y++)
                {
                    Color pixel = matrix[x, y] ? Color.Black : Color.White;
                    for (int i = 0; i < scale; i++)
                    {
                        for (int j = 0; j < scale; j++)
                            result.SetPixel(x * scale + i, y * scale + j, pixel);
                    }
                }
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            result.Save(webRootPath + "\\Images\\Qrcodenew.png"+model);
            ViewBag.Url = "\\Images\\Qrcodenew.png";
            ViewBag.InputValue = qrCodeString;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
