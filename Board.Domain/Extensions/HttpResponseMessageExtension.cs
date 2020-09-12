using Board.Common.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Board.Common.Extensions
{
    public static class HttpResponseMessageExtension
    {
        public static async Task<Result<T>> GetResult<T>(this HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                var responseError = await message.Content.ReadAsStringAsync();
                return Result.Fail<T>(responseError);
            }

            var responseData = await message.Content.ReadAsJsonAsync<T>();
            return Result.Ok(responseData);
        }
    }
}
