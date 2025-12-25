using BookManagement.API.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace BookManagement.API.Middlewares
{
   
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) when (HandleException(context, ex))
            {
                // Exception handled
            }
        }

        private bool HandleException(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            var (statusCode, title, detail) = exception switch
            {
                
                ArgumentException or ArgumentNullException =>
                    ((int)HttpStatusCode.BadRequest, "Bad Request", exception.Message),

                DbUpdateConcurrencyException =>
                    ((int)HttpStatusCode.Conflict, "Concurrency Conflict",
                     "Record was modified by another user. Please refresh and retry."),

               
                NotFoundException => ((int)HttpStatusCode.NotFound, "Not Found", exception.Message),
                ConflictException => ((int)HttpStatusCode.Conflict, "Conflict", exception.Message),

                _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error", "An unexpected error occurred.")
            };

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }
    }

}
