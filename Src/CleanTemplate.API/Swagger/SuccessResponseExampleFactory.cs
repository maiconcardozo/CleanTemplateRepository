using CleanTemplate.API.Resource;
using Swashbuckle.AspNetCore.Filters;

namespace CleanTemplate.API.Swagger
{
    public static class SuccessResponseExampleFactory
    {
        public static SucessDetails ForSuccess(object? data, string detail = null, string instance = "/example/instance")
        {
            return new SucessDetails
            {
                Status = StatusCodes.Status200OK,
                Title = ResourceAPI.OK,
                Detail = detail,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
                Data = data,
                Instance = instance
            };
        }
    }

    public class SucessDetailsExample : IExamplesProvider<SucessDetails>
    {
        public SucessDetails GetExamples() =>
            SuccessResponseExampleFactory.ForSuccess(
                new { UserId = 123, UserName = "example.example", Email = "example.example@example.com" },
                ResourceAPI.RequestWasSuccessful,
                "/example/instance");
    }
}
