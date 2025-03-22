using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ContaServicoRepository : IContaServicoRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ContaServicoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }


        public async Task<IEnumerable<ContaServico>> BuscarServicoPorConta(string contaId)
        {
            var sql = @"SELECT Id, ContaId,ServicoId,Status, DataCadastro FROM ContaServico cs 
                        WHERE cs.ContaId = @ContaId";

            var response = await _dbAgenda.Connection.QueryAsync<ContaServico>(sql, new { ContaId = contaId }) ?? Enumerable.Empty<ContaServico>();
            return response;
        }

        public async Task<int> Salvar(ContaServico contaServico)
        {
            var sql = @"INSERT INTO ContaServico (ContaId, ServicoId, Status, DataCadastro)
                        VALUES(@ContaId, @ServicoId, @Status, @DataCadastro)";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                ContaId = contaServico.ContaId,
                ServicoId = contaServico.ServicoId,
                DataCadastro = contaServico.DataCadastro,
                Status = contaServico.Status
            }, _dbAgenda.Transaction);
            return retorno;
        }
        public async Task<bool> Atualizar(ContaServico contaServico)
        {
            var sql = @"
                            UPDATE ContaServico SET 
                            ContaId = @ContaId,
                            DataCadastro = @DataCadastro,
                            Status = @Status
                            
                            WHERE ServicoId = @ServicoId;";

            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                ServicoId = contaServico.ServicoId,
                ContaId = contaServico.ContaId,
                DataCadastro = contaServico.DataCadastro,
                Status = contaServico.Status
            }, _dbAgenda.Transaction);

            return retorno > 0;
        }
    }
}
