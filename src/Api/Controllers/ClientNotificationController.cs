using Domain.RequestModels;
using Domain.ResponseModels;
using Service.Contracts;
using Service.Helpers;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    /// <summary>
    /// Notificação do Cliente
    /// </summary>
    [RoutePrefix("api/clientnotification")]
    public class ClientNotificationController : ApiController
    {
        private readonly IClientNotificationService _clientNotificationService;

        public ClientNotificationController(IClientNotificationService clientNotificationService)
            => _clientNotificationService = clientNotificationService;

        /// <summary>
        /// Listar todas notificações do cliente por aplicação.
        /// </summary>
        /// <param name="request">modelo esperado para consultar as notificações</param>
        /// <remarks>Listar todas notificações do cliente por aplicação.</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPost]
        [Route("list")]
        [SwaggerResponse(HttpStatusCode.OK, 
                         "Devolve um objeto do tipo IEnumerable<ClientNotificationsListResponse>", 
                         Type = typeof(IEnumerable<ClientNotificationsListResponse>))]
        public async Task<IHttpActionResult> List(ClientNotificationRequest request)
        => await Task.FromResult(Ok(_clientNotificationService.List(request)));

        /// <summary>
        /// Listar todas notificações não visualizadas do cliente por aplicação.
        /// </summary>
        /// <param name="request">modelo esperado para consultar as notificações</param>
        /// <remarks>Listar todas notificações não visualizadas do cliente por aplicação.</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPost]
        [Route("list/notvisualized")]
        [SwaggerResponse(HttpStatusCode.OK,
                         "Devolve um objeto do tipo IEnumerable<ClientNotificationsListResponse>",
                         Type = typeof(IEnumerable<ClientNotificationsListResponse>))]
        public async Task<IHttpActionResult> ListNotVisualized(ClientNotificationRequest request)
        => await Task.FromResult(Ok(_clientNotificationService.ListNotVisualized(request)));

        /// <summary>
        /// Listar todas notificações visualizadas do cliente por aplicação.
        /// </summary>
        /// <param name="request">modelo esperado para consultar as notificações</param>
        /// <remarks>Listar todas notificações visualizadas do cliente por aplicação.</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPost]
        [Route("list/visualized")]
        [SwaggerResponse(HttpStatusCode.OK,
                         "Devolve um objeto do tipo IEnumerable<ClientNotificationsListResponse>",
                         Type = typeof(IEnumerable<ClientNotificationsListResponse>))]
        public async Task<IHttpActionResult> ListVisualized(ClientNotificationRequest request)
        => await Task.FromResult(Ok(_clientNotificationService.ListVisualized(request)));

        /// <summary>
        /// Consultar quantidade de notificações do cliente por aplicação.
        /// </summary>
        /// <param name="request">modelo esperado para consultar quantidade de notificações</param>
        /// <remarks>Consultar quantidade de notificações do cliente por aplicação.</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPost]
        [Route("countnotificationsunchecked")]
        [SwaggerResponse(HttpStatusCode.OK,
                         "Devolve um objeto do tipo int",
                         Type = typeof(int))]
        public async Task<IHttpActionResult> CountNotificationsUnchecked(ClientNotificationRequest request)
        => await Task.FromResult(Ok(_clientNotificationService.CountNotificationsUnchecked(request)));


        /// <summary>
        /// Notificar o(s) cliente(s) por aplicação e fazer a persistencia dos dados.
        /// </summary>
        /// <param name="request">Corresponde ao modelo esperado</param>
        /// <remarks>Notificar o(s) cliente(s) por aplicação e fazer a persistencia dos dados.</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPost]
        [Route("notify")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> NotifyClients(NotifyClientRequest request)
        {
            request.SetUserContext(Request.UserId());
            _clientNotificationService.NotifyClients(request);
            return await Task.FromResult(Ok());
        }

        /// <summary>
        /// Notificar uma lista de notificações do(s) cliente(s) por aplicação e fazer a persistencia dos dados.
        /// </summary>
        /// <param name="request">Corresponde ao modelo esperado</param>
        /// <remarks>Notificar uma lista de notificações do(s) cliente(s) por aplicação e fazer a persistencia dos dados.</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPost]
        [Route("notify/many")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> NotifyManyNotificationsClients(NotifyManyNotificationsClientsRequest request)
        {
            request.SetUserContext(Request.UserId());
            _clientNotificationService.NotifyNotificationsClients(request);
            return await Task.FromResult(Ok());
        }

        /// <summary>
        /// Fazer a checagem das notificações do cliente por aplicação
        /// </summary>
        /// <param name="request">dados esperados para checkar a notificação</param>
        /// <remarks>Fazer a checagem das notificações do cliente por aplicação</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPut]
        [Route("checked/{userId}")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> Checked(ClientNotificationRequest request)
        {
            _clientNotificationService.Checked(request, Request.UserId());
            return await Task.FromResult(Ok());
        }

        /// <summary>
        /// Fazer a visualização da notificação do cliente.
        /// </summary>
        /// <param name="id">Id da notificação do cliente para visualizar notificações</param>
        /// <remarks>Fazer a visualização da notificação do cliente.</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPut]
        [Route("visualized/{id}")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> Visualized(int id)
        {
            _clientNotificationService.Visualized(id, Request.UserId());
            return await Task.FromResult(Ok());
        }

        /// <summary>
        /// Desabilitar a notificação do cliente.
        /// </summary>
        /// <param name="id">Id da notificação do cliente</param>
        /// <remarks>Desabilitar a notificação do cliente.</remarks>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="404">Registro não encontrado</response>
        /// <response code="500">Exceção gerada ao processar a requisição</response>
        [HttpPut]
        [Route("disable/{id}")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> Disable(int id)
        {
            _clientNotificationService.Disable(id, Request.UserId());
            return await Task.FromResult(Ok());
        }



    }
}