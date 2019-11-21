using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalefornia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCalefornia.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly ExploreCaleforiniaContext context;

        public BlogController(ExploreCaleforiniaContext context)
        {
            this.context = context;
        }

        [Route("")]
        // GET: /<controller>/
        public IActionResult Index(int page=0)
        {

            var pageSize = 2;
            var totalPosts = context.Posts.Count();
            var totalPages = totalPosts / pageSize;

            var previousPage = page - 1;

            var nextPage = page + 1;

            ViewBag.previousPage = previousPage;

            ViewBag.HaspreviousPage = previousPage >= 0;

            ViewBag.nextPage = nextPage;
            ViewBag.HasnextPage = nextPage < totalPages;

            List<Post> postList = context.Posts.OrderByDescending(s => s.Id)
                .Skip(pageSize * page).Take(pageSize).ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView(postList);
            return View(postList);
        }


        [Route("{year:min(2011)}/{month:range(1,12)}/{Key}-Written-by-{Auther}")]
        public IActionResult PostBlog(int year, int month, string key,string Auther){Post model = context.Posts.FirstOrDefault(s => s.Key == key);return View(model);}

     
        [Authorize]
        [HttpGet, Route("Create")]  
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost, Route("Create")]
        public IActionResult Create(Post model)
        {
            if (!ModelState.IsValid)
                return View();

            model.Auther = User.Identity.Name==null ? "Unknown":null;
            model.PostedDate = DateTime.Now;
            model.Key = model.Title.Replace(" ","-");
            context.Posts.Add(model);
            context.SaveChanges();
            return View();
        }
    }
}
