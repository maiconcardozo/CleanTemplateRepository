using System.Text;
using System.Text.Json;

namespace Authentication.Tests.Helpers;

internal static class TestHelpers
{
    public static StringContent CreateJsonContent(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    public static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
    }

    internal static class TestData
    {
        public static object ValidAccountPayload => new
        {
            userName = "testuser",
            password = "TestPassword123!",
            email = "test@example.com",
        };

        public static object InvalidAccountPayload => new
        {
            userName = string.Empty,
            password = "weak",
            email = "invalid-email",
        };

        public static object ValidLoginPayload => new
        {
            userName = "testuser",
            password = "TestPassword123!",
        };

        public static object InvalidLoginPayload => new
        {
            userName = string.Empty,
            password = string.Empty,
        };

        public static object ValidClaimPayload => new
        {
            type = "Permission",
            value = "user:read",
            description = "Read user permissions",
        };

        public static object InvalidClaimPayload => new
        {
            type = string.Empty,
            value = string.Empty,
            description = string.Empty,
        };

        public static object ValidActionPayload => new
        {
            name = "CreateUser",
            description = "Create a new user",
        };

        public static object InvalidActionPayload => new
        {
            name = string.Empty,
            description = string.Empty,
        };

        public static object ValidClaimActionPayload => new
        {
            claimId = 1,
            actionId = 1,
        };

        public static object InvalidClaimActionPayload => new
        {
            claimId = -1,
            actionId = -1,
        };

        public static object ValidAccountClaimActionPayload => new
        {
            idAccount = 1,
            idClaimAction = 1,
        };

        public static object InvalidAccountClaimActionPayload => new
        {
            idAccount = -1,
            idClaimAction = -1,
        };
    }

    internal static class StatusCodeGroups
    {
        public static readonly System.Net.HttpStatusCode[] SuccessOrClientError =
        {
            System.Net.HttpStatusCode.OK,
            System.Net.HttpStatusCode.BadRequest,
            System.Net.HttpStatusCode.Unauthorized,
            System.Net.HttpStatusCode.NotFound,
            System.Net.HttpStatusCode.InternalServerError,
        };

        public static readonly System.Net.HttpStatusCode[] ClientErrorOrServerError =
        {
            System.Net.HttpStatusCode.BadRequest,
            System.Net.HttpStatusCode.Unauthorized,
            System.Net.HttpStatusCode.NotFound,
            System.Net.HttpStatusCode.InternalServerError,
        };

        public static readonly System.Net.HttpStatusCode[] NotFoundOrError =
        {
            System.Net.HttpStatusCode.NotFound,
            System.Net.HttpStatusCode.BadRequest,
            System.Net.HttpStatusCode.InternalServerError,
        };

        public static readonly System.Net.HttpStatusCode[] MethodNotAllowedOrError =
        {
            System.Net.HttpStatusCode.MethodNotAllowed,
            System.Net.HttpStatusCode.NotFound,
            System.Net.HttpStatusCode.InternalServerError,
        };
    }

    internal static class Endpoints
    {
        public const string AuthenticationGenerateToken = "/Authentication/GenerateToken";

        public const string AccountAddAccount = "/Account/AddAccount";

        public const string ClaimGetClaims = "/Claim/GetClaims";
        public const string ClaimGetClaimById = "/Claim/GetClaimById/{0}";
        public const string ClaimAddClaim = "/Claim/AddClaim";
        public const string ClaimUpdateClaim = "/Claim/UpdateClaim/{0}";
        public const string ClaimDeleteClaim = "/Claim/DeleteClaim/{0}";

        public const string ActionGetActions = "/Action/GetActions";
        public const string ActionGetActionById = "/Action/GetActionById/{0}";
        public const string ActionAddAction = "/Action/AddAction";
        public const string ActionUpdateAction = "/Action/UpdateAction/{0}";
        public const string ActionDeleteAction = "/Action/DeleteAction/{0}";

        public const string ClaimActionGetClaimActions = "/ClaimAction/GetClaimActions";
        public const string ClaimActionGetClaimActionById = "/ClaimAction/GetClaimActionById/{0}";
        public const string ClaimActionAddClaimAction = "/ClaimAction/AddClaimAction";
        public const string ClaimActionUpdateClaimAction = "/ClaimAction/UpdateClaimAction/{0}";
        public const string ClaimActionDeleteClaimAction = "/ClaimAction/DeleteClaimAction/{0}";

        public const string AccountClaimActionGetAccountClaimActions = "/AccountClaimAction/GetAccountClaimActions";
        public const string AccountClaimActionGetAccountClaimActionById = "/AccountClaimAction/GetAccountClaimActionById/{0}/{1}";
        public const string AccountClaimActionAddAccountClaimAction = "/AccountClaimAction/AddAccountClaimAction";
        public const string AccountClaimActionUpdateAccountClaimAction = "/AccountClaimAction/UpdateAccountClaimAction/{0}";
        public const string AccountClaimActionDeleteAccountClaimAction = "/AccountClaimAction/DeleteAccountClaimAction/{0}/{1}";
    }

    internal static class HttpMethods
    {
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Put = "PUT";
        public const string Delete = "DELETE";
        public const string Patch = "PATCH";
        public const string Head = "HEAD";
        public const string Options = "OPTIONS";
    }

    public static string FormatEndpoint(string template, params object[] args)
    {
        return string.Format(template, args);
    }

    public static HttpRequestMessage CreateRequest(string method, string endpoint, object? content = null)
    {
        var request = new HttpRequestMessage(new HttpMethod(method), endpoint);

        if (content != null)
        {
            request.Content = CreateJsonContent(content);
        }

        return request;
    }

    public static async Task<bool> IsValidJsonResponse(HttpResponseMessage response)
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();
            JsonSerializer.Deserialize<object>(content);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
