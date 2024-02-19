using MediatR;
using Newtonsoft.Json;

namespace FastDinner.Application.Common.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task<T> SendLogging<T>(this ISender sender, IRequest<T> request)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (request == null) throw new ArgumentNullException(nameof(request));

            Console.WriteLine("Request: " + Environment.NewLine);
            Console.WriteLine(JsonConvert.SerializeObject(request, Formatting.Indented));

            var resp = await sender.Send(request);

            if (resp == null) return default;
            
            Console.WriteLine(Environment.NewLine + "Response: " + Environment.NewLine);
            Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));

            return resp;
        }
    }
}
