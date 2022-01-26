using Newtonsoft.Json;

namespace Toggl.Api.QueryObjects
{
	public class SummaryReportParams : ReportParams
	{
		[JsonProperty(PropertyName = "grouping")]
		public string? Group { get; set; } = Grouping.Projects;

		[JsonProperty(PropertyName = "subgrouping")]
		public string? Subgroup { get; set; } = Subgrouping.TimeEntries;

		public static class Grouping
		{
			public const string Projects = "projects";
			public const string Clients = "clients";
			public const string Users = "users";
		}

		public static class Subgrouping
		{
			public const string TimeEntries = "time_entries";
			public const string Tasks = "tasks";
			public const string Projects = "projects";
			public const string Users = "users";
			public const string Clients = "clients";
		}
	}
}
