using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;
using PesquisarVitrineEvent;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class VitrineRepository : IVitrineRepository
    {
        private readonly DbAgenda _dbAgenda;

        public VitrineRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<List<ItemVitrineResponse>> PesquisarAsync(string valorItem, CancellationToken cancellationToken)
        {
            var sql = @"SELECT DISTINCT
                            RazaoSocial,
                            IdLoja,
                            DescricaoCategoria,
                            UrlImagem
                        FROM vw_vitrine_servicos
                        WHERE
                        (
                            @ValorItem IS NULL
                            OR @ValorItem = ''
                            OR RazaoSocial LIKE CONCAT('%', @ValorItem, '%')
                            OR Descricao LIKE CONCAT('%', @ValorItem, '%')
                            OR DescricaoCategoria LIKE CONCAT('%', @ValorItem, '%')
                            OR Nome LIKE CONCAT('%', @ValorItem, '%')
                        )
                        ORDER BY RazaoSocial;";

            var command = new CommandDefinition(
                sql,
                new { ValorItem = valorItem },
                cancellationToken: cancellationToken);

            var response = await _dbAgenda.Connection.QueryAsync<ItemVitrineResponse>(command);

            return response.ToList();
        }
    }
}
