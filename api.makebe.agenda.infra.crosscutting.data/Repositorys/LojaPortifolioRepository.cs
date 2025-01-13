using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class LojaPortifolioRepository : IPortifolioContextRepository<LojaPortifolio, PortifolioDTO>
    {
        private readonly DbAgenda _dbAgenda;
        public LojaPortifolioRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<IEnumerable<PortifolioDTO>> BuscarPortifolios(string contaId)
        {
            var query = @"SELECT DISTINCT  p.Id, CAST(cl.ContaId AS CHAR) AS ContaId, p.Titulo, p.SubTitulo, l.RazaoSocial, lp.Id AS LojaPortifolioId, lp.LojaId FROM Portifolio p 
                            INNER JOIN LojaPortifolio lp ON lp.PortifolioId  = p.Id 
                            INNER JOIN Loja l ON l.Id = lp.LojaId 
                            INNER JOIN ContaLoja cl ON cl.LojaId  = l.Id 
                            WHERE cl.ContaId  = @ContaId
                            AND p.Status = 1
                            ORDER BY p.Titulo";

            var portifolios = await _dbAgenda.Connection.QueryAsync<PortifolioDTO>(query, new { ContaId = contaId }) ?? Enumerable.Empty<PortifolioDTO>();
            return portifolios;
        }

        public async Task<int> Salvar(LojaPortifolio item)
        {
            var sql = @"INSERT INTO LojaPortifolio (LojaId, PortifolioId, Status, DataCadastro) VALUES (@LojaId, @PortifolioId, @Status, @DataCadastro);
                        SELECT LAST_INSERT_ID();";
            var parametros = new
            {
                LojaId = item.LojaId,
                PortifolioId = item.PortifolioId,
                Status = item.Status,
                DataCadastro = item.DataCadastro,
            };
            var response = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros, _dbAgenda.Transaction);
            return response;
        }
        public async Task<bool> Atualizar(LojaPortifolio item)
        {
            var sql = @"UPDATE LojaPortifolio  SET 
                                  LojaId = @LojaId,
                                  PortifolioId = @PortifolioId, 
                                  Status = @Status, 
                                  DataCadastro = @DataCadastro
                                  WHERE Id = @ID";
            var parametros = new
            {
                LojaId = item.LojaId,
                PortifolioId = item.PortifolioId,
                Status = item.Status,
                DataCadastro = item.DataCadastro,
                Id = item.Id
            };
            var response = await _dbAgenda.Connection.ExecuteAsync(sql, parametros, _dbAgenda.Transaction) > 0;
            return response;
        }
    }
}
