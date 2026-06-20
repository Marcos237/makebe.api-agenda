using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class PortifolioImagemRepository : IPortifolioImagemRepository
    {
        private readonly DbAgenda _dbAgenda;
        public PortifolioImagemRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<IEnumerable<PortifolioImagemDTO>> BuscarImagensPorIdPortifolio(int id)
        {
            var sql = @"SELECT * FROM PortifolioImagens WHERE PortifolioId = @PortifolioId AND Status = 1 ORDER BY DataCadastro DESC;";
            var result = await _dbAgenda.Connection.QueryAsync<PortifolioImagemDTO>(sql, new { PortifolioId = id }) ?? Enumerable.Empty<PortifolioImagemDTO>();
            return result;
        }
        public async Task<PortifolioImagemDTO> BuscarImagensPorId(int id)
        {
            var sql = @"SELECT * FROM PortifolioImagens WHERE PortifolioId = @ID AND Status = 1;";
            var result = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<PortifolioImagemDTO>(sql, new { id = id }) ?? new PortifolioImagemDTO();
            return result;
        }
        public async Task<IEnumerable<ColaboradorPortifolioImagemDTO>> BuscarImagensPorColaboradorId(int id)
        {
            var sql = @"SELECT DISTINCT
	                        t.NomeImagem, 
	                        t.UrlImagem, 
	                        t.TituloImagem, 
	                        cp.ColaboradorId,
	                        CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                            p.Texto 
                        FROM ColaboradorProfissional cp 
                        INNER JOIN Colaborador c On c.Id  = cp.ColaboradorId 
                        INNER JOIN ColaboradorPortifolio cp2 ON cp2.ColaboradorId  = cp.ColaboradorId 
                        INNER JOIN Portifolio p ON p.Id  = cp2.PortifolioId 
                        INNER JOIN PortifolioImagens t ON t.PortifolioId = p.Id 
                        WHERE t.Status = 1 AND cp.ColaboradorId  = @Id";

            var result = await _dbAgenda.Connection.QueryAsync<ColaboradorPortifolioImagemDTO>(sql, new { Id = id })
                ?? Enumerable.Empty<ColaboradorPortifolioImagemDTO>();

            return result;
        }
        public async Task<int> Salvar(PortifolioImagens PortifolioImagens)
        {
            var sql = @"INSERT INTO PortifolioImagens (PortifolioId, TituloImagem, UrlImagem, NomeImagem, Status, DataCadastro, DataAtualizacao)
                                            VALUES (@PortifolioId, @TituloImagem, @UrlImagem, @NomeImagem, @Status, @DataCadastro, @DataAtualizacao)";
            var parametros = new
            {
                PortifolioId = PortifolioImagens.PortifolioId,
                TituloImagem = PortifolioImagens.TituloImagem,
                UrlImagem = PortifolioImagens.Imagem!.UrlImagem,
                NomeImagem = PortifolioImagens.Imagem.NomeArquivo,
                Status = PortifolioImagens.Status,
                DataCadastro = PortifolioImagens.DataCadastro,
                DataAtualizacao = PortifolioImagens.DataAtualizacao,
            };
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros, _dbAgenda.Transaction);
            return result;
        }
        public async Task<PortifolioImagens> Atualizar(PortifolioImagens PortifolioImagens)
        {
            var sql = @"UPDATE  PortifolioImagens SET
                                TituloImagem = @TituloImagem, 
                                UrlImagem = @UrlImagem, 
                                NomeImagem = @NomeImagem, 
                                DataAtualizacao = @DataAtualizacao
                                WHERE Id = @Id";
            var parametros = new
            {
                PortifolioId = PortifolioImagens.PortifolioId,
                TituloImagem = PortifolioImagens.TituloImagem,
                UrlImagem = PortifolioImagens.Imagem!.UrlImagem,
                NomeImagem = PortifolioImagens.Imagem.NomeArquivo,
                Status = PortifolioImagens.Status,
                DataCadastro = PortifolioImagens.DataCadastro,
                DataAtualizacao = PortifolioImagens.DataAtualizacao,
            };
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros, _dbAgenda.Transaction);
            return PortifolioImagens;
        }
        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE  PortifolioImagens SET
                                Status = 0
                                WHERE PortifolioId = @Id";
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new { Id = id }) > 0;
            return result;
        }
    }
}
