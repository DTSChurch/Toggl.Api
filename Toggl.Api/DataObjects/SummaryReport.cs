using System.Collections.Generic;

namespace Toggl.Api.DataObjects
{
	using Newtonsoft.Json;
	public class SummaryReport : Report
	{
		[JsonProperty(PropertyName = "total_currencies")]
		public List<ReportCurrency>? Currencies { get; set; }

		[JsonProperty(PropertyName = "data")]
		public List<SummaryReportTimeEntry>? Data { get; set; }
	}
}
