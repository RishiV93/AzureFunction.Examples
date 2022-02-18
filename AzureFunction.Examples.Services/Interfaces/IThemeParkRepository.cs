using AzureFunction.Examples.Models;
using Microsoft.Azure.Cosmos;

namespace AzureFunction.Examples.Services.Interfaces
{
    public interface IThemeParkRepository
    {
        Container Container { get; }

        Task<ThemePark> Delete(string id);
        Task<ThemePark> Get(string id);
        Task<ThemePark> Insert(ThemePark themePark);
        Task<IEnumerable<ThemePark>> List();
        IOrderedQueryable<ThemePark> Query();
        Task<IEnumerable<ThemePark>> ExecuteQuery(IOrderedQueryable<ThemePark> query);
        Task<ThemePark> Update(ThemePark themePark, string id);
    }
}