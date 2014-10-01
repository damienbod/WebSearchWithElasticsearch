using System;
using System.Web.Mvc;
using WebSearchWithElasticsearch.Search;

namespace WebSearchWithElasticsearch.Controllers
{
	public class SearchController : Controller
	{
		readonly ISearchProvider _searchProvider = new ElasticSearchProvider();
		public ActionResult Index(Skill model)
		{
			model = new Skill();
			return View(model);
		}

		public ActionResult AddUpdate(Skill model)
		{
			if (ModelState.IsValid)
			{
				model.Created = DateTime.UtcNow;
				model.Updated = DateTime.UtcNow;
				_searchProvider.AddUpdateEntity(model);

				return View("Index", model);
			}
			return Redirect("Index");
			
		}

		public JsonResult Search(string term)
		{			
			return Json(_searchProvider.QueryString(term), "Skills", JsonRequestBehavior.AllowGet);
		}
	}
}
