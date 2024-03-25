﻿using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Toggl.Api.Models;

namespace Toggl.Api.Interfaces;

public interface ITasks
{
	/// <summary>
	/// Get project tasks for given workspace.
	/// https://engineering.toggl.com/docs/api/tasks#get-workspaceprojecttasks
	/// </summary>
	/// <param name="workspaceId">The workspace id</param>
	/// <param name="projectId">The project id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns></returns>
	[Get("/api/v9/workspaces/{workspace_id}/projects/{project_id}/tasks")]
	Task<ICollection<Models.Task>> GetAsync(
		[AliasAs("workspace_id")] long workspaceId,
		[AliasAs("project_id")] long projectId,
		CancellationToken cancellationToken
		);

	/// <summary>
	/// Get all tasks for given workspace.
	/// https://engineering.toggl.com/docs/api/tasks#get-workspaceprojecttasks
	/// </summary>
	/// <param name="workspaceId">The workspace id</param>
	/// <param name="since">Retrieve tasks created/modified/deleted since this date using UNIX timestamp.</param>
	/// <param name="page">Page number, default 1</param>
	/// <param name="perPage">Number of items per page, default 50</param>
	/// <param name="sortField">Field used for sorting</param>
	/// <param name="sortOrder">Sort order, default ASC</param>
	/// <param name="isActive">Filter by active state</param>
	/// <param name="projectId">Filter by project id</param>
	/// <param name="startDate">Smallest boundary date in the format YYYY-MM-DD</param>
	/// <param name="endDate">Biggest boundary date in the format YYYY-MM-DD</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns></returns>
	[Get("/api/v9/workspaces/{workspace_id}/tasks")]
	Task<Page<Models.Task>> GetAsync(
		[AliasAs("workspace_id")] long? workspaceId,
		[AliasAs("since")] long? since,
		[AliasAs("page")] int? page,
		[AliasAs("per_page")][Range(1, 200)] int? perPage,
		[AliasAs("sort_field")] string? sortField,
		[AliasAs("sort_order")] SortDirection? sortOrder,
		[AliasAs("active")] bool? isActive,
		[AliasAs("pid")] int? projectId,
		[AliasAs("start_date")] DateOnly? startDate,
		[AliasAs("end_date")] DateOnly? endDate,
		CancellationToken cancellationToken
		);
}