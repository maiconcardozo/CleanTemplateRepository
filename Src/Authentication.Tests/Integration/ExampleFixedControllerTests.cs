using System.Net;
using System.Text;
using System.Text.Json;
using Authentication.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Authentication.Tests.Integration;

/// <summary>
/// Example test demonstrating the MySQL connection fix
/// This test should not try to connect to MySQL database.
/// </summary>
public class ExampleFixedControllerTests : IClassFixture<AuthenticationWebApplicationFactory>
{
    private readonly AuthenticationWebApplicationFactory factory;
    private readonly HttpClient client;

    public ExampleFixedControllerTests(AuthenticationWebApplicationFactory factory)
    {
        this.factory = factory;
        factory.SeedTestData(); // This should work without MySQL connection
        client = this.factory.CreateClient();
    }
}
