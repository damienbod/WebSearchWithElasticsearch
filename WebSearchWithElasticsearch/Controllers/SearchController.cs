using System;
using System.Web.Mvc;
using ElasticsearchCRUD;
using WebSearchWithElasticsearch.Search;

namespace WebSearchWithElasticsearch.Controllers
{
	public class SearchController : Controller
	{
		ISearchProvider _searchProvider = new ElasticSearchProvider();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult AddUpdate(Skill model)
		{
			model.Created = DateTime.UtcNow;
			model.Updated = DateTime.UtcNow;
			_searchProvider.AddUpdateEntity(model);

			return Redirect("Index");
		}

		public JsonResult Search(string term)
		{			
			
			return Json(_searchProvider.QueryString(term), "Skills", JsonRequestBehavior.AllowGet);
		}
	}
}
