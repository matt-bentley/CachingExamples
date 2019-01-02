using CachingExamples.Cache;
using CachingExamples.Cache.Containers.Interception;
using CachingExamples.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading;

namespace CachingExamples.Repositories
{
    public class ValuesRepository : IValuesRepository
    {
        [Cache]
        public virtual IEnumerable<string> Get()
        {
            Thread.Sleep(1000);
            return new string[] { "value1", "value2" };
        }
    }
}