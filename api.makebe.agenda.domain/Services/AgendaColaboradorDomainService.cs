using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using ContasEvent;

namespace api.makebe.agenda.domain.Services
{
    public class AgendaColaboradorDomainService : IAgendaContextDomainService<AgendaColaborador>, IAgendaColaboradorDomainService
    {
        private readonly IAgendaContextRepository<AgendaColaborador> _repository;
        private readonly IAgendaColaboradorRepository _agendaColaboradorRepository;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        public AgendaColaboradorDomainService(IAgendaContextRepository<AgendaColaborador> repository, IContaEventCrossCuttingService contaEventCrossCuttingService,
            IAgendaColaboradorRepository agendaColaboradorRepository)
        {
            _repository = repository;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _agendaColaboradorRepository = agendaColaboradorRepository;
        }
        public async Task<PaginacaoDTO<AgendaDTO>> BuscarPaginado(PaginacaoDTO<AgendaDTO> paginacao, string contaId)
        {
            var usuarioConsultadoEvent = new UsuarioContaConsultadoPorContaEvent() { IdConta = PropiedadesHelper.ParseGuidOrDefault(contaId ?? string.Empty) };
            var usuariosConta = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(usuarioConsultadoEvent);

            var agendas = await _repository.BuscarPaginado(paginacao, contaId ?? string.Empty);
            agendas.objetoPesquisa = paginacao?.objetoPesquisa ?? new AgendaDTO();
            var retornoColaborador = await MontarColaboradorProfissional(agendas, usuariosConta.UsuariosEvents);
            var colaboradorFiltrado = await Filtrar(retornoColaborador);
            return colaboradorFiltrado;
        }

        public async Task<AgendaDTO> BuscarPorId(int id)
        {
            if (id == 0)
                return new AgendaDTO();

            var response = await _repository.BuscarPorId(id);
            var dataFinalDia = ValoresHelper.SetDateTimeCustomer(response?.AgendaBloqueadaFim);
            response!.Bloqueado = dataFinalDia == DateTime.Today.AddDays(1).AddMinutes(-1) ? true : false;

            return response!;
        }
        public async Task<AgendaDTO> BuscarPorIdColaborador(int idColaborador)
        {
            var response = await _agendaColaboradorRepository.BuscarPorIdColaborador(idColaborador);
            return response;
        }
        public async Task<int> Persistir(AgendaColaborador agendaColaborador)
        {
            agendaColaborador.Status = true;
            agendaColaborador.DataAtualizacao = DateTime.Now;
            if (agendaColaborador.Id == 0)
            {
                agendaColaborador.DataCadastro = DateTime.Now;
                var result = await _repository.Salvar(agendaColaborador);
                return result;
            }
            var resultUpdate = await _repository.Atualizar(agendaColaborador);
            return agendaColaborador.Id;
        }

        public async Task<PaginacaoDTO<AgendaDTO>> Filtrar(PaginacaoDTO<AgendaDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var pesquisa = paginacao?.objetoPesquisa ?? new AgendaDTO();

            var filtrados = await FiltrarPorDiaSemana(paginacao!, pesquisa);
            filtrados = await PesquisarPorAgendaAberta(pesquisa, filtrados);
            filtrados = await PesquisarPorBloqueio(pesquisa, filtrados);

            paginacao!.total = filtrados.Count();
            paginacao.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;
            paginacao.objetos = filtrados
                .Skip(paginacao.registroInicial)
                .Take(paginacao.quantidadePagina)
                .ToList();

            return await Task.FromResult(paginacao);
        }
        public async Task<IEnumerable<AgendaDTO>> FiltrarPorDiaSemana(PaginacaoDTO<AgendaDTO> paginacao, AgendaDTO? pesquisa)
        {
            return await Task.FromResult(paginacao?.objetos?.Where(objeto =>
                (string.IsNullOrWhiteSpace(pesquisa?.Nome) ||
                 (objeto.Nome?.Contains(pesquisa.Nome, StringComparison.OrdinalIgnoreCase) ?? false)) &&

                (!(pesquisa?.IdAgendaSemanaInicio > 0) == true && !(pesquisa?.IdAgendaSemanaFim > 0) == true ||
                 (
                     pesquisa.IdAgendaSemanaInicio <= pesquisa.IdAgendaSemanaFim
                         ? (objeto.IdAgendaSemanaInicio >= pesquisa.IdAgendaSemanaInicio &&
                            objeto.IdAgendaSemanaInicio <= pesquisa.IdAgendaSemanaFim)
                         : (objeto.IdAgendaSemanaInicio >= pesquisa.IdAgendaSemanaInicio ||
                            objeto.IdAgendaSemanaInicio <= pesquisa.IdAgendaSemanaFim)
                 ))
            )) ?? Enumerable.Empty<AgendaDTO>();
        }
        public async Task<IEnumerable<AgendaDTO>> PesquisarPorAgendaAberta(AgendaDTO agendaPesquisa, IEnumerable<AgendaDTO> agendas)
        {
            var dataInicioFormat = ValoresHelper.SetDateTimeCustomer(agendaPesquisa.AgendaAbertaInicio);
            var dataFimFormat = ValoresHelper.SetDateTimeCustomer(agendaPesquisa.AgendaAbertaFim);

            if (dataInicioFormat != null && dataFimFormat != null)
                return agendas.Where(agendaItem => dataInicioFormat >= ValoresHelper.SetDateTimeCustomer(agendaItem.AgendaAbertaInicio) &&
                    dataFimFormat <= ValoresHelper.SetDateTimeCustomer(agendaItem.AgendaAbertaFim));

            if (dataInicioFormat != null)
                return agendas.Where(agendaItem =>
                ValoresHelper.SetDateTimeCustomer(agendaItem.AgendaAbertaInicio) == dataInicioFormat);

            if (dataFimFormat != null)
                return agendas.Where(agendaItem =>
                ValoresHelper.SetDateTimeCustomer(agendaItem.AgendaAbertaFim) == dataFimFormat);

            return await Task.FromResult(agendas);
        }

