using System.Threading.Tasks;

namespace EasyEventSourcing
{
    public interface IEventStoreProjections
    {
        Task<string> GetStateAsync(string projectionName);

        Task CreateAsync(string projectionName, string query);
    }
}
