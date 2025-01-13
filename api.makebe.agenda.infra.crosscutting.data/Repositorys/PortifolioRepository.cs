using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class PortifolioRepository : IPortifolioRepository
    {
        private readonly DbAgenda _dbAgenda;
        public PortifolioRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<PortifolioDTO> BuscarPorId(int id)
        {
            var query = @"SELECT p.*, lp.LojaId, cp.ColaboradorId, lp.Id AS LojaPortifolioId, cp.Id AS ColaboradorPortifolioId FROM Portifolio p 
                            LEFT JOIN LojaPortifolio lp On lp.PortifolioId  = p.Id 
                            LEFT JOIN ColaboradorPortifolio cp on cp.PortifolioId  = p.Id  
                         where p.Id = @Id";
            var result = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<PortifolioDTO>(query, new { Id = id }) ?? new PortifolioDTO();
            return result;
        }

        public async Task<int> Salvar(Portifolio portifolio)
        {
            var query = @"INSERT INTO Portifolio (Titulo, SubTitulo, Texto, Status, DataCadastro, DataAtualizacao)
                          VALUES 
                          (@Titulo, @SubTitulo, @Texto, @Status, @DataCadastro, @DataAtualizacao);
                        SELECT LAST_INSERT_ID();";
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(query, portifolio, _dbAgenda.Transaction);

            return result;
        }
        public async Task<Portifolio> Atualizar(Portifolio portifolio)
        {
            var query = @"UPDATE Portifolio  SET 
                          Titulo  = @Titulo,
                          SubTitulo  = @SubTitulo,
                          Texto  = @Texto
                          WHERE Id = @Id";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, portifolio);
            return portifolio;
        }

        public async Task<bool> Deastivar(int id)
        {
            var query = @"UPDATE Portifolio  SET 
                          Status  = 0
                          WHERE Id = @Id
                          ";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, new { Id = id }) > 0;
            return result;
        }
    }
}
