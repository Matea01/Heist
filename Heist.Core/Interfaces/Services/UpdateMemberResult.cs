namespace Heist.Core.Interfaces.Services
{
    public class UpdateMemberResult
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }

        public static UpdateMemberResult Success()
        {
            return new UpdateMemberResult { IsSuccess = true };
        }

        public static UpdateMemberResult Failure(string error)
        {
            return new UpdateMemberResult { IsSuccess = false, Error = error };
        }

    }
}
