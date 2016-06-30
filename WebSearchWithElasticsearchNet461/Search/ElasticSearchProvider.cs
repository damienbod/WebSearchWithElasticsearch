using System;
using System.Collections.Generic;
using System.Linq;
using ElasticsearchCRUD;
using ElasticsearchCRUD.Model.SearchModel;
using ElasticsearchCRUD.Model.SearchModel.Queries;

namespace WebSearchWithElasticsearch.Search
{
	public class ElasticSearchProvider : ISearchProvider, IDisposable
	{
		public ElasticSearchProvider()
		{
			_context = new ElasticsearchContext(ConnectionString, _elasticSearchMappingResolver);
		}

		private const string ConnectionString = "http://localhost:9200/";
		private readonly IElasticsearchMappingResolver _elasticSearchMappingResolver = new ElasticsearchMappingResolver();
		private readonly ElasticsearchContext _context;

		public IEnumerable<Skill> QueryString(string term)
		{
			var results = _context.Search<Skill>(BuildQueryStringSearch(term));
			 return results.PayloadResult.Hits.HitsResult.Select(t => t.Source);
		}

		/*			
		{
		  "query": {
					"query_string": {
					   "query": "*"

					}
				}
		}
		 */
		private ElasticsearchCRUD.Model.SearchModel.Search BuildQueryStringSearch(string term)
		{
			var names = "";
			if (term != null)
			{
				names = term.Replace("+", " OR *");
			}

			var search = new ElasticsearchCRUD.Model.SearchModel.Search
			{
				Query = new Query(new QueryStringQuery(names + "*"))
			};

			return search;
		}

		public void AddUpdateEntity(Skill skill)
		{
			_context.AddUpdateDocument(skill, skill.Id);
			_context.SaveChanges();
		}

		public void UpdateSkill(long updateId, string updateName, string updateDescription)
		{
			var skill = _context.GetDocument<Skill>(updateId);
			skill.Updated = DateTime.UtcNow;
			skill.Name = updateName;
			skill.Description = updateDescription;
			_context.AddUpdateDocument(skill, skill.Id);
			_context.SaveChanges();
		}

		public void DeleteSkill(long deleteId)
		{
			_context.DeleteDocument<Skill>(deleteId);
			_context.SaveChanges();
		}

		private bool isDisposed;
		public void Dispose()
		{
			if (isDisposed)
			{
				isDisposed = true;
				_context.Dispose();
			}
		}
	}
}