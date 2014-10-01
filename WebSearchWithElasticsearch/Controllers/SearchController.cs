using System;
using System.Web.Mvc;
using WebSearchWithElasticsearch.Search;

namespace WebSearchWithElasticsearch.Controllers
{
	public class SearchController : Controller
	{
		readonly ISearchProvider _searchProvider = new ElasticSearchProvider();

		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(Skill model)
		{
			if (ModelState.IsValid)
			{
				model.Created = DateTime.UtcNow;
				model.Updated = DateTime.UtcNow;
				_searchProvider.AddUpdateEntity(model);

				return Redirect("Search/Index");
			}

			return View("Index", model);
		}

		[HttpPost]
		public ActionResult Update(long updateId, string updateName, string updateDescription)
		{
			_searchProvider.UpdateSkill(updateId, updateName, updateDescription);
			return Redirect("Index");
		}

		public JsonResult Search(string term)
		{			
			return Json(_searchProvider.QueryString(term), "Skills", JsonRequestBehavior.AllowGet);
		}
	}
}
