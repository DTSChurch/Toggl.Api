﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toggl.Api.DataObjects;
using Toggl.Api.Interfaces;
using Toggl.Api.Routes;

namespace Toggl.Api.Services
{
	using System.Net;

	public class TimeEntryServiceAsync : ITimeEntryServiceAsync
	{
		private IApiServiceAsync TogglSrv { get; set; }

		public TimeEntryServiceAsync(string apiKey)
			: this(new ApiServiceAsync(apiKey))
		{
		}

		public TimeEntryServiceAsync(IApiServiceAsync srv)
		{
			TogglSrv = srv;
		}

		/// <summary>
		/// List recent time entries
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/time_entries.md#get-time-entries-started-in-a-specific-time-range
		/// </summary>
		public /* async */ Task<List<TimeEntry>> ListRecent() => throw new NotImplementedException();

		public async Task<List<TimeEntry>> List() => await List(new QueryObjects.TimeEntryParams()).ConfigureAwait(false);

		/// <summary>
		/// List time entries
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/time_entries.md#get-time-entries-started-in-a-specific-time-range
		/// </summary>
		/// <param name="obj"></param>
		public async Task<List<TimeEntry>> List(QueryObjects.TimeEntryParams obj)
		{
			var response = await TogglSrv.Get(ApiRoutes.TimeEntry.TimeEntriesUrl, obj.GetParameters()).ConfigureAwait(false);
			var entries = response
				.GetData<List<TimeEntry>>()
				.AsQueryable();

			if (obj.ProjectId.HasValue)
				entries = entries.Where(w => w.ProjectId == obj.ProjectId);

			return entries.Select(s => s).ToList();
		}

		/// <summary>
		/// Get the Current time entry
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/time_entries.md#get-time-entry-details
		/// </summary>
		public async Task<TimeEntry> Current()
		{
			var response = await TogglSrv.Get(ApiRoutes.TimeEntry.TimeEntryCurrentUrl).ConfigureAwait(false);
			return response.GetData<TimeEntry>();
		}

		/// <summary>
		/// Get a time entry
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/time_entries.md#get-time-entry-details
		/// </summary>
		/// <param name="id"></param>
		public async Task<TimeEntry> Get(long id)
		{
			var response = await TogglSrv.Get(string.Format(ApiRoutes.TimeEntry.TimeEntryUrl, id)).ConfigureAwait(false);
			return response.GetData<TimeEntry>();
		}

		/// <summary>
		/// Add a time entry
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/time_entries.md#create-a-time-entry
		/// </summary>
		/// <param name="obj"></param>
		public async Task<TimeEntry> Add(TimeEntry obj)
		{
			var response = await TogglSrv
				.Post(ApiRoutes.TimeEntry.TimeEntriesUrl, obj.ToJson())
				.ConfigureAwait(false);
			var timeEntry = response.GetData<TimeEntry>();

			return timeEntry;
		}

		/// <summary>
		/// Start a TimeEntry
		/// </summary>
		/// <param name="obj">A TimeEntry object.</param>
		/// <returns>The runnig TimeEntry.</returns>
		public async Task<TimeEntry> Start(TimeEntry obj)
		{
			var response = await TogglSrv
				.Post(ApiRoutes.TimeEntry.TimeEntryStartUrl, obj.ToJson())
				.ConfigureAwait(false);
			var timeEntry = response.GetData<TimeEntry>();

			return timeEntry;
		}

		/// <summary>
		/// Stop a TimeEntry
		/// </summary>
		/// <param name="obj">A TimeEntry object.</param>
		/// <returns>The stopped TimeEntry.</returns>
		public async Task<TimeEntry> Stop(TimeEntry obj)
		{
			var url = string.Format(ApiRoutes.TimeEntry.TimeEntryStopUrl, obj.Id);

			var response = await TogglSrv.Put(url, obj.ToJson()).ConfigureAwait(false);
			var timeEntry = response.GetData<TimeEntry>();

			return timeEntry;
		}

		/// <summary>
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/time_entries.md#update-a-time-entry
		/// </summary>
		/// <param name="obj"></param>
		public async Task<TimeEntry> Edit(TimeEntry obj)
		{
			var url = string.Format(ApiRoutes.TimeEntry.TimeEntryUrl, obj.Id);

			var response = await TogglSrv.Put(url, obj.ToJson()).ConfigureAwait(false);
			var timeEntry = response.GetData<TimeEntry>();

			return timeEntry;
		}

		/// <summary>
		/// Delete a time entry
		/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/time_entries.md#delete-a-time-entry
		/// </summary>
		/// <param name="id"></param>
		public async Task<bool> Delete(long id)
		{
			var rsp = await TogglSrv.Delete(string.Format(ApiRoutes.TimeEntry.TimeEntryUrl, id)).ConfigureAwait(false);

			return rsp.StatusCode == HttpStatusCode.OK;
		}

		public async Task<bool> DeleteIfAny(long[] ids)
		{
			if (ids.Length == 0 || ids == null)
				return true;
			return await Delete(ids).ConfigureAwait(false);
		}

		public async Task<bool> Delete(long[] ids)
		{
			if (ids.Length == 0 || ids == null)
				throw new ArgumentNullException(nameof(ids));

			var result = new Dictionary<long, bool>(ids.Length);
			foreach (var id in ids)
			{
				result.Add(id, await Delete(id).ConfigureAwait(false));
			}

			return !result.ContainsValue(false);
		}
	}
}