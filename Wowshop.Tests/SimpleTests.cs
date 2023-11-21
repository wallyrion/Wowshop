using FluentAssertions;

namespace Wowshop.Tests;

public class SimpleTests
{
	[Fact]
	public void Test()
	{
		var result = "qqerty";

		result.Should().NotBeEmpty();
	}
}