using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorProfissionalRepository : IColaboradorProfissionalRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ColaboradorProfissionalRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorContaId(string contaId)
        {
            var sql = @"SELECT DISTINCT 
                                c.Id,
                                cp.ColaboradorId, 
                                CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                                cp.LojaId, 
                                l.RazaoSocial, 
                                cp.ServicoId, 
                                s.Descricao as DescricaoServico, 
                                cp.Descricao, 
                                cp.DataCadastro, 
                                cc.ContaId 
                       FROM ColaboradorProfissional cp
                       INNER JOIN Colaborador c ON c.Id  = cp.ColaboradorId 
                       INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                       INNER JOIN Loja l on l.Id = cp.LojaId 
                       INNER JOIN Servicos s ON s.Id  = cp.ServicoId 
                       WHERE 
                              cc.ContaId = @ContaId
                       AND    cp.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorProfissionalDTO>(
                sql,
                new { ContaId = contaId }
            ) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();

            return retorno;
        }
        public async Task<ColaboradorProfissionalDTO> BuscarPorId(int id)
        {
            var sql = @"SELECT cp.Id, cp.ColaboradorId, cp.LojaId, l.RazaoSocial, cp.ServicoId, s.Descricao as DescricaoServico, cp.Descricao  FROM ColaboradorProfissional cp
                            INNER JOIN Colaborador c ON c.Id  = cp.ColaboradorId 
                            INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                            INNER JOIN Loja l on l.Id = cp.LojaId 
                            INNER JOIN Servicos s ON s.Id  = cp.ServicoId 
                            WHERE cp.Status = 1
                            AND cp.Id = @Id";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<ColaboradorProfissionalDTO>(sql, new { Id = id }) ?? new ColaboradorProfissionalDTO();
            return retorno;
        }
        public async Task<int> Salvar(ColaboradorProfissional colaborador)
        {
            var sql = @"INSERT INTO ColaboradorProfissional (ColaboradorId, LojaId, ServicoId, Descricao, Status, DataCadastro, DataAtualizacao) VALUES
                                   (@ColaboradorId, @LojaId, @ServicoId, @Descricao, @Status, @DataCadastro, @DataAtualizacao);
                                    SELECT LAST_INSERT_ID();";

            var parametros = new
            {
                ColaboradorId = colaborador.ColaboradorId,
                LojaId = colaborador.LojaId,
                ServicoId = colaborador.ServicoId,
                Descricao = colaborador.Descricao,
                Status = colaborador.Status,
                DataCadastro = colaborador.DataCadastro,
                DataAtualizacao = colaborador.DataAtualizacao
            };
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros, _dbAgenda.Transaction);
            return retorno;
        }


        public async Task<bool> Atualizar(ColaboradorProfissional colaborador)
        {
            var sql = @"UPDATE ColaboradorProfissional
                            SET 
                                ColaboradorId = @ColaboradorId,
                                LojaId = @LojaId,
                                ServicoId = @ServicoId,
                                Descricao = @Descricao,
                                DataAtualizacao = @DataAtualizacao
                            WHERE 
                                Id = @Id;";
            var parametros = new
            {
                ColaboradorId = colaborador.ColaboradorId,
                LojaId = colaborador.LojaId,
                ServicoId = colaborador.ServicoId,
                Descricao = colaborador.Descricao,
                DataCadastro = colaborador.DataCadastro,
                DataAtualizacao = colaborador.DataAtualizacao,
                Id = colaborador.Id,
            };
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, parametros) > 0;
            return retorno;
        }
        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE ColaboradorProfissional SET 
                      Status = false 
                      Where Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            { Id = id }) > 0;
            return retorno;
        }
    }
}
