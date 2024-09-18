using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.ValueObjects;
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

        public async Task<IEnumerable<LojaEnderecoDTO>> BuscarLojas(PaginacaoDTO<LojaEnderecoDTO> paginacao, string usuarioId)
        {
            string countQuery = "SELECT COUNT(*) FROM Loja l INNER JOIN UsuarioLoja ul ON ul.LojaId = l.Id WHERE UsuarioId = @UsuarioId";
            paginacao!.total = await _dbAgenda.Connection.ExecuteScalarAsync<int>(countQuery, new { UsuarioId = usuarioId });
            paginacao.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            string sql = @"SELECT l.Id, l.RazaoSocial, l.Email, l.Telefone, l.Status, l.DataCadastro, l.DataAtualizacao,
                     le.EnderecoId, e.Logradouro, e.Numero, e.Complemento, e.CEP, e.Estado, e.Cidade, l.CNPJ
                FROM Loja l
            INNER JOIN UsuarioLoja ul ON ul.LojaId = l.Id  
            LEFT JOIN LojaEndereco le ON le.LojaId = l.Id 
            LEFT JOIN Endereco e ON e.Id = le.EnderecoId
                WHERE (@RazaoSocial IS NULL OR l.RazaoSocial LIKE CONCAT('%', @RazaoSocial, '%')) 
                  AND (@CNPJ IS NULL OR l.CNPJ LIKE CONCAT('%', @CNPJ, '%')) 
                  AND (@Email IS NULL OR l.Email LIKE CONCAT('%', @Email, '%'))
                  AND ul.UsuarioId = @UsuarioId
                  AND l.Status = 1
                ORDER BY l.RazaoSocial
                LIMIT @TamanhoPagina OFFSET @RegistroInicial";

            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                RazaoSocial = paginacao?.objetoPesquisa?.RazaoSocial,
                CNPJ = paginacao?.objetoPesquisa?.CNPJ,
                Email = paginacao?.objetoPesquisa?.Email,
                UsuarioId = usuarioId
            };

            paginacao!.objetos = await _dbAgenda.Connection.QueryAsync<LojaEnderecoDTO, Endereco, LojaEndereco, LojaEnderecoDTO>(
                           sql,
                (loja, endereco, lojaEndereco) =>
                {
                    var lojaExistente = paginacao?.objetos?.FirstOrDefault(l => l.Id == loja?.Id) ?? new LojaEnderecoDTO();

                    if (lojaExistente == null)
                        lojaExistente = loja;


                    if (endereco != null && lojaEndereco.EnderecoId == endereco.Id)
                        lojaExistente?.Enderecos?.ToList().Add(endereco);

                    return lojaExistente ?? new LojaEnderecoDTO();
                },
                splitOn: "EnderecoId, CNPJ",
                param: parametros) ?? Enumerable.Empty<LojaEnderecoDTO>();

            return paginacao.objetos;
        }

        public async Task<Loja> BuscarLojaPorCodigo(int id)
        {
            var sql = @"SELECT l.Id, l.RazaoSocial , l.CNPJ , l.Email, l.Telefone, l.Status, l.DataCadastro, l.DataAtualizacao
                               FROM Loja l 
                      WHERE Id = @Id AND Status = 1";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<Loja>(sql, new { Id = id }) ?? new Loja();
            return retorno;
        }

        public async Task<int> Salvar(Loja loja)
        {
            var sql = @"                      
                      INSERT INTO Loja (Id, RazaoSocial , CNPJ , Email, Telefone, Status, DataCadastro, DataAtualizacao)
                      VALUES(@Id, @RazaoSocial , @CNPJ , @Email, @Telefone, @Status, @DataCadastro, @DataAtualizacao)
                      SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Id = loja?.Id,
                RazaoSocial = loja?.RazaoSocial,
                Cnpj = loja?.CNPJ?.Codigo,
                Email = loja?.Email,
                Telefone = loja?.Telefone,
                Status = loja?.Status,
                DataCadastro = loja?.DataCadastro,
                DataAtualizacao = loja?.DataAtualizacao
            });
            return retorno;

        }
        public async Task<Loja> Atualizar(Loja loja)
        {
            var sql = @"UPDATE Loja SET 
                      Id = @Id, 
                      RazaoSocial = @RazaoSocial, 
                      CNPJ = @CNPJ, 
                      Email =  @Email, 
                      Telefone  = @Telefone, 
                      Status = @Status, 
                      DataAtualizacao = @DataAtualizacao";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Id = loja?.Id,
                RazaoSocial = loja?.RazaoSocial,
                Cnpj = loja?.CNPJ?.Codigo,
                Email = loja?.Email,
                Telefone = loja?.Telefone,
                Status = loja?.Status,
                DataCadastro = loja?.DataCadastro,
                DataAtualizacao = loja?.DataAtualizacao
            });
            return loja!;
        }

    }
}
