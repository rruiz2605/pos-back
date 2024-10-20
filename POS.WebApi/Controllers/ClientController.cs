using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Features.Clients.Queries;
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
        /// Obtiene la información del cliente
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
    }
}
