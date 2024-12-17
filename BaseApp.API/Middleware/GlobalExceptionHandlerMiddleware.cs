using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BaseApp.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed with the next middleware
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Database Error",
                    Detail = "An error occurred while updating the database.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "SQL Error",
                    Detail = "An error occurred while interacting with the SQL database.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (ArgumentNullException argNullEx)
            {
                _logger.LogError(argNullEx, "Argument null error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Null Argument Error",
                    Detail = "A required argument was null.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError(argEx, "Argument error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Argument",
                    Detail = argEx.Message,
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (FileNotFoundException fileEx)
            {
                _logger.LogError(fileEx, "File not found error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "File Not Found",
                    Detail = "The requested file could not be found.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (UnauthorizedAccessException unAuthEx)
            {
                _logger.LogError(unAuthEx, "Unauthorized access error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized Access",
                    Detail = "You do not have permission to perform this action.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status502BadGateway,
                    Title = "HTTP Request Error",
                    Detail = "There was an error while making an HTTP request.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status502BadGateway;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (TimeoutException timeoutEx)
            {
                _logger.LogError(timeoutEx, "Request timed out.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status408RequestTimeout,
                    Title = "Request Timeout",
                    Detail = "The request timed out while processing.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (NullReferenceException nullRefEx)
            {
                _logger.LogError(nullRefEx, "Null reference error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Null Reference Error",
                    Detail = "An unexpected null reference error occurred.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (OutOfMemoryException oomEx)
            {
                _logger.LogError(oomEx, "Out of memory error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Out of Memory Error",
                    Detail = "The server ran out of memory while processing the request.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (BadHttpRequestException badRequestEx)
            {
                _logger.LogError(badRequestEx, "Bad request error occurred due to invalid input.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = "The request body is malformed or contains invalid data. Ensure the JSON is correctly formatted, especially with DateOnly values.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (FormatException formatEx)
            {
                _logger.LogError(formatEx, "Format error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Format",
                    Detail = "The format of one or more fields in the request body is invalid. For example, DateOnly might not be in a supported format.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (KeyNotFoundException keyNotFoundEx)
            {
                _logger.LogError(keyNotFoundEx, "Resource not found.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = "The requested resource could not be found.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (DuplicateNameException dupKeyEx)
            {
                _logger.LogError(dupKeyEx, "Resource Id/Name already exist.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = "Failed to create the requested resource because it already exist.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Unexpected Error",
                    Detail = "An unexpected error occurred while processing your request.",
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
