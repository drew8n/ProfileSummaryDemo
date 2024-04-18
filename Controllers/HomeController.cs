using Microsoft.AspNetCore.Mvc;
using ProfileSummaryDemo.Models;
using System.Diagnostics;

namespace ProfileSummaryDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AiUtil _ai;

        public HomeController(ILogger<HomeController> logger, AiUtil aiUtil)
        {
            _logger = logger;
            _ai = aiUtil;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SummariseContent(string content)
        {
            string result = await _ai.SummariseContent(content);
            ViewBag.Result = result;
            ViewBag.OriginalContent = content;
            return View("SummariseContent");
        }

        [HttpPost]
        public async Task<IActionResult> SummariseUrl(string url)
        {
            var content = await UrlLoader.Get(url);
            if (!string.IsNullOrEmpty(content))
            {
                return await SummariseContent(content);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SummariseFile(List<IFormFile> files)
        {
            string content = null;
            long size = files.Sum(f => f.Length);
            if (files[0].Length > 0)
            {
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await files[0].CopyToAsync(stream);
                }
                content = PdfReaderUtil.Get(filePath);

                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(content))
            {
                return await SummariseContent(content);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Followup(string question, string originalContent)
        {
            string result = await _ai.FollowUp(question, originalContent);
            ViewBag.Result = result;
            ViewBag.OriginalContent = originalContent;
            return View("SummariseContent");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
