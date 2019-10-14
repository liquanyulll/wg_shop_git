using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace wg_frame_work
{
    public class ServiceEngin
    {
        private static IServiceProvider _applicationServices;

        private ServiceEngin()
        {
        }

        public static void Initialize(IServiceProvider applicationServices)
        {
            _applicationServices = applicationServices;
        }

        public static void Resolve<T>(Action<T> cb)
        {
            using (var serviceScope = _applicationServices.CreateScope())
            {
                cb((T)serviceScope.ServiceProvider.GetService(typeof(T)));
            }
        }

        public static HttpContext Current
        {
            get
            {
                object factory = _applicationServices.GetService(typeof(IHttpContextAccessor));
                HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }

        public static string IP
        {
            get
            {
                return Current.Connection.RemoteIpAddress.ToString();
            }
        }
    }
}
