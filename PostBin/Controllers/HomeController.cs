using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PostBin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(string id)
        {
            //Set the data to be used inside of the view
            ViewBag.Bins = Database.GetBins().OrderBy(x=>x.Name);
            ViewBag.Id = id;

            return View();
        }

        [ChildActionOnly]
        public ActionResult Details(string id)
        {
            if(string.IsNullOrEmpty(id))
                return Content("");

            var bin = Database.GetBin(id);

            if (bin == null)
                return Content(string.Format("Sorry the bin {0} was not found.", id));

            ViewBag.Bin = bin;
            ViewBag.Posts = PostBin.Post.CreateFromDatabase(Database.GetPosts(bin.Id));

            return View();
        }

        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            Database.DeleteBin(id);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Post(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");
           
            var bin = Database.GetBin(id);

            if (bin == null)
                Database.CreateBin(id);

            bin = Database.GetBin(id);

            var post = new Post(Request);

            var serializedPost = post.Serialize();

            Database.SavePost(bin.Id, serializedPost);

            return RedirectToAction("Index", new { id = id });
        }
    }
}
