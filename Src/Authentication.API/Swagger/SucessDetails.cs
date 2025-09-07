using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Swagger
{
    public class SucessDetails : ProblemDetails
    {
        public object? Data { get; set; }
    }
}
