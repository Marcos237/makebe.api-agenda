using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorPortifolioRepository : IPortifolioContextRepository<ColaboradorPortifolio, PortifolioDTO>
    {
        private readonly DbAgenda _dbAgenda;
        public ColaboradorPortifolioRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<PortifolioDTO>> BuscarPortifolios(string contaId)
        {

            var sql = @"SELECT DISTINCT  p.Id, CAST(cc.ContaId AS CHAR) AS ContaId, p.Titulo, p.SubTitulo, CAST(c.UsuarioId AS CHAR) AS UsuarioId, cp.Id AS ColaboradorPortifolioId FROM Portifolio p 
                            INNER JOIN ColaboradorPortifolio cp ON cp.PortifolioId = p.Id
                            INNER JOIN Colaborador c ON c.Id = cp.ColaboradorId
                            INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id
                            WHERE cc.ContaId  = @ContaId
                            AND p.Status = 1";

            var portifolios = await _dbAgenda.Connection.QueryAsync<PortifolioDTO>(sql, new {ContaId = contaId}) ?? Enumerable.Empty<PortifolioDTO>();
            return portifolios;
        }

        public async Task<int> Salvar(ColaboradorPortifolio item)
        {
            var sql = @"INSERT INTO ColaboradorPortifolio (ColaboradorId, PortifolioId, Status, DataCadastro) VALUES (@ColaboradorId, @PortifolioId, @Status, @DataCadastro);
                        SELECT LAST_INSERT_ID();";
            var parametros = new
            {
                ColaboradorId = item.ColaboradorId,
                PortifolioId = item.PortifolioId,
                Status = item.Status,
                DataCadastro = item.DataCadastro
            };
            var response = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros, _dbAgenda.Transaction);
            return response;
        }
        public async Task<bool> Atualizar(ColaboradorPortifolio item)
        {
            var sql = @"UPDATE ColaboradorPortifolio  SET 
                                  ColaboradorId = @ColaboradorId,
                                  PortifolioId = @PortifolioId, 
                                  Status = @Status, 
                                  DataCadastro = @DataCadastro
                                  WHERE Id = @ID";
            var parametros = new
            {
                ColaboradorId = item.ColaboradorId,
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