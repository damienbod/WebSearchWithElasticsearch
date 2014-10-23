using System;
using System.Collections.Generic;
using System.Text;
using ElasticsearchCRUD;

namespace WebSearchWithElasticsearch.Search
{
	public class ElasticSearchProvider : ISearchProvider, IDisposable
	{
		public ElasticSearchProvider()
		{
			_context = new ElasticSearchContext(ConnectionString, _elasticSearchMappingResolver);
		}

		private const string ConnectionString = "http://localhost:9200/";
		private readonly IElasticSearchMappingResolver _elasticSearchMappingResolver = new ElasticSearchMappingResolver();
		private readonly ElasticSearchContext _context;

		public IEnumerable<Skill> QueryString(string term)
		{
			return _context.Search<Skill>(BuildQueryStringSearch(term));
		}

		private string BuildQueryStringSearch(string term)
		{
			/*			
			{
			  "query": {
						"query_string": {
						   "query": "*"

						}
					}
			}
			 */
			var names = "";
			if (term != null)
			{
				names = term.Replace("+", " OR *");
			}

			var buildJson = new StringBuilder();
			buildJson.AppendLine("{");
			buildJson.AppendLine(" \"query\": {");
			buildJson.AppendLine("   \"query_string\": {");
			buildJson.AppendLine("      \"query\": \"" + names  + "*\"");
			buildJson.AppendLine("     }");
			buildJson.AppendLine("  }");
			buildJson.AppendLine("}");

			return buildJson.ToString();
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