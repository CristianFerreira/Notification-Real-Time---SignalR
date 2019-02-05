using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using AuditorHelper.Models;
using AuditorHelper.Services;
using Dapper;
using Repository.Configuration;
using Repository.Contracts;

namespace Repository.Repositories
{
    /// <summary>
    /// Classe que representa um repositório genérico, oferecendo as operações básicas de CRUD
    /// à todas as classes que a implementarem
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade que sofrerá as operações de persistência</typeparam>
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : Auditable
    {
        /// <summary>
        /// Propriedade que representa um contexto do Entity Framework
        /// </summary>
        protected readonly DbContext context;

        /// <summary>
        /// Propriedade que representa a transação corrente
        /// </summary>
        protected readonly DbContextTransaction transaction;

        /// <summary>
        /// Propriedade que representa a connection sql, para operações de mais baixo nível
        /// de acesso a dados
        /// </summary>
        protected readonly SqlConnection connection;

        // TODO remover
        protected EntityContext GetEntityContext()
        {
            return (EntityContext)context;
        }

        /// <summary>
        /// Contrutor da classe, explicitando a passagem obrigatória de parâmetros, para o correto funcionamento
        /// </summary>
        /// <param name="context">Contexto do Entity Framework</param>
        /// <param name="transaction">Transação Corrente</param>
        public GenericRepository(DbContext context, DbContextTransaction transaction)
        {
            this.context = context;
            this.transaction = transaction;
            connection = GetConnection(context);
        }

        #region Dapper

        /// <summary>
        /// Recupera a transação atual. Este método deve ser utilizado em operações com o Dapper framework
        /// </summary>
        /// <param name="transaction">Corresponde a transação atual, no contexto de Entity Framework</param>
        /// <returns>Retorna a transação corrente, preparada para ser utilizada em operações de mais baixo nível 
        /// na base de persistência</returns>
        private IDbTransaction GetCurrentTransaction(DbContextTransaction transaction) { return (SqlTransaction)transaction.UnderlyingTransaction; }

        /// <summary>
        /// Recupera a conexão sql da aplicação. Este método deve ser utilizado em operações com o Dapper framework
        /// </summary>
        /// <param name="context">Contexto do Entity Framework</param>
        /// <returns>Retorna a conexão sql da aplicação, preparada para ser utilizada em operações de mais baixo nível 
        /// na base de persistência</returns>
        private static SqlConnection GetConnection(DbContext context) { return (SqlConnection)context.Database.Connection; }

        /// <summary>
        /// Executa operações sql da baixo nível. Este método deve ser utilizado em operações com o Dapper framework
        /// </summary>
        /// <param name="query">Instrução sql a ser realizada</param>
        /// <param name="parameters">Parâmetros a serem utilizados na instrução</param>
        protected void ExecuteOperation(string query, object parameters)
        {
            if (transaction != null)
                connection.Execute(query, parameters, GetCurrentTransaction(transaction));
            else
                connection.Execute(query, parameters);
        }

        /// <summary>
        /// Executa operações sql da baixo nível. Este método deve ser utilizado em operações com o Dapper framework
        /// </summary>
        /// <param name="query">Instrução sql a ser realizada</param>
        protected void ExecuteOperation(string query)
        {
            if (transaction != null)
                connection.Execute(query, GetCurrentTransaction(transaction));
            else
                connection.Execute(query);
        }

        protected IEnumerable<T> ExecuteQuery<T>(string query, object parameters)
        {
            if (transaction != null)
                return connection.Query<T>(query, parameters, GetCurrentTransaction(transaction));
            else
                return connection.Query<T>(query, parameters);
        }

        protected IEnumerable<T> ExecuteQuery<T>(string query)
        {
            if (transaction != null)
                return connection.Query<T>(query, GetCurrentTransaction(transaction));
            else
                return connection.Query<T>(query);
        }

        protected T ExecuteScalar<T>(string query)
        {
            if (transaction != null)
                return connection.ExecuteScalar<T>(query, GetCurrentTransaction(transaction));
            else
                return connection.ExecuteScalar<T>(query);
        }

        protected T ExecuteScalar<T>(string query, object parameters)
        {
            if (transaction != null)
                return connection.ExecuteScalar<T>(query, parameters, GetCurrentTransaction(transaction));
            else
                return connection.ExecuteScalar<T>(query, parameters);
        }

        #endregion

        #region Entity Framework

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity entity)
        {
            try
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
            catch (Exception ex) { CustomHandlingDatabaseException(ex, entity); }
            return entity;
        }

        protected abstract void SpecificHandlingDatabaseException(Exception ex, TEntity entity);

        private void CustomHandlingDatabaseException(Exception ex, TEntity entity)
        {
            SpecificHandlingDatabaseException(ex, entity);
            throw ex;
        }

        /// <summary>
        /// Método genérico para atualizações
        /// </summary>
        /// <param name="entity">Entidade a ser atualizada</param>
        public virtual TEntity Update(TEntity entity)
        {
            try
            {
                context.Entry(entity).State = EntityState.Modified;
                AuditorService.ProtectAuditableValues(entity, context);
                context.SaveChanges();
            }
            catch (Exception ex) { CustomHandlingDatabaseException(ex, entity); }
            return entity;
        }

        /// <summary>
        /// Método genérico para efetuar buscas por id
        /// </summary>
        /// <param name="id">Id do objeto a ser recuperado</param>
        /// <returns>Retorna o objeto encontrado</returns>
        public virtual TEntity FindById(int id) { return context.Set<TEntity>().Find(id); }

        /// <summary>
        /// Método que retorna todos os objetos do tipo especificado
        /// </summary>
        /// <returns>Retorna todos os objetos do tipo especificado</returns>
        public virtual IList<TEntity> ListAll()
        {
            var busca = from e in context.Set<TEntity>() select e;

            return busca.ToList();
        }


        public virtual TEntity Merge(int id, TEntity entity)
        {
            if (id > 0)
                return Update(entity);
            else
                return Add(entity);
        }

        /// <summary>
        /// Remove o objeto especificado
        /// </summary>
        /// <param name="entity">Objeto a ser removido</param>
        public virtual void Remove(TEntity entity)
        {
            try
            {
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
            catch (Exception ex) { CustomHandlingDatabaseException(ex, entity); }
        }

        /// <summary>
        /// Remove o objeto especificado
        /// </summary>
        /// <param name="entity">Objeto a ser removido</param>
        public virtual void RemoveWithoutContext(TEntity entity)
        {
            try
            {

                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (Exception ex) { CustomHandlingDatabaseException(ex, entity); }
        }

        #endregion
    }
}
