using Microsoft.AspNetCore.Mvc;
using Transacao.API.API.Filters;
using Transacao.API.API.Models;
using Transacao.API.Application.UseCases;

namespace Transacao.API.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class PagamentosController : ControllerBase
    {
        private readonly IProcessarPagamentoUseCase _useCase;

        public PagamentosController(IProcessarPagamentoUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidarTokenAttribute))]
        public async Task<IActionResult> Processar([FromBody] RequisicaoPagamentoDto requisicao)
        {
            if (requisicao == null)
                return BadRequest(new RespostaTransacaoDto { Sucesso = false, Mensagem = "requisição inválida" });
                             

            var token = HttpContext.Items["BearerToken"] as string ?? string.Empty;
            var resultado = await _useCase.ProcessarAsync(requisicao, token);
            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }


    }
}
