using Ledgerly.API.Helpers;

namespace Ledgerly.Helpers
{
    public class ApiResponse<T> where T : class
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public ApiResponse()
        { }

        public ApiResponse(int code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>()
            {
                Data = data,
                Code = ApiResponseStatus.Success.Value(),
                Message = ApiResponseStatus.Success.Description()
            };
        }

        public static ApiResponse<T> Failure(int errorCode, string message)
        {
            return new ApiResponse<T>()
            {
                Data = null,
                Code = errorCode,
                Message = message
            };
        }

        public static ApiResponse<T> Failure(ApiResponseStatus error, string message)
        {
            return new ApiResponse<T>()
            {
                Data = null,
                Code = (int)error,
                Message = message
            };
        }

        public static ApiResponse<T> Failure(ApiResponseStatus error)
        {
            return new ApiResponse<T>()
            {
                Data = null,
                Code = (int)error,
                Message = error.Description()
            };
        }
    }
}