using Microsoft.AspNetCore.Mvc;

namespace CleanTemplate.API.Swagger
{
    public class SucessDetails : ProblemDetails
    {
        public object? Data { get; set; }
    }
}
