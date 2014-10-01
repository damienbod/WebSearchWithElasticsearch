using System;
using System.ComponentModel.DataAnnotations;

namespace WebSearchWithElasticsearch.Search
{
	public class Skill
	{
		public long Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		public DateTimeOffset Created { get; set; }
		public DateTimeOffset Updated { get; set; }
	}
}