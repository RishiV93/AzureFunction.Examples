using AzureFunction.Examples.Function.Extensions;
using AzureFunction.Examples.Models;
using AzureFunction.Examples.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunction.Examples.Function.Functions
{
    [Obsolete] // The filters class is marked as obsolete however it is due to change only, not removed.
    public class RideFunctions : IFunctionInvocationFilter
    {
        private readonly IRideRepository _rideRepository;
        private ILogger _log;

        public RideFunctions(IRideRepository rideRepository, ILogger<RideFunctions> log)
        {
            _rideRepository = rideRepository ?? throw new ArgumentNullException(nameof(rideRepository));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        [FunctionName("GetRidesList")]
        public async Task<IActionResult> List(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1.0/rides")] HttpRequest req)
        {
            var result = await _rideRepository.List();

            return new OkObjectResult(result);
        }

        [FunctionName("GetRideById")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1.0/rides/{id}")] HttpRequest req,
            string id)
        {
            var result = await _rideRepository.Get(id);

            return new OkObjectResult(result);
        }

        [FunctionName("PostRide")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1.0/rides")] HttpRequest req)
        {
            var ride = await req.ExtractValue<Ride>();
            var result = await _rideRepository.Insert(ride);

            return new OkObjectResult(result);
        }

        [FunctionName("PutRide")]
        public async Task<IActionResult> Put(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "v1.0/rides/{id}")] HttpRequest req,
            string id)
        {
            var ride = await req.ExtractValue<Ride>();
            var result = await _rideRepository.Update(ride, id);

            return new OkObjectResult(result);
        }

        [FunctionName("DeleteRide")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "v1.0/rides/{id}")] HttpRequest req,
            string id)
        {
            var result = await _rideRepository.Delete(id);

            return new OkObjectResult(result);
        }

        [Obsolete] // The filters class is marked as obsolete however it is due to change only, not removed.
        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            _log.LogInformation($"C# HTTP trigger {executingContext.FunctionName}");
            return Task.CompletedTask;
        }

        [Obsolete] // The filters class is marked as obsolete however it is due to change only, not removed.
        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}