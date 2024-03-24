﻿using System.Text.Json.Serialization;
using Toggl.Api.Models;

namespace Toggl.Api.QueryObjects;

public class ProjectDashboardParams : Item
{
	/// <summary>
	/// The developer's details
	/// </summary>
	[JsonPropertyName("user_agent")]
	public string? UserAgent { get; set; }

	/// <summary>
	/// The workspace whose data you want to access
	/// </summary>
	[JsonPropertyName("workspace_id")]
	public long WorkspaceId { get; set; }

	/// <summary>
	/// The project whose data you want to access
	/// </summary>
	[JsonPropertyName("project_id")]
	public long ProjectId { get; set; }

	/// <summary>
	/// name/assignee/duration/billable_amount/estimated_seconds
	/// </summary>
	[JsonPropertyName("order_field")]
	public string OrderField { get; set; } = "name";

	/// <summary>
	/// on/off, on for descending and off for ascending order
	/// </summary>
	[JsonPropertyName("order_desc")]
	public string OrderDesc { get; set; } = "on";
}
