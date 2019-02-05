using System;
using System.Collections.Generic;
using System.Data.Entity;
using AuditorHelper.Models;
using Autofac.Util;
using Repository.Contracts;
using Repository.Repositories;
using Repository.Util;
using Service.Contracts;

namespace Service.Services
{
    public abstract class GenericService<E, R> : Disposable, IGenericService<E> where E : Auditable
                                                                                where R : GenericRepository<E>
    {
        /// <summary>
        /// Representa o repositório genérico em que o serviço irá atuar
        /// </summary>
        private IRepository<E> genericDaoEfContext;

        /// <summary>
        /// Objeto que trata das operações transacionais
        /// </summary>
        protected TransactionHelper transactionHelper;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public GenericService() { transactionHelper = new TransactionHelper(); }

        /// <summary>
        /// Método abstrato em que as classes concretas devolvem a instância do seu próprio repositório
        /// </summary>
        /// <param name="context">Contexto do Entity Framework</param>
        /// <param name="transaction">Transação corrente do contexto</param>
        /// <returns></returns>
        //protected abstract IRepository<E> InstanceGenericRepository(DbContext context,
        //    DbContextTransaction transaction);

        /// <summary>
        /// Insere um objeto
        /// </summary>
        /// <param name="entity">Objeto a ser inserido</param>
        public virtual E Add(E entity)
        {
            E entityReturned = null;

            transactionHelper.ExecuteComplexTransactionalOperation(() => entityReturned = genericDaoEfContext.Add(entity),
                LoadDaoToSimpleOperation);

            return entityReturned;
        }

        /// <summary>
        /// Atualiza um objeto
        /// </summary>
        /// <param name="entity">Objeto a ser atualizado</param>
        /// <param name="id">Id do objeto a ser atualizado</param>
        public virtual E Update(E entity, dynamic id)
        {

            //SHOULD BE TESTED
            //var entityId = entity.GetType().GetProperty("Id").GetValue(entity);
            //if (Convert.ToInt32(entityId) != id)
            //    throw new ArgumentException("Informações incorretas!");
            E entityReturned = null;

            transactionHelper.ExecuteComplexTransactionalOperation(() => entityReturned = genericDaoEfContext.Update(entity),
                LoadDaoToSimpleOperation);

            return entityReturned;
        }

        /// <summary>
        /// Encontra um objeto pelo seu id
        /// </summary>
        /// <param name="id">Id do objeto a ser pesquisado</param>
        /// <returns>Retorna o objeto encontrado</returns>
        public virtual E FindById(dynamic id)
        {
            E entity = null;
            transactionHelper
                .ExecuteComplexTransactionalOperation(
                        () => entity = genericDaoEfContext.FindById(id),
                                                        LoadDaoToSimpleOperation);

            return entity;
        }

        /// <summary>
        /// Pesquisa todos os objetos de um determinado tipo
        /// </summary>
        /// <returns>Retorna todos os objetos de um determinado tipo</returns>
        public virtual IList<E> ListAll()
        {
            IList<E> resultList = new List<E>();
            transactionHelper.ExecuteComplexTransactionalOperation(() => resultList = genericDaoEfContext.ListAll(),
                LoadDaoToSimpleOperation);

            return resultList;
        }

        /// <summary>
        /// Remove um objeto, encontrado pelo seu id 
        /// </summary>
        /// <param name="id">Id do objeto a ser excluído</param>
        /// <returns>Retorna o objeto que foi excluído</returns>
        public virtual E Remove(dynamic id)
        {
            E removedEntity = null;
            transactionHelper.ExecuteComplexTransactionalOperation(() => removedEntity = Delete(id),
                LoadDaoToSimpleOperation);

            return removedEntity;
        }

        /// <summary>
        /// Pesquisa o objeto pelo seu id e remove. Este método funciona como uma espécie de Facade
        /// para encapsular essas duas operações, que serão executadas no método Remove
        /// </summary>
        /// <param name="id">Id do objeto a ser removido</param>
        /// <returns>Retorna o objeto que foi excluído</returns>
        private E Delete(dynamic id)
        {
            var entityToRemove = genericDaoEfContext.FindById(id);
            genericDaoEfContext.Remove(entityToRemove);

            return entityToRemove;
        }

        /// <summary>
        /// Devolve uma instância do repositório principal, utilizado pelo serviço
        /// </summary>
        /// <param name="context">Contexto do Entity Framework</param>
        /// <param name="transaction">Transação corrente do contexto</param>
        private void LoadDaoToSimpleOperation(DbContext context, DbContextTransaction transaction)
        {
            genericDaoEfContext = GetDefaultRepository(context, transaction);
        }

        protected R GetDefaultRepository(DbContext context,
                                         DbContextTransaction transaction)
        {
            return (R)Activator.CreateInstance(typeof(R), context, transaction);
        }

    }
}
