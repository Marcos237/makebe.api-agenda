using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Dapper;
using System.Reflection.Metadata;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class UsuarioLojaRepository : IUsuarioLojaRepository
    {
        private readonly DbAgenda _dbAgenda;
        public UsuarioLojaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<int> Salvar(UsuarioLoja loja)
        {
            var sql = @"INSERT INTO UsuarioLoja (UsuarioId, LojaId, Status, DataCadastro) VALUES (@UsuarioId, @LojaId, @Status, @DataCadastro);
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                UsuarioId = loja.UsuarioId,
                LojaId = loja.LojaId,
                Status = loja.Status,   
                DataCadastro = loja.DataCadastro
            }, _dbAgenda.Transaction);

            return retorno;
        }
        public async Task<Loja> BuscarLojaPorCNPJ(string cnpj, Guid usuarioId)
        {
            var sql = @"SELECT l.Id, l.RazaoSocial , l.CNPJ , l.Email, l.Telefone, l.Status, l.DataCadastro, l.DataAtualizacao
                        FROM Loja l
                        INNER JOIN UsuarioLoja ul ON ul.LojaId = l.Id 
                        WHERE l.CNPJ = @CNPJ AND ul.UsuarioId  <> @UsuarioId";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<Loja>(sql, new { Cnpj = cnpj, UsuarioId = usuarioId }) ?? new Loja();
            return retorno;
        }
    }
}
