using System.Threading.Tasks;

using RestSharp;
using RestSharp.Serialization.Json;

namespace UdemyPortfolio.Services.Extensions
{
    public static class RestSharpExtensions
    {
        public static async Task<T> GetAsync<T>(this RestClient client, RestRequest request)
        {
            TaskCompletionSource<IRestResponse> tcs = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, (res, handler) =>
            {
                tcs.SetResult(res);
            });

            await tcs.Task;

            JsonSerializer jsonSerializer = new JsonSerializer();

            return jsonSerializer.Deserialize<T>(tcs.Task.Result);
        }
    }
}
