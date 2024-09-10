namespace Heist.Core.Interfaces.Services
{
    public class UpdateHeistResult
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }


        public static UpdateHeistResult Success()
        {
            return new UpdateHeistResult { IsSuccess = true, StatusCode = 204 };
        }

        public static UpdateHeistResult Failure(string error, int statusCode)
        {
            return new UpdateHeistResult { IsSuccess = false, Error = error, StatusCode = statusCode };
        }

    }
}

