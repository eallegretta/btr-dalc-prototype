using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core;

namespace Web.Backend.DependencyInjection
{
    public static class ContainerExtensions
    {
        public static IEnumerable<T> ResolveAll<T>(this IContainer container)
        {
            // We're going to find each service which was registered
            // with a key, and for those which match the type T we'll store the key
            // and later supplement the default output with individual resolve calls to those
            // keyed services
            var allKeys = new List<object>();
            foreach (var componentRegistration in container.ComponentRegistry.Registrations)
            {
                // Get the services which match the KeyedService type
                var typedServices = componentRegistration.Services.Where(x => x is KeyedService).Cast<KeyedService>();
                // Add the key to our list so long as the registration is for the correct type T
                allKeys.AddRange(typedServices.Where(y => y.ServiceType == typeof(T)).Select(x => x.ServiceKey));
            }

            // Get the default resolution output which resolves all un-keyed services
            var allUnKeyedServices = new List<T>(container.Resolve<IEnumerable<T>>());
            // Add the ones which were registered with a key
            allUnKeyedServices.AddRange(allKeys.Select(key => container.ResolveKeyed<T>(key)));

            // Return the total resultset
            return allUnKeyedServices;
        }
    }
}