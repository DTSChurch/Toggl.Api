﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Toggl.Api.DataObjects;
using Toggl.Api.Extensions;
using Toggl.Api.Interfaces;
using Toggl.Api.QueryObjects;
using Toggl.Api.Routes;
using Task = Toggl.Api.DataObjects.Task;

namespace Toggl.Api.Services
{
	public class TaskServiceAsync : ITaskServiceAsync
	{
		private readonly string _togglTasksUrl = ApiRoutes.Task.TogglTasksUrl;

		private IApiServiceAsync TogglSrv { get; set; }

		public TaskServiceAsync(string apiKey)
			: this(new ApiServiceAsync(apiKey))
		{
		}

		public TaskServiceAsync(IApiServiceAsync srv)
		{
			TogglSrv = srv;
		}

		public async Task<Task> Get(int id)
		{
			var url = string.Format(ApiRoutes.Task.TogglTasksGet, id);
			var response = await TogglSrv.Get(url).ConfigureAwait(false);
			var data = response.GetData<Task>();
			return data;
		}

		/// <summary>
		///Add a task
		/// https://www.toggl.com/public/api#post_tasks
		/// </summary>
		/// <param name="t"></param>
		public async Task<Task> Add(Task t)
		{
			var response = await TogglSrv.Post(_togglTasksUrl, t.ToJson()).ConfigureAwait(false);
			var data = response.GetData<Task>();
			return data;
		}

		/// <summary>
		/// Edit a task
		/// https://www.toggl.com/public/api#put_tasks
		/// </summary>
		/// <param name="t"></param>
		public async Task<Task> Edit(Task t)
		{
			var url = string.Format(ApiRoutes.Task.TogglTasksGet, t.Id);
			var response = await TogglSrv.Put(url, t.ToJson()).ConfigureAwait(false);
			var data = response.GetData<Task>();
			return data;
		}

		/// <summary>
		///
		/// https://www.toggl.com/public/api#del_tasks
		/// </summary>
		/// <param name="id"></param>
		public async Task<bool> Delete(int id)
		{
			var url = string.Format(ApiRoutes.Task.TogglTasksGet, id);

			var rsp = await TogglSrv.Delete(url).ConfigureAwait(false);

			return rsp.StatusCode == HttpStatusCode.OK;
		}

		public async Task<bool> DeleteIfAny(int[] ids)
		{
			if (ids.Length == 0 || ids == null)
				return true;
			return await Delete(ids).ConfigureAwait(false);
		}

		public async Task<bool> Delete(int[] ids)
		{
			if (ids.Length == 0 || ids == null)
				throw new ArgumentNullException(nameof(ids));

			var url = string.Format(
				ApiRoutes.Task.TogglTasksGet,
				string.Join(",", ids.Select(id => id.ToString()).ToArray()));

			var rsp = await TogglSrv.Delete(url).ConfigureAwait(false);

			return rsp.StatusCode == HttpStatusCode.OK;
		}

		public async Task<Task> ForProjectByName(Project project, string taskName)
		{
			if (!project.Id.HasValue)
				throw new InvalidOperationException("Project Id not set");

			return await ForProjectByName(project.Id.Value, taskName).ConfigureAwait(false);
		}

		public async Task<Task> ForProjectByName(int projectId, string taskName)
		{
			var projectTasks = await ForProject(projectId).ConfigureAwait(false);
			return projectTasks.Single(task => task.Name == taskName);
		}

		public async Task<Task> TryGetForProjectByName(int projectId, string taskName)
		{
			var projectTasks = await ForProject(projectId).ConfigureAwait(false);
			return projectTasks.SingleOrDefault(task => task.Name == taskName);
		}

		public async Task<List<Task>> ForProject(int id)
		{
			var url = string.Format(ApiRoutes.Project.ProjectTasksUrl, id);
			var response = await TogglSrv.Get(url).ConfigureAwait(false);
			var data = response.GetData<List<Task>>();
			return data;
		}

		public async Task<List<Task>> ForProject(Project project)
		{
			if (!project.Id.HasValue)
				throw new InvalidOperationException("Project Id not set");

			return await ForProject(project.Id.Value).ConfigureAwait(false);
		}

		public async void Merge(Task masterTask, Task slaveTask, int workspaceId, string userAgent = TogglClient.UserAgent)
		{
			if (!masterTask.Id.HasValue)
				throw new InvalidOperationException("Master task Id not set");

			if (!slaveTask.Id.HasValue)
				throw new InvalidOperationException("Slave task Id not set");

			await Merge(masterTask.Id.Value, slaveTask.Id.Value, workspaceId, userAgent).ConfigureAwait(false);
		}

		public async System.Threading.Tasks.Task Merge(int masterTaskId, int slaveTaskId, int workspaceId, string userAgent = TogglClient.UserAgent)
		{
			var reportService = new ReportServiceAsync(TogglSrv);
			var timeEntryService = new TimeEntryServiceAsync(TogglSrv);

			var reportParams = new DetailedReportParams
			{
				UserAgent = userAgent,
				WorkspaceId = workspaceId,
				TaskIds = slaveTaskId.ToString(),
				Since = DateTime.Now.AddYears(-1).ToIsoDateStr()
			};

			var result = await reportService.Detailed(reportParams).ConfigureAwait(false);

			if (result.TotalCount > result.PerPage)
				result = await reportService.FullDetailedReport(reportParams).ConfigureAwait(false);

			foreach (var reportTimeEntry in result.Data)
			{
				if (reportTimeEntry.Id == null) continue;
				var timeEntry = await timeEntryService.Get(reportTimeEntry.Id.Value).ConfigureAwait(false);
				timeEntry.TaskId = masterTaskId;
				try
				{
					var _ = await timeEntryService.Edit(timeEntry).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					var _ = ex.Data;
				}
			}

			if (!await Delete(slaveTaskId).ConfigureAwait(false))
			{
				throw new InvalidOperationException(string.Format("Can't delete task #{0}", slaveTaskId));
			}
		}

		public async void Merge(int masterTaskId, int[] slaveTasksIds, int workspaceId, string userAgent = TogglClient.UserAgent)
		{
			var reportService = new ReportServiceAsync(TogglSrv);
			var timeEntryService = new TimeEntryServiceAsync(TogglSrv);

			var reportParams = new DetailedReportParams
			{
				UserAgent = userAgent,
				WorkspaceId = workspaceId,
				TaskIds = string.Join(",", slaveTasksIds.Select(id => id.ToString())),
				Since = DateTime.Now.AddYears(-1).ToIsoDateStr()
			};

			var result = await reportService.Detailed(reportParams).ConfigureAwait(false);

			if (result.TotalCount > result.PerPage)
				result = await reportService.FullDetailedReport(reportParams).ConfigureAwait(false);

			foreach (var reportTimeEntry in result.Data)
			{
				if (reportTimeEntry.Id == null)
				{
					continue;
				}
				var timeEntry = await timeEntryService.Get(reportTimeEntry.Id.Value).ConfigureAwait(false);
				timeEntry.TaskId = masterTaskId;
				var editedTimeEntry = await timeEntryService.Edit(timeEntry).ConfigureAwait(false);
				if (editedTimeEntry == null)
					throw new ArgumentNullException(string.Format("Can't edit timeEntry #{0}", reportTimeEntry.Id));
			}

			foreach (var slaveTaskId in slaveTasksIds)
			{
				if (!await Delete(slaveTaskId).ConfigureAwait(false))
					throw new InvalidOperationException(string.Format("Can't delete task #{0}", slaveTaskId));
			}
		}
	}
}