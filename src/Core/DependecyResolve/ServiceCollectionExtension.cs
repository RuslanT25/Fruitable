using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DependecyResolve
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreDependencies(this IServiceCollection services)
        {
            CoreModule module = new();
            module.Load(services);
        }
    }
}
