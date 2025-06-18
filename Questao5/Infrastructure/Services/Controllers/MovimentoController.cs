using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Language;

namespace Questao5.Infrastructure.Services.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly MovimentarContaHandler _movimentarHandler;
        private readonly ConsultarSaldoHandler _saldoHandler;

        public MovimentoController(MovimentarContaHandler movimentarHandler, ConsultarSaldoHandler saldoHandler)
        {
            _movimentarHandler = movimentarHandler;
            _saldoHandler = saldoHandler;
        }

        [HttpPost]
        public IActionResult Post([FromBody] MovimentarContaCommand command)
        {
            try
            {
                var response = _movimentarHandler.Handle(command);
                return Ok(response);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { tipo = ex.Tipo, mensagem = ex.Message });
            }
        }

        [HttpGet("{numero}/saldo")]
        public IActionResult GetSaldo(int numero)
        {
            try
            {
                var response = _saldoHandler.Handle(new ConsultarSaldoQuery { NumeroConta = numero });
                return Ok(response);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { tipo = ex.Tipo, mensagem = ex.Message });
            }
        }
    }

}
