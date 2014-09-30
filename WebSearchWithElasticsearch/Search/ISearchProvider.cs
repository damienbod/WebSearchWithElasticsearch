using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebSearchWithElasticsearch.Search
{
	public interface ISearchProvider
	{
		IEnumerable<Skill> QueryString(string term);

		void AddUpdateEntity(Skill skill);
	}
}