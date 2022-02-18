using AzureFunction.Examples.Models;
using Microsoft.Azure.Cosmos;

namespace AzureFunction.Examples.Services.Interfaces
{
    public interface IRideRepository
    {
        Container Container { get; }

        Task<Ride> Delete(string id);
        Task<Ride> Get(string id);
        Task<Ride> Insert(Ride ride);
        Task<IEnumerable<Ride>> List();
        IOrderedQueryable<Ride> Query();
        Task<IEnumerable<Ride>> ExecuteQuery(IOrderedQueryable<Ride> query);
        Task<Ride> Update(Ride ride, string id);
    }
}