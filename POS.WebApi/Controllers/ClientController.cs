using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Clients.Commands;
using POS.Application.Features.Clients.Queries;
using POS.Application.Models.Persistence;
using System.Net;

namespace POS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator mediator;

        public ClientController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Obtiene lista de clientes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ListResponse>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<ListResponse>>> List([FromQuery] ListQuery request)
        {
            return Ok(await mediator.Send(request));
        }

        /// <summary>
        /// Obtiene lista paginada de clientes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginationResponse<PaginatedListResponse>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PaginationResponse<PaginatedListResponse>>> PaginatedList([FromQuery] PaginatedListQuery request)
        {
            return Ok(await mediator.Send(request));
        }

        /// <summary>
        /// Crea un cliente
        /// </summary>
        /// <param name="request"></param>
        /// <!-- Bad request response -->
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CreateResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CreateResponse>> Create([FromBody] CreateCommand request)
        {
            return Ok(await mediator.Send(request));
        }

        /// <summary>
        /// Actualiza un cliente
        /// </summary>
        /// <param name="request"></param>
        /// <!-- Bad request response -->
        /// <returns></returns>
        [HttpPut("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UpdateResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<UpdateResponse>> Update([FromBody] UpdateCommand request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}
