using AzureFunction.Examples.Models;
using AzureFunction.Examples.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;

namespace AzureFunction.Examples.Services
{
    public class RideRepository : IRideRepository
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosClient _cosmosClient;
        const string RideEntityType = "Ride";

        public RideRepository(IConfiguration configuration, CosmosClient cosmosClient)
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
                    GetContainer(_configuration["Azure:Cosmos:Containers:Ride"]);
            }
        }

        public async Task<IEnumerable<Ride>> List()
        {
            var query = Query();
            return await ExecuteQuery(query);
        }

        public async Task<Ride> Get(string id)
        {
            var result = await Container.ReadItemAsync<Ride>(id, partitionKey: new PartitionKey(id));
            return result.Resource;
        }

        public async Task<Ride> Insert(Ride ride)
        {
            var result = await Container.CreateItemAsync<Ride>(ride);
            return result.Resource;
        }

        public async Task<Ride> Update(Ride ride, string id)
        {
            var result = await Container.UpsertItemAsync<Ride>(ride, new PartitionKey(id));
            return result.Resource;
        }

        public async Task<Ride> Delete(string id)
        {
            var result = await Container.DeleteItemAsync<Ride>(id, new PartitionKey(id));
            return result.Resource;
        }

        public async Task<IEnumerable<Ride>> ExecuteQuery(IOrderedQueryable<Ride> query)
        {
            var iterator = query.ToFeedIterator();
            var result = await iterator.ReadNextAsync();
            return result.AsEnumerable();
        }

        public IOrderedQueryable<Ride> Query()
        {
            var query = Container.GetItemLinqQueryable<Ride>(requestOptions: new QueryRequestOptions() { MaxItemCount = int.MaxValue });
            query = (IOrderedQueryable<Ride>)query.Where(c => c.EntityType == RideEntityType);
            return query;
        }
    }
}