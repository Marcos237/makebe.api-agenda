using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces;

namespace api.makebe.agenda.domain.Services
{
    public class FiltrosAgendamentoDomainService : IFiltrosAgendamentoDomainService
    {
        public IEnumerable<AgendamentoDTO> FiltrarPorNomes(PaginacaoDTO<AgendamentoDTO> paginacao)
        {
            var objetosPaginacao = Enumerable.Empty<AgendamentoDTO>();
            var pesquisa = paginacao.objetoPesquisa ?? new AgendamentoDTO();
            if (!string.IsNullOrEmpty(pesquisa.NomeColaborador))
                objetosPaginacao = paginacao?.objetos?.Where(agenda => agenda.NomeColaborador == paginacao?.objetoPesquisa?.NomeColaborador) ?? Enumerable.Empty<AgendamentoDTO>();

            if (!string.IsNullOrEmpty(pesquisa.NomeUsuario))
                objetosPaginacao = paginacao?.objetos?.Where(agenda => agenda.NomeUsuario == paginacao?.objetoPesquisa?.NomeUsuario) ?? Enumerable.Empty<AgendamentoDTO>();

            if (!string.IsNullOrEmpty(pesquisa.RazaoSocial))
                objetosPaginacao = paginacao?.objetos?.Where(agenda => agenda.RazaoSocial == paginacao?.objetoPesquisa?.RazaoSocial) ?? Enumerable.Empty<AgendamentoDTO>();

            if (!string.IsNullOrEmpty(pesquisa.DescricaoServico))
                objetosPaginacao = paginacao?.objetos?.Where(agenda => agenda.DescricaoServico == paginacao?.objetoPesquisa?.DescricaoServico) ?? Enumerable.Empty<AgendamentoDTO>();

            return objetosPaginacao;
        }
        public IEnumerable<AgendamentoDTO> FiltrarPorDatas(PaginacaoDTO<AgendamentoDTO> paginacao)
        {
            var pesquisa = paginacao.objetoPesquisa ?? new AgendamentoDTO();
            var objetosPaginacao = Enumerable.Empty<AgendamentoDTO>();

            if (string.IsNullOrEmpty(pesquisa.DataInicioAgendamentoExtenso) && string.IsNullOrEmpty(pesquisa.DataTerminoAgendamentoExtenso))
                return paginacao.objetos ?? Enumerable.Empty<AgendamentoDTO>();

            if (!string.IsNullOrEmpty(pesquisa.DataInicioAgendamentoExtenso) && (string.IsNullOrEmpty(pesquisa.DataTerminoAgendamentoExtenso)))
                return objetosPaginacao = paginacao.objetos?.Where(agenda => agenda.DataInicioAgendamento == pesquisa.DataInicioAgendamento) ?? Enumerable.Empty<AgendamentoDTO>();

            if (string.IsNullOrEmpty(pesquisa.DataInicioAgendamentoExtenso) && (!string.IsNullOrEmpty(pesquisa.DataTerminoAgendamentoExtenso)))
                return objetosPaginacao = paginacao.objetos?.Where(agenda => agenda.DataInicioAgendamento == pesquisa.DataInicioAgendamento) ?? Enumerable.Empty<AgendamentoDTO>();

            if (!string.IsNullOrEmpty(pesquisa.DataInicioAgendamentoExtenso) && !string.IsNullOrEmpty(pesquisa.DataTerminoAgendamentoExtenso))
            {
                var dataInicio = DateTime.Parse(pesquisa.DataInicioAgendamentoExtenso);
                var dataTermino = DateTime.Parse(pesquisa.DataTerminoAgendamentoExtenso);

                return objetosPaginacao = paginacao.objetos?
                    .Where(agenda =>
                        DateTime.Parse(agenda?.DataInicioAgendamentoExtenso ?? string.Empty) >= dataInicio &&
                        DateTime.Parse(agenda?.DataInicioAgendamentoExtenso ?? string.Empty) <= dataTermino)
                    ?? Enumerable.Empty<AgendamentoDTO>();
            }

            return objetosPaginacao;

        }
    }
}
