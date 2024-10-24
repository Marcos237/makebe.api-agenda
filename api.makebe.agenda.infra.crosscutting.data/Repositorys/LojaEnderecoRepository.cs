using api.makebe.agenda.domain.Entidades;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class LojaEnderecoRepository : ILojaEnderecoRepository
    {
        private readonly DbAgenda _dbAgenda;
        public LojaEnderecoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<int> SalvarLojaEndereco(LojaEndereco endereco)
        {
            var query = @"INSERT INTO LojaEndereco (LojaId, EnderecoId, DataCadastro) VALUES (@LojaId, @EnderecoId, @DataCadastro);
                           SELECT LAST_INSERT_ID() AS LastInsertedId;";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, endereco);
            return result;

        }
    }
}
