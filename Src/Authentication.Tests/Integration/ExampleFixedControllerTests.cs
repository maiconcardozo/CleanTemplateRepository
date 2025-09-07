using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Authentication.Tests.Fixtures;
using Xunit;

namespace Authentication.Tests.Integration;

/// <summary>
/// Example test demonstrating the MySQL connection fix
/// This test should not try to connect to MySQL database
/// </summary>
public class ExampleFixedControllerTests : IClassFixture<AuthenticationWebApplicationFactory>
{
    private readonly AuthenticationWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ExampleFixedControllerTests(AuthenticationWebApplicationFactory factory)
    {
        _factory = factory;
        factory.SeedTestData(); // This should work without MySQL connection
        _client = _factory.CreateClient();
    }

   
}