using Heist.Core.DTO;

namespace Heist.Core.Interfaces.Services
{
    public interface IHeistService
    {
        Task<CreateHeistResult> CreateHeistAsync(AddHeistDto heistDto);
    }
}