using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enum;
using api.makebe.agenda.domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace api.makebe.agenda.applications.Services
{
    public class PermissaoAutenticacaoService : IPermissaoAutenticacaoService
    {
        private IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        public PermissaoAutenticacaoService(IUsuarioSessaoDomainService usuarioSessaoDomainService, IConfiguration configuration)
        {
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _configuration = configuration;
            _secretKey = _configuration["SysKey"] ?? string.Empty;
        }
        public async Task<IEnumerable<Papeis>> ValidarPermissaoAutenticacao(PapeisPermissoes papeisPermissoes, string papeis
            , string chave)
        {
            var sessao = await _usuarioSessaoDomainService.BuscarSessao(chave);
            if (sessao.UsuarioId == Guid.Empty)
                return Enumerable.Empty<Papeis>();

            var papeisRetorno = JsonConvert.DeserializeObject<IEnumerable<Papeis>>(papeis);
            var papeisService = PermissaoEnumParaTexto(papeisPermissoes);
            return papeisRetorno?.Where(x => x.Descricao == papeisService) ?? Enumerable.Empty<Papeis>();
        }

        public string PermissaoEnumParaTexto(PapeisPermissoes papeisPermissoes)
        {
            string descricao = papeisPermissoes switch
            {
                PapeisPermissoes.GerenciaPropriaConta => PapaeisConstant.GerenciaPropriaConta,
                PapeisPermissoes.GerenciaContasGestor => PapaeisConstant.GerenciaContasGestor,
                PapeisPermissoes.GerenciaTodasContas => PapaeisConstant.GerenciaTodasContas,
                PapeisPermissoes.GerenciaPropriaContaCliente => PapaeisConstant.GerenciaPropriaContaCliente,
                _ => string.Empty
            };

            return descricao;
        }
    }
}
