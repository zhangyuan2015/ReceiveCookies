using ReceiveCookies.Core.Model;
using System.Net;
using System.Text.Json;

namespace ReceiveCookies.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly IWebHostEnvironment _env;
        private readonly RequestDelegate _next;  // 用来处理上下文请求  
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(IWebHostEnvironment env, RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); //要么在中间件中处理，要么被传递到下一个中间件中去
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex); // 捕获异常了 在HandleExceptionAsync中处理
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var apiRes = new ApiResult
            {
                IsSuccess = true,
                Code = HttpStatusCode.InternalServerError.ToString()
            };
            if (_env.IsDevelopment())
                apiRes.Message = $"{exception.Message}{exception.StackTrace}";
            else
                apiRes.Message = exception.Message;

            _logger.LogError(exception, exception.Message);
            var result = JsonSerializer.Serialize(apiRes);
            await context.Response.WriteAsync(result);
        }
    }
}