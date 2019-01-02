using CachingExamples.Cache.Containers;
using CachingExamples.Repositories;
using CachingExamples.Repositories.Abstract;
using System.Collections.Generic;
using System.Web.Http;

namespace CachingExamples.Controllers
{
    public class ValuesController : ApiController
    {
        IValuesRepository _valuesRepository;

        public ValuesController()
        {
            _valuesRepository = CacheContainer.Get<ValuesRepository>();
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return _valuesRepository.Get();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
