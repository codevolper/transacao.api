using Transacao.API.API.Models;
using Transacao.API.Domain.DTOs;
using Transacao.API.Domain.Interfaces;

namespace Transacao.API.Application.UseCases
{
    public class ProcessarPagamentoUseCase : IProcessarPagamentoUseCase
    {
        private readonly ITransacaoRepository _repositorio;
        private readonly Transacao.API.Infrastructure.Gateways.IClienteGateway _clienteGateway;

        public ProcessarPagamentoUseCase(ITransacaoRepository repositorio, Transacao.API.Infrastructure.Gateways.IClienteGateway clienteGateway)
        {
            _repositorio = repositorio;
            _clienteGateway = clienteGateway;
        }

        public async Task<RespostaTransacaoDto> ProcessarAsync(RequisicaoPagamentoDto requisicao, string token)
        {
            ClienteDto? cliente = await _clienteGateway.ObterClienteAsync(requisicao.Id, token);
            if (cliente == null)
            {
                return new RespostaTransacaoDto { Sucesso = false, Mensagem = "transação não autorizada: cliente não encontrado" };
            }

            if (cliente.ValorLimite < requisicao.Valor)
            {
                return new RespostaTransacaoDto { Sucesso = false, Mensagem = "transação não autorizada: saldo/limite indisponível" };
            }

            var transacao = new Transacao.API.Domain.Entidades.Transacao
            {
                ClienteId = requisicao.Id,
                Valor = requisicao.Valor,                
                NumeroTransacao = Guid.NewGuid()
            };

            await _repositorio.SalvarAsync(transacao);

            var atualizado = await _clienteGateway.AtualizarSaldoAsync(requisicao.Id, requisicao.Valor, token);
            if (!atualizado)
            {
                return new RespostaTransacaoDto { Sucesso = false, Mensagem = "transação não autorizada: falha ao atualizar saldo do cliente" };
            }

            return new RespostaTransacaoDto
            {
                Sucesso = true,
                Mensagem = "APROVADO",
                NumeroTransacao = transacao.NumeroTransacao.ToString(),                
                Valor = transacao.Valor
            };
        }
    }
}
