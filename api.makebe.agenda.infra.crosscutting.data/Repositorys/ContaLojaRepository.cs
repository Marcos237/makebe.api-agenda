using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ContaLojaRepository : IContaLojaRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ContaLojaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<int> Salvar(ContaLoja loja)
        {
            var sql = @"INSERT INTO ContaLoja (ContaId, LojaId, Status, DataCadastro) VALUES (@ContaId, @LojaId, @Status, @DataCadastro);
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                ContaId = loja.ContaId,
                LojaId = loja.LojaId,
                Status = loja.Status,   
                DataCadastro = loja.DataCadastro
            }, _dbAgenda.Transaction);

            return retorno;
        }
        public async Task<Loja> BuscarLojaPorCNPJ(string cnpj, Guid contaId)
        {
            var sql = @"SELECT l.Id, l.RazaoSocial , l.CNPJ , l.Email, l.Telefone, l.Status, l.DataCadastro, l.DataAtualizacao
                        FROM Loja l
                        INNER JOIN ContaLoja ul ON ul.LojaId = l.Id 
                        WHERE l.CNPJ = @CNPJ AND ul.ContaId <> @ContaId";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<Loja>(sql, new { Cnpj = cnpj, ContaId = contaId }) ?? new Loja();
            return retorno;
        }
    }
}
