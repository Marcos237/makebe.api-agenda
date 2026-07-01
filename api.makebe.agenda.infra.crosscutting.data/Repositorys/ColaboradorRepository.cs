using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ColaboradorRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<ColaboradorDTO> BuscarPorUsuarioId(Guid id)
        {
            var sql = @"SELECT c.Id, c.UsuarioId , c.DataCadastro , c.DataAtualizacao ,c.Status  FROM ContaColaborador cc 
                        INNER JOIN Colaborador c  ON cc.ColaboradorId  = c.Id 
                        WHERE c.UsuarioId  = @UsuarioId";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<ColaboradorDTO>(sql, new { UsuarioId = id }) ?? new ColaboradorDTO();
            return retorno;
        }
        public async Task<ColaboradorDTO> BuscarPorId(int id)
        {

            var sql = @"SELECT c.Id, CAST(c.UsuarioId AS CHAR) AS UsuarioCodigo, c.DataAtualizacao, c.Status FROM Colaborador c WHERE c.Id = @Id AND c.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<ColaboradorDTO>(sql, new { Id = id }) ?? new ColaboradorDTO();
            return retorno;

        }

        public async Task<int> Salvar(Colaborador colaborador)
        {
            var sql = @"                      
                        INSERT INTO Colaborador (UsuarioId, DataCadastro, DataAtualizacao, Status) VALUES  (@UsuarioId, @DataCadastro, @DataAtualizacao, @Status);                     
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                UsuarioId = colaborador.UsuarioId.ToString(),
                DataCadastro = colaborador.Datacadastro,
                DataAtualizacao = colaborador.DataAtualizacao,
                Status = colaborador.Status
            }, _dbAgenda.Transaction);
            return retorno;

        }
        public async Task<Colaborador> Atualizar(Colaborador colaborador)
        {
            var sql = @"                      
                        UPDATE Colaborador SET 
                             DataAtualizacao  = @DataAtualizacao,
                             Status  = @Status
                        WHERE Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                DataAtualizacao = colaborador.DataAtualizacao,
                Status = colaborador.Status,
                Id = colaborador.Id
            }, _dbAgenda.Transaction);
            return colaborador;
        }
        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE Colaborador SET 
                      Status = false 
                      Where Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            { Id = id }) > 0;
            return retorno;
        }

        public async Task<PaginacaoDTO<ColaboradorDTO>> BuscarPaginadoPorConta(string usuarioId, PaginacaoDTO<ColaboradorDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;
            var sql = await BuscarConsultaPorConta();
            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                ContaId = usuarioId,
                Status = paginacao?.objetoPesquisa?.Status,
                Nome = paginacao?.objetoPesquisa?.Nome,
                Cpf = paginacao?.objetoPesquisa?.Cpf,
                Email = paginacao?.objetoPesquisa?.Email,
                PermissaoId = paginacao?.objetoPesquisa?.PermissaoId
            };
            var lojas = await _dbAgenda.Connection.QueryAsync<ColaboradorDTO>(sql, parametros) ?? Enumerable.Empty<ColaboradorDTO>();
            paginacao!.total = lojas.Count();

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao!.objetos = await _dbAgenda.Connection.QueryAsync<ColaboradorDTO>(sqlBusca, param: parametros) ?? Enumerable.Empty<ColaboradorDTO>();

            return paginacao;
        }

        public async Task<PaginacaoDTO<ColaboradorDTO>> BuscarPaginadoPorUsuario(string usuarioId, PaginacaoDTO<ColaboradorDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;
            var sql = await BuscarConsultaPorUsuario();
            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                UsuarioId = usuarioId,
                Status = paginacao?.objetoPesquisa?.Status,
                Nome = paginacao?.objetoPesquisa?.Nome,
                Cpf = paginacao?.objetoPesquisa?.Cpf,
                Email = paginacao?.objetoPesquisa?.Email,
                PermissaoId = paginacao?.objetoPesquisa?.PermissaoId
            };
            var colaboradores = await _dbAgenda.Connection.QueryAsync<ColaboradorDTO>(sql, parametros) ?? Enumerable.Empty<ColaboradorDTO>();
            paginacao.total = colaboradores.Count();

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao.objetos = await _dbAgenda.Connection.QueryAsync<ColaboradorDTO>(sqlBusca, param: parametros) ?? Enumerable.Empty<ColaboradorDTO>();

            return paginacao;
        }

        private Task<string> BuscarConsultaPorConta()
        {
            var query = @"SELECT     
                             Id,
                            UsuarioId,
                            Nome,
                            Email,
                            Cpf,
                            Telefone,
                            Instagran,
                            CAST(PermissaoId AS CHAR) AS PermissaoId,
                            MostrarVitrine,
                            Status,
                            UrlImagem,
                            NomeImagem,
                            DescricaoPermissao,
                            ContaId  
                        FROM vw_colaborador
                        WHERE ContaId  = @ContaId
                        AND (@Status IS NULL OR Status = @Status)
                        AND (Nome LIKE CONCAT('%', @Nome, '%') OR @Nome IS NULL OR @Nome = '') 
                        AND (Cpf LIKE CONCAT('%', @Cpf, '%') OR @Cpf IS NULL OR @Cpf = '')
                        AND (Email LIKE CONCAT('%', @Email, '%') OR @Email IS NULL OR @Email = '')
                        AND (PermissaoId LIKE CONCAT('%', @PermissaoId, '%') OR @PermissaoId IS NULL OR @PermissaoId = '00000000-0000-0000-0000-000000000000')
                        AND PermissaoId IN ('70A54CCD-8124-4BCE-AEC1-4913A37BAE8E', 'FFBFA665-0370-4953-8A33-3C1B1D87A091', '4391AA5D-65C9-4523-B401-0337D1F4FCED')
                        ORDER BY Nome ";
            return Task.FromResult(query);
        }

        private Task<string> BuscarConsultaPorUsuario()
        {
            var query = @"SELECT
                            Id,
                            UsuarioId,
                            Nome,
                            Email,
                            Cpf,
                            Telefone,
                            Instagran,
                            CAST(PermissaoId AS CHAR) AS PermissaoId,
                            MostrarVitrine,
                            Status,
                            UrlImagem,
                            NomeImagem,
                            DescricaoPermissao,
                            ContaId
                        FROM vw_colaborador vc
                        WHERE vc.UsuarioId = @UsuarioId
                        AND vc.Status = 1
                        AND (@Status IS NULL OR vc.Status = @Status)
                        AND (vc.Nome LIKE CONCAT('%', @Nome, '%') OR @Nome IS NULL OR @Nome = '')
                        AND (vc.Cpf LIKE CONCAT('%', @Cpf, '%') OR @Cpf IS NULL OR @Cpf = '')
                        AND (vc.Email LIKE CONCAT('%', @Email, '%') OR @Email IS NULL OR @Email = '')
                        AND (vc.PermissaoId LIKE CONCAT('%', @PermissaoId, '%') OR @PermissaoId IS NULL OR @PermissaoId = '00000000-0000-0000-0000-000000000000')
                        ORDER BY vc.Nome ";
            return Task.FromResult(query);
        }

        public async Task<IEnumerable<ColaboradorDTO>> BuscarPorConta(string usuarioId)
        {
            var sql = @"SELECT     
                             Id,
                            UsuarioId,
                            Nome,
                            Email,
                            Cpf,
                            Telefone,
                            Instagran,
                            CAST(PermissaoId AS CHAR) AS PermissaoId,
                            MostrarVitrine,
                            Status,
                            UrlImagem,
                            NomeImagem,
                            DescricaoPermissao,
                            ContaId   
                        FROM vw_colaborador
                        WHERE ContaId  = @ContaId ORDER BY Nome; ";
            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorDTO>(sql, new { ContaId = usuarioId });
            return retorno;
        }
    }
}
