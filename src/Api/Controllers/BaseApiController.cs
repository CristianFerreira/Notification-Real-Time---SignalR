using AuditorHelper.Models;
using AuditorHelper.Services;
using Repository.Repositories;
using Service.Contracts;
using Service.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    public class BaseApiController<E, R, S> : ApiController where E : Auditable
                                                             where R : GenericRepository<E>
                                                             where S : GenericService<E, R>
    {
        private IGenericService<E> service;

        public BaseApiController()
        {
            service = GetService();
        }

        protected S GetService()
        {
            return Activator.CreateInstance<S>();
        }

        [HttpGet]
        public virtual async Task<IHttpActionResult> Get([FromUri] int id)
        {
            if (id == 0)
                return await Task.FromResult(BadRequest());

            E entity = service.FindById(id);
            if (entity == null)
                return await Task.FromResult(NotFound());

            return await Task.FromResult(Ok(entity));
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Post([FromBody] E entity)
        {
            AuditorService.Audit(entity, Request);
            var postedEntity = service.Add(entity);
            return await Task.FromResult(Ok<E>(postedEntity));
        }

        [HttpPut]
        public virtual async Task<IHttpActionResult> Put([FromBody] E entity, [FromUri] int id)
        {
            AuditorService.Audit(entity, Request);
            var updatedEntity = service.Update(entity, id);
            return await Task.FromResult(Ok<E>(updatedEntity));
        }


        [HttpDelete]
        public virtual async Task<IHttpActionResult> Delete([FromUri] int id)
        {
            service.Remove(id);
            return await Task.FromResult(Ok());
        }
    }
}