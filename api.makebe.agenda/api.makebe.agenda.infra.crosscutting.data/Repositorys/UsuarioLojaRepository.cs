using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class UsuarioLojaRepository : IUsuarioLojaRepository
    {
        private readonly DbAgenda _dbAgenda;
        public UsuarioLojaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
            _dbAgenda.GetConnection(DataBaseConstant.AgendaBase);
        }
        public async Task<int> Salvar(UsuarioLoja loja)
        {
            var sql = @"INSERT INTO UsuarioLoja (UsuarioId, LojaId, Status, DataCadastro) VALUES (UsuarioId, LojaId, Status, DataCadastro)
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, loja);

            return retorno;
        }
    }
}
