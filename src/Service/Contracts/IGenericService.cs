using System;
using System.Collections.Generic;

namespace Service.Contracts
{
    /// <summary>
    /// Interface genérica que garante o corportamento de CRUD básico para as classes que a implementarem
    /// </summary>
    /// <typeparam name="T">Tipo, no modelo de domínio, onde serão aplicadas as operações de persistência</typeparam>
    public interface IGenericService<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Insere um objeto
        /// </summary>
        /// <param name="entity">Objeto a ser persistido</param>
        T Add(T entity);

        /// <summary>
        /// remove um objeto
        /// </summary>
        /// <param name="id">Id do objeto a ser removido</param>
        /// <returns>Retorna o objeto removido</returns>
        T Remove(dynamic id);

        /// <summary>
        /// Pesquisa um objeto pelo seu id
        /// </summary>
        /// <param name="id">Id do objeto a ser pesquisado</param>
        /// <returns>Retorna o objeto pesquisado</returns>
        T FindById(dynamic id);

        /// <summary>
        /// Atualiza um objeto
        /// </summary>
        /// <param name="entity">Objeto a ser atualizado</param>
        /// <param name="id">Id do objeto a ser atualizado</param>
        T Update(T entity, dynamic id);

        /// <summary>
        /// Lista todos os objetos de um determinado tipo
        /// </summary>
        /// <returns>Retorna uma lista de todos os objetos de um determinado tipo</returns>
        IList<T> ListAll();
    }
}
