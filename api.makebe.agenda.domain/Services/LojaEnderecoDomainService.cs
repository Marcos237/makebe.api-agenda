using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.data.Repositorys;

namespace api.makebe.agenda.domain.Services
{
    public class LojaEnderecoDomainService : ILojaEnderecoDomainService
    {
        private readonly ILojaEnderecoRepository _lojaEnderecoRepository;
        public LojaEnderecoDomainService(ILojaEnderecoRepository lojaEnderecoRepository)
        {
            _lojaEnderecoRepository = lojaEnderecoRepository;
        }
        public async Task<int> SalvarLojaEndereco(LojaEndereco endereco)
        {
            endereco.DataCadastro = DateTime.Now;

            if(endereco.EnderecoId == 0 ) 
            return await _lojaEnderecoRepository.SalvarLojaEndereco(endereco);

            await _lojaEnderecoRepository.AtualizaLojaEndereco(endereco);
            return endereco.Id;
        }
    }
}
