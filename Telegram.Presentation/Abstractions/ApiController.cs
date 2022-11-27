using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Shared;

namespace Telegram.Presentation.Abstractions
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected ISender Sender { get; }
        public ApiController(ISender sender)
        {
            Sender = sender;
        }

        protected IActionResult HandleFailure(Result result) =>
            result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException(),
                IValidationResult vResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error", StatusCodes.Status400BadRequest,
                        result.Error!,
                        vResult.Errors)),
                _ => BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error!))
            };

        private static ProblemDetails CreateProblemDetails(
            string title,
            int code,
            Error error,
            Error[]? errors = null) =>
            new()
            {
                Title = title,
                Type = error.Code,
                Detail = error.Message,
                Status = code,
                Extensions = { { nameof(errors), errors } }
            };
    }
}
