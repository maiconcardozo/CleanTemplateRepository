// ...existing code...
    public class SuccessDetailsExample : IExamplesProvider<SuccessResponseDTO>
    {
        public SuccessResponseDTO GetExamples()
        {
            return new SuccessResponseDTO
            {
                Status = 200,
                Detail = ResourceAPI.RequestWasSuccessful,
                Data = new object()
            };
        }
    }

    public static class ProblemDetailsExampleFactory
    {
        public static ProblemDetails ForBadRequest(string detail, string instance) => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = ResourceAPI.InvalidRequest,
            Detail = detail,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Instance = instance
        };

        public static ProblemDetails ForUnauthorized(string detail, string instance) => new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = ResourceAPI.Unauthorized,
            Detail = detail,
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Instance = instance
        };

        public static ProblemDetails ForNotFound(string detail, string instance) => new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = ResourceAPI.NotFound,
            Detail = detail,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Instance = instance
        };

        public static ProblemDetails ForInternalServerError(string detail, string instance) => new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ResourceAPI.InternalServerError,
            Detail = detail,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Instance = instance
        };
    }

    public class ProblemDetailsBadRequestExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples() => ProblemDetailsExampleFactory.ForBadRequest(
            ResourceAPI.OneOrMoreValidationErrorsOccurred,
            "/example/instance");
    }

    public class ProblemDetailsUnauthorizedExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples() => ProblemDetailsExampleFactory.ForUnauthorized(
            ResourceAPI.AuthenticationFailedInvalidCredentials,
            "/example/instance");
    }

    public class ProblemDetailsInternalServerErrorExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples() => ProblemDetailsExampleFactory.ForInternalServerError(
            ResourceAPI.AnUnexpectedErrorOccurred,
            "/example/instance");
    }

    public class ProblemDetailsNotFoundExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples() => ProblemDetailsExampleFactory.ForNotFound(
            ResourceAPI.TheRequestedResourceWasNotFound,
            "/example/instance");
    }