namespace ReceiveCookies.Core.Model
{
    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }

    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public ApiResult Success()
        {
            return Success("");
        }

        public ApiResult Success(string message)
        {
            return Success("", message);
        }
        public ApiResult Success(string code, string message)
        {
            return new ApiResult { IsSuccess = true, Code = code, Message = message };
        }

        public ApiResult Fail(string message)
        {
            return Fail("", message);
        }

        public ApiResult Fail(string code, string message)
        {
            return new ApiResult { IsSuccess = false, Code = code, Message = message };
        }
    }
}