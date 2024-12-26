using System.Net.Http.Json;

namespace BaseApp.Shared.ErrorHandling
{
    public static class ErrorHandler
    {
        public static void ResetErrorState(ref bool hasError, ref string? errorMessage)
        {
            hasError = false;
            errorMessage = null;
        }

        public static void HandleError(Exception ex, ref bool hasError, ref string? errorMessage)
        {
            hasError = true;
            errorMessage = ex.Message;
        }

        public static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
        {
            if (response == null)
            {
                return "No response received.";
            }

            if (response.Content != null)
            {
                try
                {
                    // Attempt to read ProblemDetails from the response content
                    var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                    if (problemDetails != null)
                    {
                        return problemDetails.Detail ?? "An unexpected error occurred.";
                    }
                }
                catch
                {
                    // Fallback for non-JSON or invalid ProblemDetails
                    return await response.Content.ReadAsStringAsync() ?? "An error occurred, but no details were provided.";
                }
            }

            return $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}";
        }
    }
}
