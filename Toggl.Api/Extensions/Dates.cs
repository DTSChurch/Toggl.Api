namespace Toggl.Api.Extensions
{
	using System;

	public static class Dates
	{
		public static string ToIsoDateStr(this DateTime date) => date.ToString("yyyy-MM-ddTHH:mm:sszzz");

		public static long ToUnixTime(this DateTime date) => (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
	}
}

