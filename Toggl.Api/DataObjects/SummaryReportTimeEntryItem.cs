namespace Toggl.Api.DataObjects
{
	using Newtonsoft.Json;
	public class SummaryReportTimeEntryItem : BaseDataObject
	{
		[JsonProperty(PropertyName = "title")]
		public ReportTitle? Title { get; set; }

		[JsonProperty(PropertyName = "time")]
		public long? Time { get; set; }

		[JsonProperty(PropertyName = "cur")]
		public string? CurrencyCode { get; set; }

		[JsonProperty(PropertyName = "sum")]
		public long? Sum { get; set; }

		[JsonProperty(PropertyName = "rate")]
		public long? Rate { get; set; }
	}
}
