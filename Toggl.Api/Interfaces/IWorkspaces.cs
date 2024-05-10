﻿using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toggl.Api.Models;

namespace Toggl.Api.Interfaces;

/// <summary>
/// https://engineering.toggl.com/docs/api/workspaces
/// </summary>
public interface IWorkspaces
{
	/// <summary>
	/// Get clients for a workspace
	/// https://engineering.toggl.com/docs/api/clients#get-list-clients
	/// </summary>
	/// <param name="workspaceId"></param>
	/// <param name="clientStatus"></param>
	/// <param name="nameContains"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Get("/api/v9/workspaces/{workspace_id}/clients")]
	Task<ICollection<Client>> GetClientsAsync(
		[AliasAs("workspace_id")] long workspaceId,
		[AliasAs("status")] ClientStatus? clientStatus,
		[AliasAs("name")] string? nameContains,
		CancellationToken cancellationToken
		);

	/// <summary>
	/// Get information of single workspace.
	/// https://engineering.toggl.com/docs/api/workspaces#get-get-single-workspace
	/// </summary>
	/// <param name="workspaceId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Get("/api/v9/workspaces/{workspace_id}")]
	Task<Workspace> GetAsync(
		[AliasAs("workspace_id")] long workspaceId,
		CancellationToken cancellationToken
		);

	/// <summary>
	/// Returns users that belong to the workspace and organization
	/// https://engineering.toggl.com/docs/api/projects#get-get-workspace-projects-users
	/// </summary>
	/// <param name="organizationId">The organization ID</param>
	/// <param name="workspaceId">The workspace ID</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns></returns>
	[Get("/api/v9/organizations/{organization_id}/workspaces/{workspace_id}/workspace_users")]
	Task<ICollection<WorkspaceUser>> GetUsersAsync(
		[AliasAs("organization_id")] long organizationId,
		[AliasAs("workspace_id")] long workspaceId,
		CancellationToken cancellationToken);
}

