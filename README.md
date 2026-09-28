# Transacao.API

Microserviço de processamento de pagamentos que valida o limite do cliente, registra transações e solicita atualização de saldo via um gateway de cliente.

## Especificação técnica

- Plataforma: .NET 10 (ASP.NET Core Web API)
- Arquitetura: API -> UseCase -> Domain -> Infrastructure (gateway HTTP) -> Repository
- Serialização: System.Text.Json
- HTTP client: IHttpClientFactory (AddHttpClient)
- Autenticação: Bearer JWT validado por filtro (ValidarTokenAttribute)
- Persistência: abstração de repositório para salvar a entidade Transacao

## Regras de negócio

1. Apenas processar pagamento se houver token Bearer válido.
2. Requisição de pagamento (RequisicaoPagamentoDto) contém:
   - Id (Guid) — identificador do cliente
   - Valor (decimal) — valor da transação
3. Se o cliente não existir → rejeitar com: "transação não autorizada: cliente não encontrado".
4. Se ValorLimite do cliente for menor que Valor solicitado → rejeitar com: "transação não autorizada: saldo/limite indisponível".
5. Se persistência da transação ou atualização do saldo falhar → rejeitar com mensagem adequada.
6. Se tudo ocorrer corretamente → retornar sucesso com NumeroTransacao, Data e Valor.
7. O serviço de clientes deve expor o endpoint GET /api/Clientes/{id} retornando um objeto ClienteDto. Se o serviço retornar outro formato (ex.: lista ou HTML de erro), a desserialização para ClienteDto falhará.

## Contratos importantes

- ClienteDto
  - Id: Guid
  - Nome: string
  - CPF: string
  - ValorLimite: decimal

- RequisicaoPagamentoDto
  - Id: Guid
  - Valor: decimal

- RespostaTransacaoDto
  - Sucesso: bool
  - Mensagem: string
  - NumeroTransacao: string? (quando sucesso)
  - Data: DateTime? (quando sucesso)
  - Valor: decimal? (quando sucesso)

## Endpoints

- POST /api/Pagamentos
  - Segurança: Authorization: Bearer {token}
  - Corpo (exemplo):

```json
{
  "id": "f6d89141-60e4-4f39-9dc6-72587a461ad8",
  "valor": 150.00
}
```

  - Resposta de sucesso (200):

```json
{
  "sucesso": true,
  "mensagem": "transação aprovada",
  "numeroTransacao": "GUID-string",
  "data": "2026-09-28T12:00:00Z",
  "valor": 150.00
}
```

## Bibliotecas e pacotes

- Dapper (2.1.0)
- Microsoft.OpenApi (1.2.3)
- Swashbuckle.AspNetCore (6.4.0)
- Microsoft.Extensions.Http (IHttpClientFactory — parte do ASP.NET Core)
- System.Text.Json (serialização)

Consulte Transacao.API.csproj para versões exatas.

## Configuração

- Configure a base URL do serviço de clientes ao registrar o HttpClient (Program.cs ou appsettings): por exemplo, ClienteApi:BaseUrl.
- Configure parâmetros JWT (Issuer, Audience, Secret) usados pelo validador de token.
- Locais típicos: appsettings.json, appsettings.Development.json ou variáveis de ambiente.

Exemplo (ilustrativo) em appsettings.json:

```json
{
  "ClienteApi": { "BaseUrl": "https://clienteservice.local" },
  "Jwt": { "Issuer": "Issuer", "Audience": "Audience", "Secret": "secret-dev" }
}
```

## Como baixar e rodar

1. Clonar

```bash
git clone https://github.com/codevolper/transacao.api.git
cd Transacao.API
```

2. Restaurar dependências

```bash
dotnet restore
```

3. Compilar

```bash
dotnet build
```

4. Executar em desenvolvimento

```bash
dotnet run --project Transacao.API/Transacao.API.csproj
```

5. No Visual Studio

- Abra Transacao.API.slnx
- Defina Transacao.API (projeto dentro da solução) como startup
- Configure appsettings.Development.json ou perfil de inicialização conforme necessário
- Inicie com F5 ou Ctrl+F5

## Troubleshooting

- JsonException ao desserializar ClienteDto:
  - Verifique response.StatusCode e o corpo da resposta do serviço de clientes.
  - Confirme que a chamada usa GET /api/Clientes/{id} (incluindo o id no caminho). Se a chamada usar apenas /api/Clientes, o serviço pode retornar uma lista ou HTML de erro.
  - Registre o conteúdo da resposta antes de chamar ReadFromJsonAsync para inspecionar o payload.
  - Se o serviço retornar lista, desserialize List<ClienteDto> e selecione o item correspondente ao id ou corrija o endpoint.

- Falha de autenticação:
  - Verifique o header Authorization: "Bearer {token}" e as configurações do validador de token.

## Observações

- Ajuste URLs e segredos por ambiente (desenvolvimento/produção).
- Consulte arquivos .csproj para dependências exatas.

---

Arquivo gerado automaticamente com especificação técnica e instruções básicas para desenvolvimento.
