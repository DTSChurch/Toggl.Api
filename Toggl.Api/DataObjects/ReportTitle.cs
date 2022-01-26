namespace Toggl.Api.DataObjects
{
	using Newtonsoft.Json;
	public class ReportTitle : BaseDataObject
	{
		[JsonProperty(PropertyName = "project")]
		public string? Project { get; set; }

		[JsonProperty(PropertyName = "client")]
		public string? Client { get; set; }

		[JsonProperty(PropertyName = "user")]
		public string? User { get; set; }

		[JsonProperty(PropertyName = "task")]
		public string? Task { get; set; }

		[JsonProperty(PropertyName = "time_entry")]
		public string? TimeEntry { get; set; }

		[JsonProperty(PropertyName = "color_hex")]
		public string? Color { get; set; }
	}
}
