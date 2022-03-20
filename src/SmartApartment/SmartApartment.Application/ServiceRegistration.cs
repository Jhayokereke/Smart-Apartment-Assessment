using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SmartApartment.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
