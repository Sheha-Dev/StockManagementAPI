namespace StockManagementAPI.Exceptions
{
    public class APIException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        public APIException(
            int statusCode,
            string errorCode,
            string message) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}