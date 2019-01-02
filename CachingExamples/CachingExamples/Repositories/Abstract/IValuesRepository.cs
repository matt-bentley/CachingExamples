using System.Collections.Generic;

namespace CachingExamples.Repositories.Abstract
{
    public interface IValuesRepository
    {
        IEnumerable<string> Get();
    }
}
