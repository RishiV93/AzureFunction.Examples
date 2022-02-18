using AzureFunction.Examples.Models;
using AzureFunction.Examples.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;

namespace AzureFunction.Examples.Services
{
    public class ThemeParkRepository : IThemeParkRepository
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosClient _cosmosClient;
        const string ThemeParkEntityType = "ThemePark";

        public ThemeParkRepository(IConfiguration configuration, CosmosClient cosmosClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
        }

        public Container Container
        {
            get
            {
                return _cosmosClient.
                    GetDatabase(_configuration["Azure:Cosmos:Database"]).
                    GetContainer(_configuration["Azure:Cosmos:Containers:ThemePark"]);
            }
        }

        public async Task<IEnumerable<ThemePark>> List()
        {
            var query = Query();
            return await ExecuteQuery(query);
        }

        public async Task<ThemePark> Get(string id)
        {
            var result = await Container.ReadItemAsync<ThemePark>(id, partitionKey: new PartitionKey(id));
            return result.Resource;
        }

        public async Task<ThemePark> Insert(ThemePark themePark)
        {
            var result = await Container.CreateItemAsync<ThemePark>(themePark);
            return result.Resource;
        }

        public async Task<ThemePark> Update(ThemePark themePark, string id)
        {
            var result = await Container.UpsertItemAsync<ThemePark>(themePark, new PartitionKey(id));
            return result.Resource;
        }

        public async Task<ThemePark> Delete(string id)
        {
            var result = await Container.DeleteItemAsync<ThemePark>(id, new PartitionKey(id));
            return result.Resource;
        }

        public async Task<IEnumerable<ThemePark>> ExecuteQuery(IOrderedQueryable<ThemePark> query)
        {
            var iterator = query.ToFeedIterator();
            var result = await iterator.ReadNextAsync();
            return result.AsEnumerable();
        }

        public IOrderedQueryable<ThemePark> Query()
        {
            var query = Container.GetItemLinqQueryable<ThemePark>(requestOptions: new QueryRequestOptions() { MaxItemCount = int.MaxValue });
            query = (IOrderedQueryable<ThemePark>)query.Where(c => c.EntityType == ThemeParkEntityType);
            return query;
        }        
    }
}