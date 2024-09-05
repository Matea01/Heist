namespace Heist.Core.Interfaces.Services
{
    public class CreateMemberResult
    {
        public bool IsSuccess { get; private set; }
        public int MemberId { get; private set; }
        public string[] Errors { get; private set; }

        public static CreateMemberResult Success(int memberId) =>
            new CreateMemberResult { IsSuccess = true, MemberId = memberId };

        public static CreateMemberResult Failure(params string[] errors) =>
            new CreateMemberResult { IsSuccess = false, Errors = errors };
    }
}