using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Entidades;

namespace api.makebe.agenda.domain.Services
{
    public class AgendamentoDomainService : IAgendamentoDomainService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IAgendaColaboradorDomainService _agendaColaboradorDomainService;
        private readonly IFiltrosAgendamentoDomainService _filtrosAgendamentoDomainService;
        public AgendamentoDomainService(IAgendamentoRepository agendamentoRepository, IFiltrosAgendamentoDomainService filtrosAgendamentoDomainService
            , IAgendaColaboradorDomainService agendaColaboradorDomainService)
        {
            _agendamentoRepository = agendamentoRepository;
            _filtrosAgendamentoDomainService = filtrosAgendamentoDomainService;
            _agendaColaboradorDomainService = agendaColaboradorDomainService;
        }
        public async Task<PaginacaoDTO<AgendamentoDTO>> MontarAgendamento(PaginacaoDTO<AgendamentoDTO> paginacao, string contaId,
            IEnumerable<UsuarioEvent>? UsuariosEvents, IEnumerable<UsuarioEvent>? ColaboradoresEvents)
        {
            var responseAgendamento = await _agendamentoRepository.BuscarPaginado(paginacao, contaId);
            var agendamentos = responseAgendamento.objetos?
                .Join(UsuariosEvents ?? Enumerable.Empty<UsuarioEvent>(),
                      agenda => PropiedadesHelper.ParseGuidOrDefault(agenda.IdUsuario),
                      usuario => usuario.Id,
                      (agenda, usuario) => new { agenda, usuario })
                .Join(ColaboradoresEvents ?? Enumerable.Empty<UsuarioEvent>(),
                      response => PropiedadesHelper.ParseGuidOrDefault(response.agenda.IdColaborador),
                      colaborador => colaborador.Id,
                      (response, colaborador) =>
                      {
                          return AdicionarAgendamento(response.usuario, response.agenda, colaborador);
                      })
                .ToList() ?? new List<AgendamentoDTO>();

            return new PaginacaoDTO<AgendamentoDTO>
            {
                paginaAtual = paginacao?.paginaAtual ?? 1,
                totalPaginas = paginacao?.totalPaginas ?? 1,
                quantidadePagina = paginacao?.quantidadePagina ?? 10,
                registroInicial = paginacao?.registroInicial ?? 1,
                total = paginacao?.total ?? agendamentos.Count,
                objetos = agendamentos
            };
        }


        public PaginacaoDTO<AgendamentoDTO> Filtrar(PaginacaoDTO<AgendamentoDTO> paginacao)
        {
            var response = _filtrosAgendamentoDomainService.FiltrarPorNomes(paginacao);
            response = _filtrosAgendamentoDomainService.FiltrarPorDatas(paginacao);

            paginacao.objetos = response;
            return paginacao ?? new PaginacaoDTO<AgendamentoDTO>();
        }

        public async Task<AgendamentoDTO> BuscarPorId(int id)
        {
            var response = await _agendamentoRepository.BuscarPorId(id);
            return response;
        }

        public async Task<PaginacaoDTO<AgendamentoConsultaDTO>> BuscarMeusAgendamentos(PaginacaoDTO<AgendamentoConsultaDTO> paginacao, string idUsuario)
        {
            var agendamentos = (await _agendamentoRepository.BuscarMeusAgendamentos(idUsuario)).ToList();
            agendamentos.ForEach(agendamento => agendamento.EhDesativado = agendamento.CalcularEhDesativado());

            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;
            paginacao.total = agendamentos.Count;
            paginacao.totalPaginas = paginacao.total.CalcularTotalPaginas(paginacao.quantidadePagina);
            paginacao.objetos = agendamentos
                .Skip(paginacao.registroInicial)
                .Take(paginacao.quantidadePagina)
                .ToList();

            return paginacao;
        }

        public async Task<IEnumerable<AgendamentoDTO>> BuscarPorAnoConta(int ano, int id, string conta)
        {
            var response = await _agendamentoRepository.BuscarPorAnoConta(ano, id, conta);
            return response;
        }

        public async Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoPorData(DateTime data, int id,  string conta)
        {
            var response = await  _agendamentoRepository.BuscarAgendamentoPorData(data, id,  conta);
            
            return response;
        }

        public async Task<int> Salvar(Agendamento agendamento, int idColaborador)
        {
            var idMaxAgendaColaborador = await _agendaColaboradorDomainService.BuscarPorIdColaborador(idColaborador);
            agendamento.IdAgendaColaborador = idMaxAgendaColaborador.Id;
            agendamento.DataAtualizacao = DateTime.Now;
            agendamento.Ativo = true;
            if (agendamento.Id > 0)
            {
                await _agendamentoRepository.Atualizar(agendamento);
                return agendamento.Id;
            }
            agendamento.DataCadastro = DateTime.Now;
            return await _agendamentoRepository.Salvar(agendamento);
        }
        public async Task<bool> Desativa(int id)
        {
            return await _agendamentoRepository.Desativar(id);
        }

        private static AgendamentoDTO AdicionarAgendamento(UsuarioEvent usuario, AgendamentoDTO agendamento, UsuarioEvent colaborador)
        {
            return new AgendamentoDTO
            {
                Id = agendamento.Id,
                IdUsuario = agendamento.IdUsuario,
                NomeUsuario = usuario.Nome,
                NomeColaborador = colaborador.Nome,
                IdColaborador = agendamento.IdColaborador,
                IdServico = agendamento.IdServico,
                DescricaoServico = agendamento.DescricaoServico,
                IdLoja = agendamento.IdLoja,
                RazaoSocial = agendamento.RazaoSocial,
                DataInicioAgendamento = agendamento.DataInicioAgendamento,
                DataTerminoAgendamento = agendamento.DataTerminoAgendamento,
                Ativo = agendamento.Ativo,
            };
        }
    }
}
