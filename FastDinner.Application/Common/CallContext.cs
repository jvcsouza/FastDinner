using System.Collections.Concurrent;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace FastDinner.Application.Common
{
    public abstract class BaseContext
    {
        private static HttpContext _context;

        public static void UseContext(HttpContext ctx)
        {
            if (ctx is null)
                throw new InvalidOperationException("Context informed is not usable!");

            ctx.Items ??= new ConcurrentDictionary<object, object>();

            _context = ctx;
        }

        public static void SetDataInternal(string name, object data)
        {
            if (_context != null) _context.Items[name] = data;
        }

        public static object GetDataInternal(string name) =>
            _context?.Items[name];
    }

    public class CallContext<T> : BaseContext
    {
        public static void SetData(string name, T data) => SetDataInternal(name, data);
        public static T GetData(string name) => (T)GetDataInternal(name);
    }

    //public static class CallContext<T>
    //{
    //    private static readonly ConcurrentDictionary<string, AsyncLocal<T>> State = new();

    //    public static void SetData(string name, T data) => 
    //        State.GetOrAdd(name, _ => new AsyncLocal<T>()).Value = data;

    //    public static T GetData(string name) =>
    //        State.TryGetValue(name, out var data) ? data.Value : default;
    //}

    //public static class CallContext<T>
    //{
    //    private static readonly ConcurrentDictionary<string, ThreadLocal<T>> State = new();

    //    public static void SetData(string name, T data) =>
    //        State.GetOrAdd(name, _ => new ThreadLocal<T>()).Value = data;

    //    public static T GetData(string name) =>
    //        State.TryGetValue(name, out var data) ? data.Value : default;
    //}
}
