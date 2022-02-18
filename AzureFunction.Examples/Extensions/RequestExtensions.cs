using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunction.Examples.Function.Extensions
{
    public static class RequestExtensions
    {
        public static async Task<T> ExtractValue<T>(this HttpRequest req)
        {
            string requestBody = String.Empty;
            using (StreamReader streamReader = new(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            T returnValue = JsonConvert.DeserializeObject<T>(requestBody);
            return returnValue;
        }
    }
}