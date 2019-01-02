using System;
using System.Threading.Tasks;
using CachingExamples.Cache.Containers;
using CachingExamples.Repositories;
using CachingExamples.Repositories.Abstract;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CachingExamples.Startup))]

namespace CachingExamples
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            CacheContainer.Register<ValuesRepository>(Lifetime.Singleton);
        }
    }
}
