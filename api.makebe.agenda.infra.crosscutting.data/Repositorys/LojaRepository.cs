using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class LojaRepository : ILojaRepository
    {
        private readonly DbAgenda _dbAgenda;
        public LojaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<LojaDTO>> BuscarTodos(string contaId)
        {
            var sql = @"SELECT DISTINCT l.* FROM Loja l
                                 INNER JOIN ContaLoja ul ON ul.LojaId  = l.Id 
                                 Where ul.ContaId  = @ContaId
                                 and l.Status  = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<LojaDTO>(sql, new { ContaId = contaId });
            return retorno;
        }

        public async Task<PaginacaoDTO<LojaDTO>> BuscarLojas(PaginacaoDTO<LojaDTO> paginacao, string contaId)
        {

            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;
            var sql = await BuscarConsulta();
            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                RazaoSocial = paginacao?.objetoPesquisa?.RazaoSocial,
                CNPJ = paginacao?.objetoPesquisa?.CNPJ,
                Email = paginacao?.objetoPesquisa?.Email,
                Telefone = paginacao?.objetoPesquisa?.Telefone,
                TipoLojaId = paginacao?.objetoPesquisa?.TipoLojaId,
                ContaId = contaId
            };
            var lojas = await _dbAgenda.Connection.QueryAsync<LojaDTO>(sql, parametros) ?? Enumerable.Empty<LojaDTO>();
            paginacao!.total = lojas.Count();

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao!.objetos = await _dbAgenda.Connection.QueryAsync<LojaDTO>(sqlBusca, param: parametros) ?? Enumerable.Empty<LojaDTO>();

            return paginacao;
        }

        public async Task<LojaDTO> BuscarLojaPorCodigo(int id)
        {
            var sql = @"SELECT l.Id, l.RazaoSocial , l.CNPJ , l.Email, l.Telefone, l.Status, l.DataCadastro, l.DataAtualizacao, l.TipoLojaId
                               FROM Loja l 
                      WHERE Id = @Id AND Status = 1";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<LojaDTO>(sql, new { Id = id }) ?? new LojaDTO();
            return retorno;
        }

        public async Task<int> Salvar(Loja loja)
        {
            var sql = @"                      
                        INSERT INTO Loja (RazaoSocial, CNPJ, Email, Telefone, Status, TipoLojaId, DataCadastro, DataAtualizacao)
                        VALUES (@RazaoSocial, @CNPJ, @Email, @Telefone, @Status, @TipoLojaId, @DataCadastro, @DataAtualizacao);
                        
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                RazaoSocial = loja?.RazaoSocial,
                Cnpj = loja?.CNPJ?.Codigo,
                Email = loja?.Email,
                Telefone = loja?.Telefone,
                Status = loja?.Status,
                TipoLojaId = loja?.TipoLojaId,
                DataCadastro = loja?.DataCadastro,
                DataAtualizacao = loja?.DataAtualizacao
            }, _dbAgenda.Transaction);
            return retorno;
        }
        public async Task<Loja> Atualizar(Loja loja)
        {
            var sql = @"UPDATE Loja SET 
                      RazaoSocial = @RazaoSocial, 
                      CNPJ = @CNPJ, 
                      Email =  @Email, 
                      Telefone  = @Telefone, 
                      Status = @Status, 
                      DataAtualizacao = @DataAtualizacao,
                      TipoLojaId  = @TipoLojaId
                      WHERE Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Id = loja?.Id,
                RazaoSocial = loja?.RazaoSocial,
                Cnpj = loja?.CNPJ?.Codigo,
                Email = loja?.Email,
                Telefone = loja?.Telefone,
                Status = loja?.Status,
                DataCadastro = loja?.DataCadastro,
                DataAtualizacao = loja?.DataAtualizacao,
                TipoLojaId = loja?.TipoLojaId
            });
            return loja!;
        }

        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE Loja SET 
                      Status = false 
                      Where Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            { Id = id }) > 0;
            return retorno;
        }

        private Task<string> BuscarConsulta()
        {
            var query = @"SELECT DISTINCT l.Id, l.RazaoSocial, l.Email, l.Telefone, l.Status, l.DataCadastro, l.DataAtualizacao, l.CNPJ,
                                   l.TipoLojaId, tl.Descricao as TipoLojaDescricao
                            FROM Loja l
                                 INNER JOIN ContaLoja ul ON ul.LojaId = l.Id  
                                 INNER JOIN TipoLoja tl ON tl.Id = l.TipoLojaId 
                            WHERE ul.ContaId = @ContaId
                              AND l.Status = 1
                              AND (
                                    (@RazaoSocial IS NULL OR @RazaoSocial = '' OR l.RazaoSocial LIKE CONCAT('%', @RazaoSocial, '%')) 
                                    AND (@CNPJ IS NULL OR @CNPJ = '' OR l.CNPJ LIKE CONCAT('%', @CNPJ, '%')) 
                                    AND (@Email IS NULL OR @Email = '' OR l.Email LIKE CONCAT('%', @Email, '%'))
                                    AND (@Telefone IS NULL OR @Telefone = '' OR l.Telefone LIKE CONCAT('%', @Telefone, '%'))
                                    AND (@TipoLojaId IS NULL OR @TipoLojaId = 0 OR l.TipoLojaId = @TipoLojaId)
                                  )
                            ORDER BY l.Id DESC";
            return Task.FromResult(query);
        }
    }
}
