using api.makebe.agenda.domain.DTO;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace api.makebe.agenda.domain.Entidades
{
    public class UsuarioSessao
    {
        public Guid Id { get; set; }
        public string? Chave { get; set; }
        public Guid UsuarioId { get; set; }
        public string? UrlImagem { get; set; }
        public string? NomeImagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataExpiracaoToken { get; set; }
        public string? Nome { get; set; }
        public Guid PermissaoId { get; set; }
        public IEnumerable<PermissaoMenuDTO>? Menus { get; set; }

        public async Task<UsuarioSessao> TranformarHasEntriesSessao(HashEntry[] hashEntries)
        {
            var sessao = new UsuarioSessao();
            var id = string.Empty;
            var dataHashEntry = string.Empty;
            var dataExpiracaoHashEntry = string.Empty;
            var usuarioId = string.Empty;
            var permissaoId = string.Empty;
            var menus = string.Empty;
            foreach (var hashEntry in hashEntries)
            {
                RedisValue retorno = hashEntry.Name.ToString() switch
                {
                    nameof(Id) => id = hashEntry.Value,
                    nameof(Chave) => sessao.Chave = hashEntry.Value.ToString(),
                    nameof(UrlImagem) => sessao.UrlImagem = hashEntry.Value.ToString(),
                    nameof(NomeImagem) => sessao.NomeImagem = hashEntry.Value.ToString(),
                    nameof(UsuarioId) => usuarioId = hashEntry.Value,
                    nameof(PermissaoId) => permissaoId = hashEntry.Value,
                    nameof(DataCadastro) => dataHashEntry = hashEntry.Value.ToString(),
                    nameof(DataExpiracaoToken) => dataExpiracaoHashEntry = hashEntry.Value.ToString(),
                    nameof(Nome) => sessao.Nome = hashEntry.Value,
                    nameof(Menus) => menus = hashEntry.Value.ToString(),
                    _ => string.Empty
                };
            }
            if (hashEntries.Any())
            {
                if (DateTime.TryParse(dataHashEntry, out var dataCadastro))
                    sessao.DataCadastro = dataCadastro;
                if (DateTime.TryParse(dataExpiracaoHashEntry, out var dataExpiracao))
                    sessao.DataExpiracaoToken = dataExpiracao;
                sessao.Chave = usuarioId;
                sessao.UsuarioId = Guid.Parse(usuarioId!);
                sessao.Id = Guid.Parse(id);
                sessao.PermissaoId = Guid.Parse(permissaoId!);
                sessao.Menus = await AdicionandoListaMenuSessao(menus);
            }

            return sessao;
        }
        public async Task<IEnumerable<PermissaoMenuDTO>> AdicionandoListaMenuSessao(string menuEntry)
        {
            var menu = JsonConvert.DeserializeObject<IEnumerable<PermissaoMenuDTO>>(menuEntry) ?? Enumerable.Empty<PermissaoMenuDTO>();
            return await Task.FromResult(menu);
        }
    }
}
