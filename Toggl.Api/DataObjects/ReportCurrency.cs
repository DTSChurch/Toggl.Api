namespace Toggl.Api.DataObjects
{
	using Newtonsoft.Json;
	public class ReportCurrency : BaseDataObject
	{
		[JsonProperty(PropertyName = "currency")]
		public string? CurrencyCode { get; set; }

		[JsonProperty(PropertyName = "amount")]
		public long? Amount { get; set; }
	}
}
