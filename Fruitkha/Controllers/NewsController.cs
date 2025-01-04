using Business.Services.News;
using Business.ViewModels.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Controllers
{
    [Authorize(Roles = "Admin, Customer, Seller")]
    public class NewsController : Controller
    {
        readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var news = await _newsService.GetAllNewsVMAsync();
            return View(news);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            var news = await _newsService.GetNewsByIdVMAsync(id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateNewsVM model)
        {
            if (await _newsService.AddNewsAsync(model))
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var news = await _newsService.UpdateNewsVMAsync(id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        [HttpPost]
        public async Task<ActionResult> Update(int id, UpdateNewsVM model)
        {
            if (await _newsService.UpdateNewsAsync(id, model))
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            if (await _newsService.DeleteNewsAsync(id))
                return RedirectToAction("Index");

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> WriteComment(int newsId, string comment)
        {
            if (await _newsService.WriteCommentAsync(newsId, comment))
                return RedirectToAction("Details", new { id = newsId });

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> WriteReply(int newsId, int commentId, string reply)
        {
            if (await _newsService.WriteReplyAsync(newsId, commentId, reply))
                return RedirectToAction("Details", new { id = newsId });

            return NotFound();
        }
    }
}
