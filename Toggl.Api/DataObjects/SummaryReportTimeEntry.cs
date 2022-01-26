using System.Collections.Generic;

namespace Toggl.Api.DataObjects
{
	using Newtonsoft.Json;
	public class SummaryReportTimeEntry : BaseDataObject
	{
		[JsonProperty(PropertyName = "id")]
		public long? Id { get; set; }

		[JsonProperty(PropertyName = "title")]
		public ReportTitle? Title { get; set; }

		[JsonProperty(PropertyName = "time")]
		public long? Time { get; set; }

		[JsonProperty(PropertyName = "total_currencies")]
		public List<ReportCurrency>? currencies { get; set; }

		[JsonProperty(PropertyName = "items")]
		public List<SummaryReportTimeEntryItem>? Items { get; set; }
	}
}
