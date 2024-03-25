﻿using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Toggl.Api.Test;

public class GroupTests(ITestOutputHelper testOutputHelper) : TogglTest(testOutputHelper)
{
	[Fact]
	public async void Tasks_Get_Groups_Succeeds()
	{
		var tasks = await TogglClient
			.Groups
			.GetAsync(
				await GetOrganizationIdAsync(),
				null,
				null,
				default);

		tasks.Should().NotBeNull();
	}
}
