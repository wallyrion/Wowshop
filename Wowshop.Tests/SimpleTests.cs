using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Wowshop.Tests;

public class SimpleTests
{
	[Fact]
	public async Task SimpleIntegrationTest()
	{
		WebApplicationFactory<Program> factory = new();

		var httpClient = factory.CreateClient();

		var response = await httpClient.GetAsync("/");
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var responseContent = await response.Content.ReadAsStringAsync();
		responseContent.Should().Be("Hello World!");
	}
}