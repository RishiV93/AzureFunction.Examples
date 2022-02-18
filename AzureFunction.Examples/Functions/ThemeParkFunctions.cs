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
    public class ThemeParkFunctions : IFunctionInvocationFilter
    {
        private readonly IThemeParkRepository _themeParkRepository;
        private ILogger _log;

        public ThemeParkFunctions(IThemeParkRepository themeParkRepository, ILogger<ThemeParkFunctions> log)
        {
            _themeParkRepository = themeParkRepository ?? throw new ArgumentNullException(nameof(themeParkRepository));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        [FunctionName("GetThemeParksList")]
        public async Task<IActionResult> List(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1.0/themeparks")] HttpRequest req)
        {
            var result = await _themeParkRepository.List();

            return new OkObjectResult(result);
        }

        [FunctionName("GetThemeParkById")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1.0/themeparks/{id}")] HttpRequest req,
            string id)
        {
            var result = await _themeParkRepository.Get(id);

            return new OkObjectResult(result);
        }

        [FunctionName("PostThemePark")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1.0/themeparks")] HttpRequest req)
        {
            var themePark = await req.ExtractValue<ThemePark>();
            var result = await _themeParkRepository.Insert(themePark);

            return new OkObjectResult(result);
        }

        [FunctionName("PutThemePark")]
        public async Task<IActionResult> Put(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "v1.0/themeparks/{id}")] HttpRequest req,
            string id)
        {
            var themePark = await req.ExtractValue<ThemePark>();
            var result = await _themeParkRepository.Update(themePark, id);

            return new OkObjectResult(result);
        }

        [FunctionName("DeleteThemePark")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "v1.0/themeparks/{id}")] HttpRequest req,
            string id)
        {
            var result = await _themeParkRepository.Delete(id);

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
