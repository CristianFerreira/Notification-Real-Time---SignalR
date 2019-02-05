using Repository.Configuration;
using System;
using System.Data.Entity;

namespace Repository.Util
{
    /// <summary>
    /// Classe utilitária, responsável por encapsular o tratamento das transações da aplicação.
    /// </summary>
    public class TransactionHelper
    {
        /// <summary>
        /// Propriedade que representa o contexto do Entity Framework. Através deste, temos acesso 
        /// aos repositórios disponíveis da aplicação
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Propriedade que representa a transação corrente
        /// </summary>
        private DbContextTransaction transaction;

        /// <summary>
        /// Método responsável pelo tratamento das transações da aplicação.
        /// Segue abaixo detalhamento dos processos executados por esta função:
        ///     - Cria e inicializa um contexto
        ///     - Cria e inicializa uma transação, baseada no contexto criado anteriormente
        ///     - Instancia e inicializa os repositórios passados como parâmetro, via argumento "instanceDaos".
        ///         Parametrizando-os com o contexto e transação correntes
        ///     - Executa as ações passadas como argumento via parâmetro "transactionalAction"
        ///     - Executa o commit das operações
        ///     - Caso alguma exceção seja lançada, é feito o rollback das operações e a exceção é repassada
        ///     para ser tratada na respectiva camada de interesse
        /// </summary>
        /// <param name="transactionalAction">Action que encapsula as ações a serem executadas 
        /// na mesma transação</param>
        /// <param name="instanceDaos">Action que encapsula os repositórios a serem inicializados. De 
        /// acordo com o contexto e transação atuais</param>
        public void ExecuteComplexTransactionalOperation(Action transactionalAction,
            Action<DbContext, DbContextTransaction> instanceDaos)
        {
            using (var context = new EntityContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        this.context = context;
                        this.transaction = transaction;
                        instanceDaos(context, transaction);
                        transactionalAction();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
