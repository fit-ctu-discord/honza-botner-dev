using System.Collections.Generic;
using System.Threading.Tasks;
using HonzaBotner.Database;

namespace HonzaBotner.Services.Contract
{
    public interface IWarningService
    {
        Task<List<Warning>> GetAllWarningsAsync();

        Task<List<Warning>> GetWarningsAsync(ulong userId);

        Task<Warning?> GetWarningAsync(int id);

        Task<bool> DeleteWarningAsync(int id);

        Task<(bool, int)> AddWarningAsync(ulong userId, string reason);

        Task<int> GetNumberOfWarnings(ulong userId);
    }
}
