using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class LojaPortifolioImagemRepository : ILojaPortifolioImagemRepository
    {
        private readonly DbAgenda _dbAgenda;
        public LojaPortifolioImagemRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<IEnumerable<LojaPortifolioImagemDTO>> BuscarImagensPorIdLojaPortifolio(int id)
        {
            var sql = @"SELECT * FROM LojaPortifolioImagens WHERE LojaPortifolioId = @LojaPortifolioId AND Status = 1 ORDER BY DataCadastro DESC;";
            var result = await _dbAgenda.Connection.QueryAsync<LojaPortifolioImagemDTO>(sql, new { LojaPortifolioId = id }) ?? Enumerable.Empty<LojaPortifolioImagemDTO>();
            return result;
        }
        public async Task<LojaPortifolioImagemDTO> BuscarImagensPorId(int id)
        {
            var sql = @"SELECT * FROM LojaPortifolioImagens WHERE LojaPortifolioId = @ID AND Status = 1;";
            var result = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<LojaPortifolioImagemDTO>(sql, new { id = id }) ?? new LojaPortifolioImagemDTO();
            return result;
        }
        public async Task<int> Salvar(LojaPortifolioImagens lojaPortifolioImagens)
        {
            var sql = @"INSERT INTO LojaPortifolioImagens (LojaPortifolioId, TituloImagem, UrlImagem, NomeImagem, Status, DataCadastro, DataAtualizacao)
                                            VALUES (@LojaPortifolioId, @TituloImagem, @UrlImagem, @NomeImagem, @Status, @DataCadastro, @DataAtualizacao)";
            var parametros = new
            {
                LojaPortifolioId = lojaPortifolioImagens.LojaPortifolioId,
                TituloImagem = lojaPortifolioImagens.TituloImagem,
                UrlImagem = lojaPortifolioImagens.Imagem!.UrlImagem,
                NomeImagem = lojaPortifolioImagens.Imagem.NomeArquivo,
                Status = lojaPortifolioImagens.Status,
                DataCadastro = lojaPortifolioImagens.DataCadastro,
                DataAtualizacao = lojaPortifolioImagens.DataAtualizacao,
            };
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros);
            return result;
        }
        public async Task<LojaPortifolioImagens> Atualizar(LojaPortifolioImagens lojaPortifolioImagens)
        {
            var sql = @"UPDATE  LojaPortifolioImagens SET
                                TituloImagem = @TituloImagem, 
                                UrlImagem = @UrlImagem, 
                                NomeImagem = @NomeImagem, 
                                DataAtualizacao = @DataAtualizacao
                                WHERE Id = @Id";
            var parametros = new
            {
                LojaPortifolioId = lojaPortifolioImagens.LojaPortifolioId,
                TituloImagem = lojaPortifolioImagens.TituloImagem,
                UrlImagem = lojaPortifolioImagens.Imagem!.UrlImagem,
                NomeImagem = lojaPortifolioImagens.Imagem.NomeArquivo,
                Status = lojaPortifolioImagens.Status,
                DataCadastro = lojaPortifolioImagens.DataCadastro,
                DataAtualizacao = lojaPortifolioImagens.DataAtualizacao,
            };
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros);
            return lojaPortifolioImagens;
        }
        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE  LojaPortifolioImagens SET
                                Status = 0
                                WHERE LojaPortifolioId = @Id";
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new { Id = id }) > 0;
            return result;
        }
    }
}
