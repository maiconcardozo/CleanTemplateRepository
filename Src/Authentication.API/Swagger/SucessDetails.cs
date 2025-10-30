using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Swagger
{
    internal class SucessDetails : ProblemDetails
    {
        public object? Data { get; set; }
    }
}
