using System.Collections.Generic;

namespace Repository.Contracts
{
    /// <summary>
    /// Interface que mapeia os métodos básicos de CRUD
    /// </summary>
    /// <typeparam name="TEntity">Tipo do objeto que sofrerá as operações de persistência</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Insere um objeto
        /// </summary>
        /// <param name="entity">Objeto a ser adicionado</param>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Remove um objeto
        /// </summary>
        /// <param name="entity">Objeto a ser adicionado</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Remove o objeto especificado
        /// </summary>
        /// <param name="entity">Objeto a ser removido</param>
        void RemoveWithoutContext(TEntity entity);

        /// <summary>
        /// Pesquisa por id
        /// </summary>
        /// <param name="id">Id do registro a ser recuperado</param>
        /// <returns>Retorna um objeto do tipo especificado, pesquisado por id</returns>
        TEntity FindById(int id);

        /// <summary>
        /// Atualiza um objeto
        /// </summary>
        /// <param name="entity">Objeto a ser atualizado</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Lista todos os registros da referida entidade
        /// </summary>
        /// <returns>Retorna uma lista com todos os objetos do tipo especificado</returns>
        IList<TEntity> ListAll();

        TEntity Merge(int id, TEntity entity);
    }
}
