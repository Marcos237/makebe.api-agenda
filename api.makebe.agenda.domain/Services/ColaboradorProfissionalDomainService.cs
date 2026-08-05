using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorProfissionalDomainService : IColaboradorProfissionalDomainService
    {
        private readonly IColaboradorProfissionalRepository _ColaboradorProfissionalRepository;
        private readonly IAgendaColaboradorRepository _agendaColaboradorRepository;
        private readonly IUsuarioPermissaoDomainService _usuarioPermissaoDomainService;
        public ColaboradorProfissionalDomainService(IColaboradorProfissionalRepository ColaboradorProfissionalRepository,
            IAgendaColaboradorRepository agendaColaboradorRepository,
            IUsuarioPermissaoDomainService usuarioPermissaoDomainService)
        {
            _ColaboradorProfissionalRepository = ColaboradorProfissionalRepository;
            _agendaColaboradorRepository = agendaColaboradorRepository;
            _usuarioPermissaoDomainService = usuarioPermissaoDomainService;
        }
        public async Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginado(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao, string contaId)
        {
            var possuiAcessoCompletoConta = await _usuarioPermissaoDomainService.PossuiAcessoCompletoConta();
            var retornoColaborador = possuiAcessoCompletoConta
                ? await _ColaboradorProfissionalRepository.BuscarPaginadoPorContaId(contaId, paginacao)
                : await BuscarPaginadoPorUsuarioAutenticado(paginacao);

            retornoColaborador.totalPaginas = (retornoColaborador.total + retornoColaborador.quantidadePagina - 1) / retornoColaborador.quantidadePagina;
            return retornoColaborador;
        }

        public async Task<ColaboradorProfissionalDTO> BuscarPorId(int id)
        {
            var retorno = await _ColaboradorProfissionalRepository.BuscarPorId(id);
            retorno.Servicos = await _ColaboradorProfissionalRepository.BuscarServicosPorColaboradorId(retorno.ColaboradorId);
            return retorno;
        }

        public async Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorConta(string contaId)
        {
            var possuiAcessoCompletoConta = await _usuarioPermissaoDomainService.PossuiAcessoCompletoConta();
            return possuiAcessoCompletoConta
                ? await _ColaboradorProfissionalRepository.BuscarPorContaId(contaId)
                : await BuscarPorUsuarioAutenticado();

        }
        public async Task<bool> BuscarAgendaVisible(int colaboradorId)
        {
            var agenda = await _agendaColaboradorRepository.BuscarAgendaPorColaboradorId(colaboradorId);
            if (agenda == null)
                return false;

            if (agenda.IsBloqueadoHoje)
                return false;

            var hoje = DateTime.Today;
            var agendaInicio = ValoresHelper.SetDateTimeCustomer(agenda.AgendaAbertaInicio);
            var agendaFim = ValoresHelper.SetDateTimeCustomer(agenda.AgendaAbertaFim);
            var agendaBloqueadaInicio = ValoresHelper.SetDateTimeCustomer(agenda.AgendaBloqueadaInicio);
            var agendaBloqueadaFim = ValoresHelper.SetDateTimeCustomer(agenda.AgendaBloqueadaFim);

            var hojeDentroAberta = agendaInicio.HasValue &&
                                         agendaFim.HasValue &&
                                         (hoje > agendaInicio.Value.Date && hoje < agendaFim.Value.Date);

            return hojeDentroAberta;
        }
        public async Task<int> Salvar(ColaboradorProfissional colaborador)
        {
            colaborador.ServicoId = colaborador.Servicos?.FirstOrDefault()?.IdServico ?? colaborador.ServicoId;
            if (colaborador.Id == 0)
            {
                colaborador.DataAtualizacao = DateTime.Now;
                colaborador.DataCadastro = DateTime.Now;
                colaborador.Status = true;
                var retornoSalvar = await _ColaboradorProfissionalRepository.Salvar(colaborador);
                await AtualizarServicos(colaborador.ColaboradorId, colaborador.Servicos);
                return retornoSalvar;
            }
            colaborador.DataAtualizacao = DateTime.Now;
            await _ColaboradorProfissionalRepository.Atualizar(colaborador);
            await AtualizarServicos(colaborador.ColaboradorId, colaborador.Servicos);
            return colaborador.Id;
        }

        public async Task<bool> Desativar(int id)
        {
            return await _ColaboradorProfissionalRepository.Desativar(id);
        }

        private async Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginadoPorUsuarioAutenticado(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao)
        {
            var usuarioAutenticado = await _usuarioPermissaoDomainService.BuscarUsuarioAutenticado();
            return await _ColaboradorProfissionalRepository.BuscarPaginadoPorUsuario(usuarioAutenticado.UsuarioId.ToString(), paginacao);
        }

        private async Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorUsuarioAutenticado()
        {
            var usuarioAutenticado = await _usuarioPermissaoDomainService.BuscarUsuarioAutenticado();
            return await _ColaboradorProfissionalRepository.BuscarPorUsuarioId(usuarioAutenticado.UsuarioId.ToString());
        }

        private async Task AtualizarServicos(int colaboradorId, IEnumerable<ColaboradorServicos>? servicos)
        {
            var servicosAtuais = (await _ColaboradorProfissionalRepository.BuscarServicosPorColaboradorId(colaboradorId))
                .Select(x => x.IdServico)
                .ToHashSet();

            var novosServicos = (servicos ?? Enumerable.Empty<ColaboradorServicos>())
                .Select(x => x.IdServico)
                .Distinct()
                .ToHashSet();

            if (!novosServicos.Any())
            {
                await _ColaboradorProfissionalRepository.RemoverTodosServicos(colaboradorId);
                return;
            }

            var servicosParaAdicionar = novosServicos.Where(id => !servicosAtuais.Contains(id));
            foreach (var servicoId in servicosParaAdicionar)
            {
                await _ColaboradorProfissionalRepository.SalvarServico(new ColaboradorServicos
                {
                    IdColaborador = colaboradorId,
                    IdServico = servicoId,
                    DataCadastro = DateTime.Now
                });
            }

            var servicosParaRemover = servicosAtuais.Where(id => !novosServicos.Contains(id));
            foreach (var servicoId in servicosParaRemover)
                await _ColaboradorProfissionalRepository.RemoverServico(colaboradorId, servicoId);
        }
    }
}