        public async Task<IEnumerable<AgendaDTO>> PesquisarPorBloqueio(AgendaDTO agendaPesquisa, IEnumerable<AgendaDTO> agendas)
        {
            var dataInicioFormat = ValoresHelper.SetDateHourMinuteCustomer(agendaPesquisa.AgendaBloqueadaInicio);
            var dataFimFormat = ValoresHelper.SetDateHourMinuteCustomer(agendaPesquisa.AgendaBloqueadaFim);

            if (dataInicioFormat != null && dataFimFormat != null)
                return agendas.Where(agendaItem =>
                    ValoresHelper.SetDateHourMinuteCustomer(agendaItem.AgendaBloqueadaInicio) >= dataInicioFormat &&
                    ValoresHelper.SetDateHourMinuteCustomer(agendaItem.AgendaBloqueadaFim) >= dataFimFormat
                );

            if (dataInicioFormat != null)
                return agendas.Where(agendaItem =>
                ValoresHelper.SetDateHourMinuteCustomer(agendaItem.AgendaBloqueadaInicio) == dataInicioFormat);

            if (dataFimFormat != null)
                return agendas.Where(agendaItem =>
                 ValoresHelper.SetDateHourMinuteCustomer(agendaItem.AgendaBloqueadaFim) == dataFimFormat
                );

            return await Task.FromResult(agendas);
        }

        public async Task<PaginacaoDTO<AgendaDTO>> MontarColaboradorProfissional(PaginacaoDTO<AgendaDTO> paginacao, IEnumerable<UsuarioEvent>? usuarios)
        {
            if (paginacao.objetos == null)
                return new PaginacaoDTO<AgendaDTO> { objetos = new List<AgendaDTO>() };

            var agendaFiltrados = paginacao?.objetos?.Join(usuarios,
                agenda =>
                agenda?.UsuarioId?.ToString(), usuario => usuario?.Id.ToString() ?? string.Empty,
                    (agenda, usuario) => AdicionarAgenda(agenda, usuario));

            return await Task.FromResult(new PaginacaoDTO<AgendaDTO>
            {
                paginaAtual = paginacao?.paginaAtual ?? 1,
                totalPaginas = paginacao?.totalPaginas ?? 1,
                quantidadePagina = paginacao?.quantidadePagina ?? 10,
                registroInicial = paginacao?.registroInicial ?? 1,
                objetoPesquisa = paginacao?.objetoPesquisa ?? new AgendaDTO(),
                total = paginacao?.total ?? 0,
                objetos = agendaFiltrados?.ToList() ?? new List<AgendaDTO>()
            });
        }

        private static AgendaDTO AdicionarAgenda(AgendaDTO agenda, UsuarioEvent usuario)
        {
            return new AgendaDTO()
            {
                Id = agenda.Id,
                UsuarioId = agenda.UsuarioId,
                Nome = usuario.Nome,
                AgendaAbertaInicio = agenda.AgendaAbertaInicio,
                AgendaAbertaFim = agenda.AgendaAbertaFim,
                Bloqueado = agenda.Bloqueado,
                IsTodoDia = agenda.IsTodoDia,
                IdAgendaSemanaInicio = agenda.IdAgendaSemanaInicio,
                IdAgendaSemanaFim = agenda.IdAgendaSemanaFim,
                AgendaBloqueadaInicio = agenda.AgendaBloqueadaInicio,
                AgendaBloqueadaFim = agenda.AgendaBloqueadaFim,
                DiaInicioSemana = agenda.DiaInicioSemana,
                DiaSemanaFim = agenda.DiaSemanaFim
            };
        }
    }
}
